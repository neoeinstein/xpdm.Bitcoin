using System;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin
{
    public class InventoryVector : BitcoinSerializableBase
    {
        public InventoryObjectType Type { get; private set; }
        public byte[] ObjectHash { get; private set; }

        private const int BYTESIZE = 36;
        public override uint ByteSize
        {
            get { return BYTESIZE; }
        }

        public InventoryVector(InventoryObjectType type, byte[] objectHash)
        {
            Contract.Requires<ArgumentNullException>(objectHash != null);
            Contract.Requires<ArgumentException>(objectHash.Length >= OBJECTHASH_LENGTH);

            Type = type;
            ObjectHash = new byte[OBJECTHASH_LENGTH];
            Array.Copy(objectHash, ObjectHash, OBJECTHASH_LENGTH);
        }

        public InventoryVector(byte[] buffer, int offset)
            : base(buffer, offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentNullException>(buffer.Length >= BYTESIZE, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - BYTESIZE, "offset");

            Type = (InventoryObjectType)buffer.ReadUInt32(offset);
            ObjectHash = new byte[OBJECTHASH_LENGTH];
            Array.Copy(buffer, offset + OBJECTHASH_OFFSET, ObjectHash, 0, OBJECTHASH_LENGTH);
        }

        private const int OBJECTHASH_OFFSET = 4;
        private const int OBJECTHASH_LENGTH = 32;

        [Pure]
        public override void WriteToBitcoinBuffer(byte[] buffer, int offset)
        {
            ((uint)Type).WriteBytes(buffer, offset);
            Array.Copy(ObjectHash, 0, buffer, offset + OBJECTHASH_OFFSET, OBJECTHASH_LENGTH);
        }

        [ContractInvariantMethod]
        private void __Invariant()
        {
            Contract.Invariant(ObjectHash != null);
            Contract.Invariant(ObjectHash.Length == OBJECTHASH_LENGTH, "Expected hash length is 32.");
        }
    }
}
