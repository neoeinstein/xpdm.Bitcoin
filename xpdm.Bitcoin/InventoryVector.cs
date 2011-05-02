using System;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin
{
    public class InventoryVector : BitcoinSerializableBase
    {
        public InventoryObjectType Type { get; private set; }
        public Hash ObjectHash { get; private set; }

        public override uint ByteSize
        {
            get { return (uint)InventoryVector.MinimumByteSize; }
        }

        public InventoryVector(InventoryObjectType type, Hash objectHash)
        {
            Contract.Requires<ArgumentNullException>(objectHash != null);

            Type = type;
            ObjectHash = objectHash;
        }

        public InventoryVector(byte[] buffer, int offset)
            : base(buffer, offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Length >= InventoryVector.MinimumByteSize, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - InventoryVector.MinimumByteSize, "offset");

            Type = (InventoryObjectType)buffer.ReadUInt32(offset);
            ObjectHash = new Hash(buffer, offset + OBJECTHASH_OFFSET);
        }

        private const int OBJECTHASH_OFFSET = BitcoinBufferOperations.UINT32_SIZE;

        [Pure]
        public override void WriteToBitcoinBuffer(byte[] buffer, int offset)
        {
            ((uint)Type).WriteBytes(buffer, offset);
            ObjectHash.WriteToBitcoinBuffer(buffer, offset + OBJECTHASH_OFFSET);
        }

        public static int MinimumByteSize
        {
            get { return BitcoinBufferOperations.UINT32_SIZE + Hash.MinimumByteSize; }
        }

        [ContractInvariantMethod]
        private void __Invariant()
        {
            Contract.Invariant(ObjectHash != null);
        }
    }
}
