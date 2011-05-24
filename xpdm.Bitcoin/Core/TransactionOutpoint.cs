using System.Diagnostics.Contracts;
using System.IO;

namespace xpdm.Bitcoin.Core
{
    public sealed class TransactionOutpoint : BitcoinObject, IFreezable, IThawable<TransactionOutpoint>
    {
        private Hash256 _sourceTransactionHash = Hash256.Empty;
        public Hash256 SourceTransactionHash
        {
            get
            {
                ContractsCommon.ResultIsNonNull<Hash256>();

                return _sourceTransactionHash;
            }
            set
            {
                ContractsCommon.NotFrozen(this);
                ContractsCommon.NotNull(value, "value");

                _sourceTransactionHash = value;
                InvalidateBitcoinHashes();
            }
        }
        private int _outputSequenceNumber;
        public int OutputSequenceNumber
        {
            get
            {
                return _outputSequenceNumber;
            }
            set
            {
                ContractsCommon.NotFrozen(this);

                _outputSequenceNumber = value;
                InvalidateBitcoinHashes();
            }
        }

        public TransactionOutpoint()
        {
        }
        public TransactionOutpoint(TransactionOutpoint outpoint)
        {
            ContractsCommon.NotNull(outpoint, "outpoint");

            _sourceTransactionHash = outpoint._sourceTransactionHash;
            _outputSequenceNumber = outpoint._outputSequenceNumber;
        }

        public TransactionOutpoint(Stream stream) : base(stream) { }
        public TransactionOutpoint(byte[] buffer, int offset) : base(buffer, offset) { }

        public bool IsCoinbase
        {
            get
            {
                return Coinbase.SourceTransactionHash.Equals(this.SourceTransactionHash) &&
                    Coinbase.OutputSequenceNumber == this.OutputSequenceNumber;
            }
        }

        protected override void Deserialize(System.IO.Stream stream)
        {
            SourceTransactionHash = new Hash256(stream);
            OutputSequenceNumber = ReadInt32(stream);

            Freeze();
        }

        public override void Serialize(System.IO.Stream stream)
        {
            SourceTransactionHash.Serialize(stream);
            Write(stream, OutputSequenceNumber);
        }

        public override int SerializedByteSize
        {
            get { return SourceTransactionHash.HashByteSize + BufferOperations.UINT32_SIZE; }
        }

        public override string ToString()
        {
            return string.Format("COutPoint({0:S}, {1})", SourceTransactionHash, OutputSequenceNumber);
        }

        public bool IsFrozen { get; private set; }

        public void Freeze()
        {
            IsFrozen = true;
        }

        public TransactionOutpoint Thaw()
        {
            return new TransactionOutpoint(this);
        }

        public TransactionOutpoint ThawTree()
        {
            return Thaw();
        }

        static TransactionOutpoint()
        {
            var empty = new TransactionOutpoint();
            empty.Freeze();
            _empty = empty;
            var coinbase = new TransactionOutpoint
                               {
                                   SourceTransactionHash = Hash256.Empty,
                                   OutputSequenceNumber = -1,
                               };
            coinbase.Freeze();
            _coinbase = coinbase;
        }

        private static readonly TransactionOutpoint _empty;
        public static TransactionOutpoint Empty { get { return _empty; } }

        private static readonly TransactionOutpoint _coinbase;
        public static TransactionOutpoint Coinbase { get { return _coinbase; } }

        [ContractInvariantMethod]
        private void __Invariant()
        {
            Contract.Invariant(_sourceTransactionHash != null);
        }
    }
}
