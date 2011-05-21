using C5;
using SCG = System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin.Core
{
    public sealed class Transaction : BitcoinObject
    {
        public uint Version { get; private set; }
        public IList<TransactionInput> TransactionInputs { get; private set; }
        public IList<TransactionOutput> TransactionOutputs { get; private set; }
        public uint LockTime { get; private set; }

        [Pure]
        public TransactionInput GetInput(int index)
        {
            Contract.Requires<IndexOutOfRangeException>(0 <= index && index < TransactionInputs.Count);

            return TransactionInputs[index];
        }

        [Pure]
        public TransactionOutput GetOutput(int index)
        {
            Contract.Requires<IndexOutOfRangeException>(0 <= index && index < TransactionOutputs.Count);

            return TransactionOutputs[index];
        }

        public Transaction(uint version, SCG.IEnumerable<TransactionInput> inputs, SCG.IEnumerable<TransactionOutput> outputs, uint lockTime)
        {
            Version = version;
            var transactionInputs = new ArrayList<TransactionInput>();
            transactionInputs.AddAll(inputs);
            TransactionInputs = new GuardedList<TransactionInput>(transactionInputs);
            var transactionOutputs = new ArrayList<TransactionOutput>();
            transactionOutputs.AddAll(outputs);
            TransactionOutputs = new GuardedList<TransactionOutput>(transactionOutputs);
            LockTime = lockTime;
        }

        public Transaction() { }
        public Transaction(Stream stream) : base(stream) { }
        public Transaction(byte[] buffer, int offset) : base(buffer, offset) { }

        protected override void Deserialize(Stream stream)
        {
            Version = ReadUInt32(stream);
            var transactionInputsArr = ReadVarArray<TransactionInput>(stream);
            var transactionInputs = new WrappedArray<TransactionInput>(transactionInputsArr);
            TransactionInputs = new GuardedList<TransactionInput>(transactionInputs);
            var transactionOutputsArr = ReadVarArray<TransactionOutput>(stream);
            var transactionOutputs = new WrappedArray<TransactionOutput>(transactionOutputsArr);
            TransactionOutputs = new GuardedList<TransactionOutput>(transactionOutputs);
            LockTime = ReadUInt32(stream);
        }

        public override void Serialize(Stream stream)
        {
            Write(stream, Version);
            WriteCollection(stream, TransactionInputs);
            WriteCollection(stream, TransactionOutputs);
            Write(stream, LockTime);
        }

        public override int SerializedByteSize
        {
            get
            {
                var tiSize = TransactionInputs.Sum(ti => ti.SerializedByteSize);
                tiSize += VarIntByteSize(TransactionInputs.Count);
                var toSize = TransactionOutputs.Sum(to => to.SerializedByteSize);
                toSize += VarIntByteSize(TransactionOutputs.Count);
                return BufferOperations.UINT32_SIZE*2 + tiSize + toSize;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} [ {{{1}}} ] => [ {{{2}}} ] @ {3}", 
                Version, string.Join("}, {", TransactionInputs), string.Join("}, {", TransactionOutputs), LockTime);
        }
    }
}
