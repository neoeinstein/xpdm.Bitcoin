using System;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin
{
    public class InvPayload : MessagePayloadBase
    {
        public override string Command
        {
            get { return InvPayload.CommandText; }
        }

        public override uint ByteSize
        {
            get
            {
                return Count.ByteSize + (uint)Count * (uint)InventoryVector.MinimumByteSize;
            }
        }

        public VarInt Count { get; private set; }

        private readonly InventoryVector[] _inventory;
        public InventoryVector[] Inventory
        {
            get
            {
                Contract.Ensures(Contract.Result<InventoryVector[]>() != null);
                Contract.Ensures((uint)Contract.Result<InventoryVector[]>().Length == Count);

                var retVal = new InventoryVector[_inventory.Length];
                Array.Copy(_inventory, retVal, _inventory.Length);

                return retVal;
            }
        }

        public InvPayload(InventoryVector[] inventory)
        {
            Contract.Requires<ArgumentNullException>(inventory != null, "inventory");

            _inventory = new InventoryVector[inventory.Length];
            Array.Copy(inventory, _inventory, inventory.Length);
            Count = new VarInt((uint)inventory.Length);
        }

        public InvPayload(byte[] buffer, int offset)
            : base(buffer, offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Length >= InvPayload.MinimumByteSize, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - InvPayload.MinimumByteSize, "offset");

            Count = new VarInt(buffer, offset);
            _inventory = buffer.ReadArray<InventoryVector>(offset + (int)Count.ByteSize, Count);
        }

        [Pure]
        public override void WriteToBitcoinBuffer(byte[] buffer, int offset)
        {
            Count.WriteToBitcoinBuffer(buffer, offset);
            BitcoinBufferOperations.WriteBytes(_inventory, buffer, offset + (int)Count.ByteSize, Count);
        }

        public static string CommandText
        {
            get { return "inv"; }
        }

        public static int MinimumByteSize
        {
            get { return VarInt.MinimumByteSize; }
        }
    }
}
