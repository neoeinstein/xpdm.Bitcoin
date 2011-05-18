using System;
using System.Diagnostics.Contracts;
using C5;
using SCG = System.Collections.Generic;
using System.IO;
using System.Linq;

namespace xpdm.Bitcoin.Core
{
    public sealed class Block : BitcoinObject, IFreezable<Block>
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

        private IList<Hash256> _merkleTree;
        public Hash256 MerkleRoot
        {
            get
            {
                ContractsCommon.ResultIsNonNull<Hash256>();

                if (MerkleTree == null)
                {
                    return new Hash256();
                }
                return MerkleTree.Last;
            }
            set
            {
                ContractsCommon.NotFrozen(this);
                ContractsCommon.NotNull(value, "value");
                Contract.Requires<InvalidOperationException>(Transactions.Count == 0);

                _merkleTree = new GuardedList<Hash256>(new WrappedArray<Hash256>(new[] { value }));
            }
        }

        public IList<Transaction> Transactions { get; private set; }

        public Block()
        {
            Transactions = new HashedArrayList<Transaction>();
            Transactions.CollectionChanged += InvalidateMerkleTree;
        }

        public IList<Hash256> MerkleTree
        {
            get
            {
                ContractsCommon.ResultIsNonNull<IList<Hash256>>();

                if (_merkleTree == null)
                {
                    _merkleTree = CalculateMerkleTree(Transactions);
                    InvalidateBitcoinHashes();
                }
                return _merkleTree;
            }
        }

        public static IList<Hash256> CalculateMerkleTree(SCG.IEnumerable<Transaction> transactions)
        {
            ContractsCommon.NotNull(transactions, "transactions");
            ContractsCommon.ResultIsNonNull<IList<Hash256>>();

            var tree = new ArrayList<Hash256>();
            foreach (var trans in transactions)
            {
                tree.Add(trans.Hash256);
            }
            int j = 0;
            for (int size = tree.Count; size > 1; size = (size + 1) / 2)
            {
                for (int i = 0; i < size; i += 2)
                {
                    int i2 = Math.Min(i + 1, size - 1);
                    tree.Add(HashUtil.Hash256(tree[j + i].Bytes, tree[j + i2].Bytes));
                }
                j += size;
            }
            return new GuardedList<Hash256>(tree);
        }

        private void InvalidateMerkleTree(object sender)
        {
            ContractsCommon.NotFrozen(this);
            
            _merkleTree = null;
            InvalidateBitcoinHashes();
        }

        public Block ToBlockHeader()
        {
            ContractsCommon.ResultIsNonNull<Block>();

            if (IsBlockHeader)
            {
                return this;
            }
            return new Block
            {
                Version = this.Version,
                PreviousBlockHash = this.PreviousBlockHash,
                MerkleRoot = this.MerkleRoot,
                Timestamp = this.Timestamp,
                DifficultyBits = this.DifficultyBits,
                Nonce = this.Nonce,
            };
        }

        public bool IsBlockHeader
        {
            get
            {
                return Transactions.IsEmpty;
            }
        }

        public Block(Block block) : this(block, false) { }

        public Block(Block block, bool thawChildren)
        {
            ContractsCommon.NotNull(block, "block");
            Contract.Ensures(this.Version == block.Version);
            Contract.Ensures(this.PreviousBlockHash == block.PreviousBlockHash);
            Contract.Ensures(this.Timestamp == block.Timestamp);
            Contract.Ensures(this.DifficultyBits == block.DifficultyBits);
            Contract.Ensures(this.Nonce == block.Nonce);
            Contract.Ensures(this.Transactions.SequencedEquals(block.Transactions));
            Contract.Ensures(this.MerkleRoot == block.MerkleRoot);
            ContractsCommon.ChildrenThawed(Transactions, thawChildren);

            this._version = block._version;
            this._previousBlockHash = block._previousBlockHash;
            this._timestamp = block._timestamp;
            this._difficultyBits = block._difficultyBits;
            this._nonce = block._nonce;
            if (block._merkleTree != null)
            {
                var tree = new ArrayList<Hash256>(block._merkleTree.Count);
                tree.AddAll(block._merkleTree);
                this._merkleTree = new GuardedList<Hash256>(tree);
            }

            var transactions = new HashedArrayList<Transaction>(block.Transactions.Count);
            transactions.AddAll(FreezableExtensions.ThawChildren(block.Transactions, thawChildren));
            transactions.CollectionChanged += InvalidateMerkleTree;
            this.Transactions = transactions;
        }

        public Block(Stream stream) : base(stream) { }
        public Block(byte[] buffer, int offset) : base(buffer, offset) { }

        protected override void Deserialize(Stream stream)
        {
            Version = ReadUInt32(stream);
            PreviousBlockHash = new Hash256(stream);
            new Hash256(stream); // Throw away the Merkle Root
            Timestamp = ReadUInt32(stream);
            DifficultyBits = ReadUInt32(stream);
            Nonce = ReadUInt32(stream);

            var transactionsArr = ReadVarArray<Transaction>(stream);
            var transactions = new HashedArrayList<Transaction>(transactionsArr.Length);
            transactions.AddAll(transactionsArr);
            transactions.CollectionChanged += InvalidateMerkleTree;
            Transactions = transactions;

            Freeze();
        }

        public override void Serialize(Stream stream)
        {
            SerializeHeader(stream);
            WriteCollection(stream, Transactions);
        }

        [Pure]
        public void SerializeHeader(Stream stream)
        {
            ContractsCommon.NotNull(stream, "stream");

            Write(stream, Version);
            PreviousBlockHash.Serialize(stream);
            MerkleRoot.Serialize(stream);
            Write(stream, Timestamp);
            Write(stream, DifficultyBits);
            Write(stream, Nonce);
        }

        public override int SerializedByteSize
        {
            get
            {
                return 4 * BufferOperations.UINT32_SIZE + PreviousBlockHash.SerializedByteSize +
                       MerkleRoot.SerializedByteSize + VarIntByteSize(Transactions.Count) +
                       Transactions.Sum(t => t.SerializedByteSize);
            }
        }

        protected override byte[] BuildBitcoinHashByteArray()
        {
            ContractsCommon.ResultIsNonNull<byte[]>();

            var ms = new MemoryStream();
            this.SerializeHeader(ms);
            return ms.ToArray();
        }

        public override string ToString()
        {
            return string.Format("v{0} <{1} ^{2} {3:s} 0x{4:x8} {5}", Version, PreviousBlockHash, MerkleRoot, DateTimeExtensions.FromSecondsSinceEpoch(Timestamp), DifficultyBits, Nonce)
                + (IsBlockHeader ? string.Empty : string.Format("[ {{{0}}} ]", string.Join("}, {", Transactions)));
        }

        public bool IsFrozen { get; private set; }

        public void Freeze()
        {
            if (!Transactions.IsReadOnly)
            {
                Transactions = new GuardedList<Transaction>(Transactions);
            }
            foreach (var tx in Transactions)
            {
                tx.Freeze();
            }

            IsFrozen = true;
        }

        public Block Thaw()
        {
            return new Block(this, false);
        }

        public Block ThawTree()
        {
            ContractsCommon.IsThawed(Contract.Result<Block>().Transactions);

            return new Block(this, true);
        }

        static Block()
        {
            var empty = new Block();
            empty.Freeze();
            _empty = empty;
        }

        private static readonly Block _empty;
        public static Block Empty { get { return _empty; } }

        [ContractInvariantMethod]
        private void __Invariant()
        {
            Contract.Invariant(_previousBlockHash != null);
            Contract.Invariant(_merkleTree == null || _merkleTree is GuardedList<Hash256>);
            Contract.Invariant(Transactions != null);
            Contract.Invariant(!IsFrozen || Transactions.IsReadOnly);
        }
    }
}
