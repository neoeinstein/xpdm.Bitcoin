using System;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin
{
    public class BlockHeader : BitcoinSerializableBase
    {
        public uint Version { get; private set; }
        public Hash PreviousBlock { get; private set; }
        public Hash MerkleRoot { get; private set; }
        public uint Timestamp { get; private set; }
        public uint Bits { get; private set; }
        public uint Nonce { get; private set; }
        public VarInt TransactionCount { get; private set; }

        private const int BYTESIZE = 80;
        public override uint ByteSize
        {
            get { return BYTESIZE + TransactionCount.ByteSize; }
        }

        public BlockHeader(uint version, Hash previousBlock, Hash merkleRoot, uint timestamp, uint bits, uint nonce, VarInt transactionCount)
        {
            Contract.Requires<ArgumentNullException>(previousBlock != null);
            Contract.Requires<ArgumentNullException>(merkleRoot != null);

            Version = version;
            PreviousBlock = previousBlock;
            MerkleRoot = merkleRoot;
            Timestamp = Timestamp;
            Bits = bits;
            Nonce = nonce;
            TransactionCount = transactionCount;
        }

        public BlockHeader(byte[] buffer, int offset)
            : base(buffer, offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentNullException>(buffer.Length > BYTESIZE, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset < buffer.Length - BYTESIZE, "offset");

            Version = buffer.ReadUInt32(offset);
            PreviousBlock = new Hash(buffer, offset + PREVBLOCK_OFFSET);
            MerkleRoot = new Hash(buffer, offset + MERKLEROOT_OFFSET);
            Timestamp = buffer.ReadUInt32(offset + TIMESTAMP_OFFSET);
            Bits = buffer.ReadUInt32(offset + BITS_OFFSET);
            Nonce = buffer.ReadUInt32(offset + NONCE_OFFSET);
            TransactionCount = new VarInt(buffer, offset + TXCOUNT_OFFSET);
        }

        private const int PREVBLOCK_OFFSET = 4;
        private const int MERKLEROOT_OFFSET = PREVBLOCK_OFFSET + 32;
        private const int TIMESTAMP_OFFSET = MERKLEROOT_OFFSET + 32;
        private const int BITS_OFFSET = TIMESTAMP_OFFSET + 4;
        private const int NONCE_OFFSET = BITS_OFFSET + 4;
        private const int TXCOUNT_OFFSET = NONCE_OFFSET + 4;

        [Pure]
        public override void WriteToBitcoinBuffer(byte[] bytes, int offset)
        {
            Version.WriteBytes(bytes, offset);
            PreviousBlock.WriteToBitcoinBuffer(bytes, offset + PREVBLOCK_OFFSET);
            MerkleRoot.WriteToBitcoinBuffer(bytes, offset + MERKLEROOT_OFFSET);
            Timestamp.WriteBytes(bytes, offset + TIMESTAMP_OFFSET);
            Bits.WriteBytes(bytes, offset + BITS_OFFSET);
            Nonce.WriteBytes(bytes, offset + NONCE_OFFSET);
            TransactionCount.WriteToBitcoinBuffer(bytes, offset + TXCOUNT_OFFSET);
        }

        [ContractInvariantMethod]
        private void __Invariant()
        {
            Contract.Invariant(PreviousBlock != null);
            Contract.Invariant(MerkleRoot != null);
        }
    }
}
