using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;

namespace xpdm.Bitcoin.Messaging.Payloads
{
    public class AddrPayload : PayloadBase
    {
        public override string Command
        {
            get { return AddrPayload.CommandText; }
        }

        public TimestampedNetworkAddress[] AddressList { get; private set; }

        public AddrPayload(TimestampedNetworkAddress[] addressList)
        {
            ContractsCommon.NotNull(addressList, "addressList");

            AddressList = addressList;
        }

        public AddrPayload(Stream stream) : base(stream) { }
        public AddrPayload(byte[] buffer, int offset) : base(buffer, offset) { }

        protected override void Deserialize(Stream stream)
        {
            AddressList = ReadVarArray<TimestampedNetworkAddress>(stream);
        }

        public override void Serialize(Stream stream)
        {
            WriteVarArray(stream, AddressList);
        }

        public override int SerializedByteSize
        {
            get { return VarIntByteSize(AddressList.Length) + AddressList.Sum(a => a.SerializedByteSize); }
        }

        private const uint INCLUDE_TIMESTAMP_VERSION = 31402;

        public static string CommandText
        {
            get { return "addr"; }
        }

        [ContractInvariantMethod]
        private void __Invariant()
        {
            Contract.Invariant(AddressList != null);
        }
    }
}
