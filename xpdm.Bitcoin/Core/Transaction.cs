using C5;
using SCG = System.Collections.Generic;
using System.IO;
using System.Linq;

namespace xpdm.Bitcoin.Core
{
    public sealed class Transaction : BitcoinObject
    {
        public uint Version { get; private set; }
        public ICollection<TransactionInput> TransactionInputs { get; private set; }
        public ICollection<TransactionOutput> TransactionOutputs { get; private set; }
        public uint LockTime { get; private set; }

        public Transaction(uint version, SCG.IEnumerable<TransactionInput> inputs, SCG.IEnumerable<TransactionOutput> outputs, uint lockTime)
        {
            Version = version;
            var transactionInputs = new ArrayList<TransactionInput>();
            transactionInputs.AddAll(inputs);
            TransactionInputs = new GuardedCollection<TransactionInput>(transactionInputs);
            var transactionOutputs = new ArrayList<TransactionOutput>();
            transactionOutputs.AddAll(outputs);
            TransactionOutputs = new GuardedCollection<TransactionOutput>(transactionOutputs);
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
            TransactionInputs = new GuardedCollection<TransactionInput>(transactionInputs);
            var transactionOutputsArr = ReadVarArray<TransactionOutput>(stream);
            var transactionOutputs = new WrappedArray<TransactionOutput>(transactionOutputsArr);
            TransactionOutputs = new GuardedCollection<TransactionOutput>(transactionOutputs);
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
                return BufferOperations.UINT32_SIZE*2 + TransactionInputs.Sum(ti => ti.SerializedByteSize) +
                       TransactionOutputs.Sum(ti => ti.SerializedByteSize);
            }
        }
    }
}
