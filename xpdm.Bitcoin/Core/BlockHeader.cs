using System.Diagnostics.Contracts;
using System.IO;

namespace xpdm.Bitcoin.Core
{
    public sealed class BlockHeader : BitcoinObject, IFreezable, IThawable<BlockHeader>
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

        private Hash256 _previousBlockHash = Hash256.Empty;
        public Hash256 PreviousBlockHash
        {
            get
            {
                ContractsCommon.ResultIsNonNull<Hash256>();

                return _previousBlockHash;
            }
            set
            {
                ContractsCommon.NotFrozen(this);
                ContractsCommon.NotNull(value, "value");

                _previousBlockHash = value;
                InvalidateBitcoinHashes();
            }
        }

        private Timestamp _timestamp;
        public Timestamp Timestamp
        {
            get { return _timestamp; }
            set
            {
                ContractsCommon.NotFrozen(this);

                _timestamp = value;
                InvalidateBitcoinHashes();
            }
        }

        private uint _difficultyBits;
        public uint DifficultyBits
        {
            get { return _difficultyBits; }
            set
            {
                ContractsCommon.NotFrozen(this);

                _difficultyBits = value;
                InvalidateBitcoinHashes();
            }
        }

        private uint _nonce;
        public uint Nonce
        {
            get { return _nonce; }
            set
            {
                ContractsCommon.NotFrozen(this);

                _nonce = value;
                InvalidateBitcoinHashes();
            }
        }

        private Hash256 _merkleRoot;
        public Hash256 MerkleRoot
        {
            get
            {
                ContractsCommon.ResultIsNonNull<Hash256>();

                return _merkleRoot;
            }
            set
            {
                ContractsCommon.NotFrozen(this);
                ContractsCommon.NotNull(value, "value");

                _merkleRoot = value;
                InvalidateBitcoinHashes();
            }
        }

        public BlockHeader()
        {
        }

        private void UpdateMerkleRoot(Block block)
        {
            ContractsCommon.NotFrozen(this);

            MerkleRoot = block.MerkleTree.Last;
            InvalidateBitcoinHashes();
        }


        public BlockHeader(BlockHeader header) : this(header, false) { }

        public BlockHeader(BlockHeader header, bool thawChildren)
        {
            ContractsCommon.NotNull(header, "header");
            Contract.Ensures(this.Version == header.Version);
            Contract.Ensures(this.PreviousBlockHash == header.PreviousBlockHash);
            Contract.Ensures(this.Timestamp == header.Timestamp);
            Contract.Ensures(this.DifficultyBits == header.DifficultyBits);
            Contract.Ensures(this.Nonce == header.Nonce);
            Contract.Ensures(this.MerkleRoot == header.MerkleRoot);

            this._version = header._version;
            this._previousBlockHash = header._previousBlockHash;
            this._timestamp = header._timestamp;
            this._difficultyBits = header._difficultyBits;
            this._nonce = header._nonce;
            this._merkleRoot = header._merkleRoot;
        }

        public BlockHeader(Stream stream) : base(stream) { }
        public BlockHeader(byte[] buffer, int offset) : base(buffer, offset) { }

        protected override void Deserialize(Stream stream)
        {
            Version = ReadUInt32(stream);
            PreviousBlockHash = Read<Hash256>(stream);
            MerkleRoot = Read<Hash256>(stream);
            Timestamp = ReadUInt32(stream);
            DifficultyBits = ReadUInt32(stream);
            Nonce = ReadUInt32(stream);

            Freeze();
        }

        public override void Serialize(Stream stream)
        {
            Write(stream, Version);
            Write(stream, PreviousBlockHash);
            Write(stream, MerkleRoot);
            Write(stream, Timestamp);
            Write(stream, DifficultyBits);
            Write(stream, Nonce);
        }

        public override int SerializedByteSize
        {
            get
            {
                return 4 * BufferOperations.UINT32_SIZE + PreviousBlockHash.SerializedByteSize +
                       MerkleRoot.SerializedByteSize;
            }
        }

        public override string ToString()
        {
            return string.Format("CBlockHeader(hash={0:S20}, ver={1}, hashPrevBlock={2:S20}, hashMerkleRoot={3:S}, nTime={4}, nBits={5:x8}, nNonce={6})",
                Hash256, Version, PreviousBlockHash, MerkleRoot, Timestamp.SecondsSinceEpoch, DifficultyBits, Nonce);
        }

        public bool IsFrozen { get; private set; }

        public void Freeze()
        {
            IsFrozen = true;
        }

        public BlockHeader Thaw()
        {
            return new BlockHeader(this, false);
        }

        public BlockHeader ThawTree()
        {
            return new BlockHeader(this, true);
        }

        static BlockHeader()
        {
            var empty = new BlockHeader();
            empty.Freeze();
            _empty = empty;
        }

        private static readonly BlockHeader _empty;
        public static BlockHeader Empty { get { return _empty; } }

        [ContractInvariantMethod]
        private void __Invariant()
        {
            Contract.Invariant(_previousBlockHash != null);
        }
    }
}
