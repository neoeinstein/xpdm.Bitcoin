using System;
using System.Diagnostics.Contracts;
using xpdm.Bitcoin;

namespace xpdm.Bitcoin.Protocol
{
    public class InventoryVector : BitcoinSerializableBase
    {
        public InventoryObjectType Type { get; private set; }
        public Hash ObjectHash { get; private set; }

        public InventoryVector(InventoryObjectType type, Hash objectHash)
        {
            Contract.Requires<ArgumentNullException>(objectHash != null);

            Type = type;
            ObjectHash = objectHash;

            ByteSize = (uint)InventoryVector.ConstantByteSize;
        }

        public InventoryVector(byte[] buffer, int offset)
            : base(buffer, offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Length >= InventoryVector.ConstantByteSize, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - InventoryVector.ConstantByteSize, "offset");

            Type = (InventoryObjectType)buffer.ReadUInt32(offset);
            ObjectHash = new Hash(buffer, offset + OBJECTHASH_OFFSET);

            ByteSize = (uint)InventoryVector.ConstantByteSize;
        }

        private const int OBJECTHASH_OFFSET = BufferOperations.UINT32_SIZE;

        [Pure]
        public override void WriteToBitcoinBuffer(byte[] buffer, int offset)
        {
            ((uint)Type).WriteBytes(buffer, offset);
            ObjectHash.WriteToBitcoinBuffer(buffer, offset + OBJECTHASH_OFFSET);
        }

        public static int ConstantByteSize
        {
            get { return BufferOperations.UINT32_SIZE + Hash.ConstantByteSize; }
        }

        [ContractInvariantMethod]
        private void __Invariant()
        {
            Contract.Invariant(ObjectHash != null);
        }
    }
}
