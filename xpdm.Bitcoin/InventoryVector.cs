using System;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin
{
    public class InventoryVector : BitcoinSerializableBase
    {
        public InventoryObjectType Type { get; private set; }
        public Hash ObjectHash { get; private set; }

        private const int BYTESIZE = 36;
        public override uint ByteSize
        {
            get { return BYTESIZE; }
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
            Contract.Requires<ArgumentNullException>(buffer.Length >= BYTESIZE, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - BYTESIZE, "offset");

            Type = (InventoryObjectType)buffer.ReadUInt32(offset);
            ObjectHash = new Hash(buffer, offset + OBJECTHASH_OFFSET);
        }

        private const int OBJECTHASH_OFFSET = 4;

        [Pure]
        public override void WriteToBitcoinBuffer(byte[] buffer, int offset)
        {
            ((uint)Type).WriteBytes(buffer, offset);
            ObjectHash.WriteToBitcoinBuffer(buffer, offset + OBJECTHASH_OFFSET);
        }

        [ContractInvariantMethod]
        private void __Invariant()
        {
            Contract.Invariant(ObjectHash != null);
        }
    }
}
