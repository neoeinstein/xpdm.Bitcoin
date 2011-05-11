using System;
using System.Diagnostics.Contracts;
using xpdm.Bitcoin;

namespace xpdm.Bitcoin.Protocol
{
    public class TxOutpoint : BitcoinSerializableBase
    {
        public Hash TransactionHash { get; private set; }
        public uint Index { get; private set; }

        public TxOutpoint(Hash transactionHash, uint index)
        {
            Contract.Requires<ArgumentNullException>(transactionHash != null);

            TransactionHash = transactionHash;
            Index = index;

            ByteSize = (uint)TxOutpoint.ConstantByteSize;
        }

        public TxOutpoint(byte[] buffer, int offset)
            : base(buffer, offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Length >= TxOutpoint.ConstantByteSize, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - TxOutpoint.ConstantByteSize, "offset");

            TransactionHash = new Hash(buffer, offset);
            Index = buffer.ReadUInt32(offset + INDEX_OFFSET);

            ByteSize = (uint)TxOutpoint.ConstantByteSize;
        }

        private static readonly int INDEX_OFFSET = Hash.ConstantByteSize;

        [Pure]
        public override void WriteToBitcoinBuffer(byte[] buffer, int offset)
        {
            TransactionHash.WriteToBitcoinBuffer(buffer, offset);
            Index.WriteBytes(buffer, offset + INDEX_OFFSET);
        }

        public static int ConstantByteSize
        {
            get { return BitcoinBufferOperations.UINT32_SIZE + Hash.ConstantByteSize; }
        }

        [ContractInvariantMethod]
        private void __Invariant()
        {
            Contract.Invariant(TransactionHash != null);
        }

    }
}
