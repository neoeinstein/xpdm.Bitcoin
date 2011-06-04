using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using C5;
using xpdm.Bitcoin.Cryptography;
using SCG = System.Collections.Generic;

namespace xpdm.Bitcoin.Core
{
    public sealed class Block : BitcoinObject, IFreezable, IThawable<Block>
    {
        private BlockHeader _header = BlockHeader.Empty;
        public BlockHeader Header
        {
            get
            {
                ContractsCommon.ResultIsNonNull<BlockHeader>();

                return _header;
            }
            set
            {
                ContractsCommon.NotFrozen(this);
                ContractsCommon.NotNull(value, "value");

                _header.HashesInvalidated -= InvalidateBitcoinHashes;
                _header = value;
                _header.HashesInvalidated += InvalidateBitcoinHashes;
                InvalidateBitcoinHashes();
            }
        }

        public IList<Transaction> Transactions { get; private set; }

        public Block()
        {
            Transactions = new HashedArrayList<Transaction>();
            Transactions.CollectionChanged += InvalidateMerkleTree;
        }

        private IList<Hash256> _merkleTree;
        public IList<Hash256> MerkleTree
        {
            get
            {
                ContractsCommon.ResultIsNonNull<IList<Hash256>>();

                if (_merkleTree == null)
                {
                    _merkleTree = CalculateMerkleTree(Transactions);
                    if (!IsFrozen)
                    {
                        InvalidateBitcoinHashes();
                    }
                }
                return _merkleTree;
            }
        }

        public static IList<Hash256> CalculateMerkleTree(SCG.IEnumerable<Transaction> transactions)
        {
            ContractsCommon.NotNull(transactions, "transactions");
            Contract.Requires(Contract.ForAll(transactions, tx => tx != null));
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
                    tree.Add(CryptoFunctionProviderFactory.Default.Hash256(tree[j + i].Bytes, tree[j + i2].Bytes));
                }
                j += size;
            }
            return new GuardedList<Hash256>(tree);
        }

        private void InvalidateMerkleTree(object sender)
        {
            ContractsCommon.NotFrozen(this);

            _merkleTree = null;
            Header.MerkleRoot = MerkleTree.Last;
            InvalidateBitcoinHashes();
        }

        public Block(Block block) : this(block, false) { }

        public Block(Block block, bool thawChildren)
        {
            ContractsCommon.NotNull(block, "block");
            Contract.Ensures(this.Header == block.Header);
            Contract.Ensures(this.Transactions.SequencedEquals(block.Transactions));
            ContractsCommon.ChildThawed(Header, thawChildren);
            ContractsCommon.ChildrenThawed(Transactions, thawChildren);

            this._header = FreezableExtensions.ThawChild(block.Header, thawChildren);
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
            Header = Read<BlockHeader>(stream);

            var transactionsArr = ReadVarArray<Transaction>(stream);
            var transactions = new HashedArrayList<Transaction>(transactionsArr.Length);
            transactions.AddAll(transactionsArr);
            transactions.CollectionChanged += InvalidateMerkleTree;
            Transactions = transactions;

            Freeze();
        }

        public override void Serialize(Stream stream)
        {
            Write(stream, Header);
            WriteCollection(stream, Transactions);
        }

        protected override byte[] BuildBitcoinHashByteArray()
        {
            return Header.SerializeToByteArray();
        }

        public override int SerializedByteSize
        {
            get
            {
                return Header.SerializedByteSize + VarIntByteSize(Transactions.Count) +
                       Transactions.Sum(t => t.SerializedByteSize);
            }
        }

        public override string ToString()
        {
            var blockStr = new StringBuilder();
            blockStr.AppendFormat("CBlock(hash={0:S20}, ver={1}, hashPrevBlock={2:S20}, hashMerkleRoot={3:S}, nTime={4}, nBits={5:x8}, nNonce={6}, vtx={7})",
                Hash256, Header.Version, Header.PreviousBlockHash, Header.MerkleRoot, Header.Timestamp.SecondsSinceEpoch, Header.DifficultyBits, Header.Nonce, Transactions.Count);
            foreach (var tx in Transactions)
            {
                foreach (var line in tx.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                {
                    blockStr.AppendLine();
                    blockStr.AppendFormat("\t{0}", line);
                }
            }
            blockStr.AppendLine();
            blockStr.AppendFormat("\tvMerkleTree: {0}", string.Join(" ", MerkleTree.Select(h => h.ToString("S6"))));
            return blockStr.ToString();
        }

        public bool IsFrozen { get; private set; }

        public void Freeze()
        {
            Header.Freeze();
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
            Contract.Invariant(Header != null);
            Contract.Invariant(_merkleTree == null || _merkleTree is GuardedList<Hash256>);
            Contract.Invariant(Transactions != null);
            Contract.Invariant(!IsFrozen || Transactions.IsReadOnly);
            Contract.Invariant(!IsFrozen || Header.IsFrozen);
        }
    }
}
