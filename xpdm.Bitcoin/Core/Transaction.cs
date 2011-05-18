using C5;
using SCG = System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin.Core
{
    public sealed class Transaction : BitcoinObject, IFreezable<Transaction>
    {
        private uint _version;
        public uint Version
        {
            get { return _version; }
            set
            {
                ContractsCommon.NotFrozen(this);

                _version = value;
                InvalidateBitcoinHashes();
            }
        }
        public IList<TransactionInput> TransactionInputs { get; private set; }
        public IList<TransactionOutput> TransactionOutputs { get; private set; }
        private uint _lockTime;
        public uint LockTime
        {
            get { return _lockTime; }
            set
            {
                ContractsCommon.NotFrozen(this);

                _lockTime = value;
                InvalidateBitcoinHashes();
            }
        }

        public Transaction()
        {
            var transactionInputs = new ArrayList<TransactionInput>();
            transactionInputs.CollectionChanged += InputOutputChanged;
            TransactionInputs = transactionInputs;

            var transactionOutputs = new ArrayList<TransactionOutput>();
            transactionOutputs.CollectionChanged += InputOutputChanged;
            TransactionOutputs = transactionOutputs;            
        }
        public Transaction(Transaction tx) : this(tx, false) { }
        public Transaction(Transaction tx, bool thawChildren)
        {
            ContractsCommon.NotNull(tx, "tx");
            ContractsCommon.ChildrenThawed(TransactionInputs, thawChildren);
            ContractsCommon.ChildrenThawed(TransactionOutputs, thawChildren);

            Version = tx.Version;

            var transactionInputs = new ArrayList<TransactionInput>(tx.TransactionInputs.Count);
            transactionInputs.AddAll(FreezableExtensions.ThawChildren(tx.TransactionInputs, thawChildren));
            transactionInputs.CollectionChanged += InputOutputChanged;
            TransactionInputs = transactionInputs;

            var transactionOutputs = new ArrayList<TransactionOutput>(tx.TransactionOutputs.Count);
            transactionOutputs.AddAll(FreezableExtensions.ThawChildren(tx.TransactionOutputs, thawChildren));
            transactionOutputs.CollectionChanged += InputOutputChanged;
            TransactionOutputs = transactionOutputs;

            LockTime = tx.LockTime;
        }

        public Transaction(Stream stream) : base(stream) { }
        public Transaction(byte[] buffer, int offset) : base(buffer, offset) { }

        public void InputOutputChanged(object sender)
        {
            ContractsCommon.NotFrozen(this);

            InvalidateBitcoinHashes(sender, EventArgs.Empty);
        }

        protected override void Deserialize(Stream stream)
        {
            Version = ReadUInt32(stream);

            var transactionInputsArr = ReadVarArray<TransactionInput>(stream);
            var transactionInputs = new HashedArrayList<TransactionInput>(transactionInputsArr.Length);
            transactionInputs.AddAll(transactionInputsArr);
            transactionInputs.CollectionChanged += InputOutputChanged;
            TransactionInputs = transactionInputs;

            var transactionOutputsArr = ReadVarArray<TransactionOutput>(stream);
            var transactionOutputs = new HashedArrayList<TransactionOutput>(transactionOutputsArr.Length);
            transactionOutputs.AddAll(transactionOutputsArr);
            transactionOutputs.CollectionChanged += InputOutputChanged;
            TransactionOutputs = transactionOutputs;

            LockTime = ReadUInt32(stream);

            Freeze();
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
                tiSize += VarIntByteSize(tiSize);
                var toSize = TransactionOutputs.Sum(to => to.SerializedByteSize);
                toSize += VarIntByteSize(toSize);
                return BufferOperations.UINT32_SIZE*2 + tiSize + toSize;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} [ {{{1}}} ] => [ {{{2}}} ] @ {3}", 
                Version, string.Join("}, {", TransactionInputs), string.Join("}, {", TransactionOutputs), LockTime);
        }
        public bool IsFrozen { get; private set; }

        public void Freeze()
        {
            if (!TransactionInputs.IsReadOnly)
            {
                TransactionInputs = new GuardedList<TransactionInput>(TransactionInputs);
            }
            foreach (var txIn in TransactionInputs)
            {
                txIn.Freeze();
            }

            if (!TransactionOutputs.IsReadOnly)
            {
                TransactionOutputs = new GuardedList<TransactionOutput>(TransactionOutputs);
            }
            foreach (var txOut in TransactionOutputs)
            {
                txOut.Freeze();
            }

            IsFrozen = true;
        }

        public Transaction Thaw()
        {
            return new Transaction(this, false);
        }

        public Transaction ThawTree()
        {
            ContractsCommon.IsThawed(Contract.Result<Transaction>().TransactionInputs);
            ContractsCommon.IsThawed(Contract.Result<Transaction>().TransactionOutputs);

            return new Transaction(this, true);
        }

        static Transaction()
        {
            var empty = new Transaction();
            empty.Freeze();
            _empty = empty;
        }

        private static readonly Transaction _empty;
        public static Transaction Empty { get { return _empty; } }

        [ContractInvariantMethod]
        private void __Invariant()
        {
            Contract.Invariant(TransactionInputs != null);
            Contract.Invariant(!IsFrozen || TransactionInputs.IsReadOnly);
            Contract.Invariant(TransactionOutputs != null);
            Contract.Invariant(!IsFrozen || TransactionOutputs.IsReadOnly);
        }
    }
}
