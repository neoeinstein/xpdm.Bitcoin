using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;

namespace xpdm.Bitcoin.Messaging.Payloads
{
    public class GetDataPayload : PayloadBase
    {
        public override string Command
        {
            get { return GetDataPayload.CommandText; }
        }

        public InventoryVector[] Inventory { get; private set; }

        public GetDataPayload(InventoryVector[] inventory)
        {
            Contract.Requires<ArgumentNullException>(inventory != null, "inventory");

            Inventory = inventory;
        }

        public GetDataPayload(Stream stream) : base(stream) { }
        public GetDataPayload(byte[] buffer, int offset) : base(buffer, offset) { }

        protected override void Deserialize(Stream stream)
        {
            Inventory = ReadVarArray<InventoryVector>(stream);
        }

        public override void Serialize(Stream stream)
        {
            WriteVarArray(stream, Inventory);
        }

        public override int SerializedByteSize
        {
            get { return VarIntByteSize(Inventory.Length) + Inventory.Sum(iv => iv.SerializedByteSize); }
        }

        public static string CommandText
        {
            get { return "getdata"; }
        }
    }
}
