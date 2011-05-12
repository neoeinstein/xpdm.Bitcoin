using System;
using System.Diagnostics.Contracts;
using xpdm.Bitcoin;

namespace xpdm.Bitcoin.Protocol
{
    public class Block : BitcoinSerializableBase
    {
        public uint Version { get; private set; }
        public Hash PreviousBlock { get; private set; }
        public Hash MerkleRoot { get; private set; }
        public uint Timestamp { get; private set; }
        public uint Bits { get; private set; }
        public uint Nonce { get; private set; }
        public VarArray<Tx> Transactions { get; private set; }

        public Block(Block block, bool convertToHeader)
            : this(block.Version,
                   block.PreviousBlock,
                   block.MerkleRoot,
                   block.Timestamp,
                   block.Bits,
                   block.Nonce,
                   convertToHeader ? VarArray<Tx>.Empty : block.Transactions)
        {
        }
        
        public Block(uint version, Hash previousBlock, Hash merkleRoot, uint timestamp, uint bits, uint nonce, VarArray<Tx> transactions)
        {
            Contract.Requires<ArgumentNullException>(previousBlock != null);
            Contract.Requires<ArgumentNullException>(merkleRoot != null);

            Version = version;
            PreviousBlock = previousBlock;
            MerkleRoot = merkleRoot;
            Timestamp = Timestamp;
            Bits = bits;
            Nonce = nonce;
            Transactions = transactions;
        }

        public Block(byte[] buffer, int offset)
            : base(buffer, offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Length > Block.MinimumByteSize, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset < buffer.Length - Block.MinimumByteSize, "offset");

            Version = buffer.ReadUInt32(offset);
            PreviousBlock = new Hash(buffer, PreviousBlock_Offset(ref offset));
            MerkleRoot = new Hash(buffer, MerkleRoot_Offset(ref offset));
            Timestamp = buffer.ReadUInt32(Timestamp_Offset(ref offset));
            Bits = buffer.ReadUInt32(Bits_Offset(ref offset));
            Nonce = buffer.ReadUInt32(Nonce_Offset(ref offset));
            Transactions = new VarArray<Tx>(buffer, Transactions_Offset(ref offset));
        }

        private int PreviousBlock_Offset(ref int offset) { return offset += (int)Version.ByteSize(); }
        private int MerkleRoot_Offset(ref int offset) { return offset += (int)PreviousBlock.ByteSize; }
        private int Timestamp_Offset(ref int offset) { return offset += (int)MerkleRoot.ByteSize; }
        private int Bits_Offset(ref int offset) { return offset += (int)Timestamp.ByteSize(); }
        private int Nonce_Offset(ref int offset) { return offset += (int)Bits.ByteSize(); }
        private int Transactions_Offset(ref int offset) { return offset += (int)Nonce.ByteSize(); }

        [Pure]
        public override void WriteToBitcoinBuffer(byte[] buffer, int offset)
        {
            Version.WriteBytes(buffer, offset);
            PreviousBlock.WriteToBitcoinBuffer(buffer, PreviousBlock_Offset(ref offset));
            MerkleRoot.WriteToBitcoinBuffer(buffer, MerkleRoot_Offset(ref offset));
            Timestamp.WriteBytes(buffer, Timestamp_Offset(ref offset));
            Bits.WriteBytes(buffer, Bits_Offset(ref offset));
            Nonce.WriteBytes(buffer, Nonce_Offset(ref offset));
            Transactions.WriteToBitcoinBuffer(buffer, Transactions_Offset(ref offset));
        }

        public Block ConvertToBlockHeader()
        {
            Contract.Ensures(Contract.Result<Block>().Version == this.Version);
            Contract.Ensures(Contract.Result<Block>().PreviousBlock == this.PreviousBlock);
            Contract.Ensures(Contract.Result<Block>().MerkleRoot == this.MerkleRoot);
            Contract.Ensures(Contract.Result<Block>().Timestamp == this.Timestamp);
            Contract.Ensures(Contract.Result<Block>().Bits == this.Bits);
            Contract.Ensures(Contract.Result<Block>().Nonce == this.Nonce);
            Contract.Ensures(Contract.Result<Block>().Transactions.Count == 0);

            if (this.Transactions.Count == 0)
            {
                return this;
            }
            return new Block(
                   this.Version,
                   this.PreviousBlock,
                   this.MerkleRoot,
                   this.Timestamp,
                   this.Bits,
                   this.Nonce,
                   VarArray<Tx>.Empty);
        }

        public static int MinimumByteSize
        {
            get
            {
                return BufferOperations.UINT32_SIZE * 4 + Hash.ConstantByteSize * 2 + VarArray<Tx>.MinimumByteSize;
            }
        }

        [ContractInvariantMethod]
        private void __Invariant()
        {
            Contract.Invariant(PreviousBlock != null);
            Contract.Invariant(MerkleRoot != null);
        }
    }
}
