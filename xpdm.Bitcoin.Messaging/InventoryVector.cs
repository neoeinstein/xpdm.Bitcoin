using System.Diagnostics.Contracts;
using System.IO;
using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Messaging
{
    public class InventoryVector : BitcoinSerializable
    {
        public InventoryObjectType Type { get; private set; }
        public Hash256 ObjectHash { get; private set; }

        public InventoryVector(InventoryObjectType type, Hash256 objectHash)
        {
            ContractsCommon.NotNull(objectHash, "objectHash");

            Type = type;
            ObjectHash = objectHash;
        }

        public InventoryVector() { ObjectHash = Hash256.Empty; }
        public InventoryVector(Stream stream) : base(stream) { }
        public InventoryVector(byte[] buffer, int offset) : base(buffer, offset) { }

        protected override void Deserialize(Stream stream)
        {
            Type = (InventoryObjectType)ReadUInt32(stream);
            ObjectHash = new Hash256(stream);
        }

        public override void Serialize(Stream stream)
        {
            Write(stream, (uint)Type);
            Write(stream, ObjectHash);
        }

        public override int SerializedByteSize
        {
            get { return BufferOperations.UINT32_SIZE + ObjectHash.HashByteSize; }
        }

        public override string ToString()
        {
            if (Type == InventoryObjectType.Msg_Block)
            {
                return string.Format("{0}:{1:S20}", Type, ObjectHash);
            }
            return string.Format("{0}:{1:S}", Type, ObjectHash);
        }

        [ContractInvariantMethod]
        private void __Invariant()
        {
            Contract.Invariant(ObjectHash != null);
        }
    }
}
