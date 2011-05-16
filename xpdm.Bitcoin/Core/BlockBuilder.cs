using System;
using System.Diagnostics.Contracts;
using C5;
using SCG = System.Collections.Generic;
using System.IO;
using System.Linq;

namespace xpdm.Bitcoin.Core
{
    public sealed class BlockBuilder : BitcoinObject
    {
        private uint _version;
        public uint Version
        {
            get { return _version; }
            set { _version = value; InvalidateBitcoinHashes(); }
        }

        private Hash256 _previousBlockHash;
        public Hash256 PreviousBlockHash
        {
            get { return _previousBlockHash; }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null, "value");

                _previousBlockHash = value;
                InvalidateBitcoinHashes();
            }
        }

        private uint _timestamp;
        public uint Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; InvalidateBitcoinHashes(); }
        }

        private uint _difficultyBits;
        public uint DifficultyBits
        {
            get { return _difficultyBits; }
            set { _difficultyBits = value; InvalidateBitcoinHashes(); }
        }

        private uint _nonce;
        public uint Nonce
        {
            get { return _nonce; }
            set { _nonce = value; InvalidateBitcoinHashes(); }
        }

        private IList<Hash256> _merkleTree;
        public Hash256 MerkleRoot
        {
            get
            {
                if (_merkleTree == null)
                {
                    _merkleTree = CalculateMerkleTree(Transactions);
                }
                return _merkleTree.Last;
            }
        }

        public IList<Transaction> Transactions { get; private set; }

        public Transaction this[int index]
        {
            get
            {
                Contract.Requires<IndexOutOfRangeException>(0 <= index && index < Transactions.Count);

                return Transactions[index];
            }
        }

        public BlockBuilder()
        {
            Transactions = new HashedLinkedList<Transaction>();
            Transactions.CollectionChanged += InvalidateMerkleTree;
        }

        public Block FreezeToBlock()
        {
            Contract.Ensures(Contract.Result<Block>().MerkleRoot == this.MerkleRoot);

            return new Block(Version, PreviousBlockHash, Timestamp, DifficultyBits, Nonce, Transactions);
        }

        public static IList<Hash256> CalculateMerkleTree(SCG.IEnumerable<Transaction> transactions)
        {
            Contract.Ensures(Contract.Result<IList<Hash256>>() != null);

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

        private void InvalidateMerkleTree(object sender)
        {
            _merkleTree = null;
            InvalidateBitcoinHashes();
        }

        protected override void Deserialize(Stream stream)
        {
            Version = ReadUInt32(stream);
            PreviousBlockHash = new Hash256(stream);
            new Hash256(stream); // Throw away the Merkle Root
            Timestamp = ReadUInt32(stream);
            DifficultyBits = ReadUInt32(stream);
            Nonce = ReadUInt32(stream);
            var transactionsArr = ReadVarArray<Transaction>(stream);
            var transactions = new HashedLinkedList<Transaction>();
            transactions.AddAll(transactionsArr);
            Transactions = transactions;
        }

        public override void Serialize(Stream stream)
        {
            SerializeHeader(stream);
            WriteCollection(stream, Transactions);
        }

        public void SerializeHeader(Stream stream)
        {
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
            Contract.Ensures(Contract.Result<byte[]>() != null);

            var ms = new MemoryStream();
            this.SerializeHeader(ms);
            return ms.ToArray();
        }
    }
}
