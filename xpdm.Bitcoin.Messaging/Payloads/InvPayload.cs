using System.IO;
using System.Linq;

namespace xpdm.Bitcoin.Messaging.Payloads
{
    public class InvPayload : PayloadBase
    {
        public override string Command
        {
            get { return InvPayload.CommandText; }
        }

        public InventoryVector[] Inventory { get; private set; }

        public InvPayload(InventoryVector[] inventory)
        {
            ContractsCommon.NotNull(inventory, "inventory");

            Inventory = inventory;
        }

        public InvPayload(Stream stream) : base(stream) { }
        public InvPayload(byte[] buffer, int offset) : base(buffer, offset) { }

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

        public override string ToString()
        {
            return "{" + string.Join<InventoryVector>(",", Inventory) + "}";
        }

        public static string CommandText
        {
            get { return "inv"; }
        }
    }
}
