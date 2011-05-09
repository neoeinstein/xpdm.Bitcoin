using System;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin
{
    public class AddrPayload : MessagePayloadBase
    {
        public override string Command
        {
            get { return AddrPayload.CommandText; }
        }

        public VarArray<TimestampedNetworkAddress> AddressList { get; private set; }

        public AddrPayload(TimestampedNetworkAddress[] addressList)
        {
            Contract.Requires<ArgumentNullException>(addressList != null);

            AddressList = new VarArray<TimestampedNetworkAddress>(addressList);
            
            ByteSize = AddressList.ByteSize;
        }

        public AddrPayload(byte[] buffer, int offset)
        {
            AddressList = new VarArray<TimestampedNetworkAddress>(buffer, offset);

            ByteSize = AddressList.ByteSize;
        }

        private const uint INCLUDE_TIMESTAMP_VERSION = 31402;

        public override void WriteToBitcoinBuffer(byte[] buffer, int offset)
        {
            AddressList.WriteToBitcoinBuffer(buffer, offset);
        }

        public static string CommandText
        {
            get { return "addr"; }
        }

        public static int MinimumByteSize
        {
            get { return VarInt.MinimumByteSize; }
        }

        [ContractInvariantMethod]
        private void __Invariant()
        {
            Contract.Invariant(AddressList != null);
        }
    }
}
