using C5;
using SCG = System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin.Core
{
    public sealed class Block : BitcoinObject
    {
        public uint Version { get; private set; }
        public Hash256 PreviousBlockHash { get; private set; }
        public Hash256 MerkleRoot { get; private set; }
        public Timestamp Timestamp { get; private set; }
        public uint DifficultyBits { get; private set; }
        public uint Nonce { get; private set; }
        public IList<Transaction> Transactions { get; private set; }

        public Transaction this[int index]
        {
            get
            {
                ContractsCommon.ValidIndex(0, Transactions.Count, index);

                return Transactions[index];
            }
        }

        public Block(uint version, Hash256 previousBlockHash, Hash256 merkleRoot, Timestamp timestamp, uint difficultyBits, uint nonce)
        {
            Version = version;
            PreviousBlockHash = previousBlockHash;
            MerkleRoot = merkleRoot;
            Timestamp = timestamp;
            DifficultyBits = difficultyBits;
            Nonce = nonce;
            Transactions = new WrappedArray<Transaction>(new Transaction[0]);
        }

        public Block(uint version, Hash256 previousBlockHash, Timestamp timestamp, uint difficultyBits, uint nonce, SCG.IEnumerable<Transaction> transactions)
        {
            Version = version;
            PreviousBlockHash = previousBlockHash;
            Timestamp = timestamp;
            DifficultyBits = difficultyBits;
            Nonce = nonce;

            var trans = new ArrayList<Transaction>();
            trans.AddAll(transactions);
            Transactions = new GuardedList<Transaction>(trans);
            MerkleRoot = CalculateMerkleRoot();
        }

        private IList<Hash256> _merkleTree;
        
        public Hash256 CalculateMerkleRoot()
        {
            CalculateMerkleTree();
            return _merkleTree.IsEmpty ? _merkleTree.Last : null;
        }

        public IList<Hash256> CalculateMerkleTree()
        {
            ContractsCommon.ResultIsNonNull<IList<Hash256>>();

            if (_merkleTree == null)
            {
                _merkleTree = CalculateMerkleTree(Transactions);
            }
            return _merkleTree;
        }

        public static IList<Hash256> CalculateMerkleTree(SCG.IEnumerable<Transaction> transactions)
        {
            ContractsCommon.NotNull(transactions, "transactions");
            ContractsCommon.ResultIsNonNull<IList<Hash256>>();

            var tree = new ArrayList<Hash256>();
            var queue = new CircularQueue<Hash256>();
            foreach (var trans in transactions)
            {
                tree.Add(trans.Hash256);
                queue.Enqueue(trans.Hash256);
            }
            while (queue.Count > 1)
            {
                var newHash = HashUtil.Hash256(queue.Dequeue().Bytes, queue.Dequeue().Bytes);
                tree.Add(newHash);
                queue.Enqueue(newHash);
            }
            return tree;
        }

        public bool IsBlockHeader
        {
            get
            {
                return MerkleRoot != null && Transactions.IsEmpty;
            }
        }

        public Block() { }
        public Block(Stream stream) : base(stream) { }
        public Block(byte[] buffer, int offset) : base(buffer, offset) { }

        protected override void Deserialize(Stream stream)
        {
            Version = ReadUInt32(stream);
            PreviousBlockHash = new Hash256(stream);
            MerkleRoot = new Hash256(stream);
            Timestamp = ReadUInt32(stream);
            DifficultyBits = ReadUInt32(stream);
            Nonce = ReadUInt32(stream);
            var transactionsArr = ReadVarArray<Transaction>(stream);
            var transactions = new WrappedArray<Transaction>(transactionsArr);
            Transactions = new GuardedList<Transaction>(transactions);
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
                return 4*BufferOperations.UINT32_SIZE + PreviousBlockHash.SerializedByteSize +
                       MerkleRoot.SerializedByteSize + VarIntByteSize(Transactions.Count) +
                       Transactions.Sum(t => t.SerializedByteSize);
            }
        }

        protected override byte[] BuildBitcoinHashByteArray()
        {
            var ms = new MemoryStream();
            this.SerializeHeader(ms);
            return ms.ToArray();
        }

        public override string ToString()
        {
            return string.Format("v{0} <{1} ^{2} {3:s} 0x{4:x8} {5}", Version, PreviousBlockHash, MerkleRoot, DateTimeExtensions.FromSecondsSinceEpoch(Timestamp), DifficultyBits, Nonce)
                + (IsBlockHeader ? string.Empty : string.Format("[ {{{0}}} ]", string.Join("}, {", Transactions)));
        }
    }
}
