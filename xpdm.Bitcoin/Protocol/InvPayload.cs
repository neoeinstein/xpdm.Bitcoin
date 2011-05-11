using System;
using System.Diagnostics.Contracts;
using xpdm.Bitcoin;

namespace xpdm.Bitcoin.Protocol
{
    public class InvPayload : MessagePayloadBase
    {
        public override string Command
        {
            get { return InvPayload.CommandText; }
        }

        public VarArray<InventoryVector> Inventory { get; private set; }

        public InvPayload(InventoryVector[] inventory)
        {
            Contract.Requires<ArgumentNullException>(inventory != null, "inventory");

            Inventory = new VarArray<InventoryVector>(inventory);

            ByteSize = Inventory.ByteSize;
        }

        public InvPayload(byte[] buffer, int offset)
            : base(buffer, offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Length >= InvPayload.MinimumByteSize, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - InvPayload.MinimumByteSize, "offset");

            Inventory = new VarArray<InventoryVector>(buffer, offset);

            ByteSize = Inventory.ByteSize;
        }

        [Pure]
        public override void WriteToBitcoinBuffer(byte[] buffer, int offset)
        {
            Inventory.WriteToBitcoinBuffer(buffer, offset);
        }

        public static string CommandText
        {
            get { return "inv"; }
        }

        public static int MinimumByteSize
        {
            get { return VarArray<InventoryVector>.MinimumByteSize; }
        }
    }
}
