﻿using C5;
using SCG = System.Collections.Generic;
using System.IO;
using System.Linq;

namespace xpdm.Bitcoin.Core
{
    public sealed class Block : BitcoinObject
    {
        public uint Version { get; private set; }
        public Hash256 PreviousBlockHash { get; private set; }
        public Hash256 MerkleRoot { get; private set; }
        public uint Timestamp { get; private set; }
        public uint DifficultyBits { get; private set; }
        public uint Nonce { get; private set; }
        public ICollection<Transaction> Transactions { get; private set; }

        public Block(uint version, Hash256 previousBlockHash, Hash256 merkleRoot, uint timestamp, uint difficultyBits, uint nonce)
        {
            Version = version;
            PreviousBlockHash = previousBlockHash;
            MerkleRoot = merkleRoot;
            Timestamp = timestamp;
            DifficultyBits = difficultyBits;
            Nonce = nonce;
            Transactions = new WrappedArray<Transaction>(new Transaction[0]);
        }

        public Block(uint version, Hash256 previousBlockHash, uint timestamp, uint difficultyBits, uint nonce, SCG.IEnumerable<Transaction> transactions)
        {
            Version = version;
            PreviousBlockHash = previousBlockHash;
            Timestamp = timestamp;
            DifficultyBits = difficultyBits;
            Nonce = nonce;

            var trans = new ArrayList<Transaction>();
            trans.AddAll(transactions);
            Transactions = new GuardedCollection<Transaction>(trans);
            MerkleRoot = GetMerkleRoot();
        }

        private IIndexed<Hash256> _merkleTree;
        
        public Hash256 GetMerkleRoot()
        {
            if (_merkleTree == null)
            {
                _merkleTree = CalculateMerkleTree(Transactions);
            }
            return _merkleTree[_merkleTree.Count - 1];
        }

        public static IIndexed<Hash256> CalculateMerkleTree(SCG.IEnumerable<Transaction> transactions)
        {
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
            Transactions = new GuardedCollection<Transaction>(transactions);
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
    }
}
