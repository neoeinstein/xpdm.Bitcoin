﻿using System;
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
            Contract.Requires<ArgumentNullException>(objectHash != null);

            Type = type;
            ObjectHash = objectHash;
        }

        public InventoryVector() { }
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

        [ContractInvariantMethod]
        private void __Invariant()
        {
            Contract.Invariant(ObjectHash != null);
        }
    }
}
