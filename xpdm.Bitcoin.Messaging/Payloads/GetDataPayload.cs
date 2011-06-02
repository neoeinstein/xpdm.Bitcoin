using System;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin.Messaging.Payloads
{
    public class GetDataPayload : PayloadBase
    {
        public override string Command
        {
            get { return GetDataPayload.CommandText; }
        }

        public VarArray<InventoryVector> Inventory { get; private set; }

        public GetDataPayload(InventoryVector[] inventory)
        {
            Contract.Requires<ArgumentNullException>(inventory != null, "inventory");

            Inventory = new VarArray<InventoryVector>(inventory);

            ByteSize = Inventory.ByteSize;
        }

        public GetDataPayload(byte[] buffer, int offset)
            : base(buffer, offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Length >= GetDataPayload.MinimumByteSize, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - GetDataPayload.MinimumByteSize, "offset");

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
            get { return "getdata"; }
        }

        public static int MinimumByteSize
        {
            get { return VarArray<InventoryVector>.MinimumByteSize; }
        }
    }
}
