using System.Diagnostics.Contracts;
using System.IO;

namespace xpdm.Bitcoin.Core
{
    public sealed class TransactionInput : BitcoinObject, IFreezable, IThawable<TransactionInput>
    {
        private TransactionOutpoint _source = TransactionOutpoint.Empty;
        public TransactionOutpoint Source
        {
            get
            {
                ContractsCommon.ResultIsNonNull<TransactionOutpoint>();

                return _source;
            }
            set
            {
                ContractsCommon.NotFrozen(this);
                ContractsCommon.NotNull(value, "value");

                _source.HashesInvalidated -= InvalidateBitcoinHashes;
                _source = value;
                _source.HashesInvalidated += InvalidateBitcoinHashes;
                InvalidateBitcoinHashes();
            }
        }

        private Script _script = Script.Empty;
        public Script Script
        {
            get
            {
                ContractsCommon.ResultIsNonNull<Script>();

                return _script;
            }
            set
            {
                ContractsCommon.NotFrozen(this);
                ContractsCommon.NotNull(value, "value");

                _script.HashesInvalidated -= InvalidateBitcoinHashes;
                _script = value;
                _script.HashesInvalidated += InvalidateBitcoinHashes;
                InvalidateBitcoinHashes();
            }
        }

        private uint _sequenceNumber = DefaultSequenceNumber;
        public uint SequenceNumber
        {
            get
            {
                return _sequenceNumber;
            }
            set
            {
                ContractsCommon.NotFrozen(this);

                _sequenceNumber = value;
                InvalidateBitcoinHashes();
            }
        }

        private const uint DefaultSequenceNumber = 0xFFFFFFFFU;

        public TransactionInput()
        {
        }
        public TransactionInput(TransactionInput txIn) : this(txIn, false) { }
        public TransactionInput(TransactionInput txIn, bool thawChildren)
        {
            ContractsCommon.NotNull(txIn, "txIn");
            ContractsCommon.ChildThawed(Source, thawChildren);
            ContractsCommon.ChildThawed(Script, thawChildren);

            _source = FreezableExtensions.ThawChild(txIn._source, thawChildren);
            _script = FreezableExtensions.ThawChild(txIn._script, thawChildren);
            _sequenceNumber = txIn._sequenceNumber;
        }

        public TransactionInput(Stream stream) : base(stream) { }
        public TransactionInput(byte[] buffer, int offset) : base(buffer, offset) { }

        public bool IsCoinbase
        {
            get
            {
                return Source.IsCoinbase;
            }
        }

        protected override void Deserialize(Stream stream)
        {
            Source = new TransactionOutpoint(stream);
            Script = new Script(stream);
            SequenceNumber = ReadUInt32(stream);

            Freeze();
        }

        public override void Serialize(Stream stream)
        {
            Source.Serialize(stream);
            Script.Serialize(stream);
            Write(stream, SequenceNumber);
        }

        public override int SerializedByteSize
        {
            get { return Source.SerializedByteSize + Script.SerializedByteSize + BufferOperations.UINT32_SIZE; }
        }

        public override string ToString()
        {
            if (IsCoinbase)
            {
                return string.Format("CTxIn({0}, coinbase {1:x})", Source, Script);
            }
            if (SequenceNumber == DefaultSequenceNumber)
            {
                return string.Format("CTxIn({0}, scriptSig={1:S})", Source, Script);
            }
            return string.Format("CTxIn({0}, scriptSig={1:S}, nSequence={2})", Source, Script, SequenceNumber);
        }

        public bool IsFrozen { get; private set; }

        public void Freeze()
        {
            Source.Freeze();
            Script.Freeze();
            IsFrozen = true;
        }

        public TransactionInput Thaw()
        {
            return new TransactionInput(this, false);
        }

        public TransactionInput ThawTree()
        {
            ContractsCommon.IsThawed(Contract.Result<TransactionInput>().Source);
            ContractsCommon.IsThawed(Contract.Result<TransactionInput>().Script);

            return new TransactionInput(this, true);
        }

        static TransactionInput()
        {
            var empty = new TransactionInput();
            empty.Freeze();
            _empty = empty;
        }

        private static readonly TransactionInput _empty;
        public static TransactionInput Empty { get { return _empty; } }

        [ContractInvariantMethod]
        private void __Invariant()
        {
            Contract.Invariant(_source != null);
            Contract.Invariant(_script != null);
        }
    }
}
