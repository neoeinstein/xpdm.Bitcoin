using System.Diagnostics.Contracts;
using System.IO;
using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Messaging
{
    public class TimestampedNetworkAddress : BitcoinSerializable
    {
        public Timestamp Timestamp { get; private set; }
        public NetworkAddress Address { get; private set; }

        public TimestampedNetworkAddress(NetworkAddress address, Timestamp timestamp)
        {
            ContractsCommon.NotNull(address, "address");

            Timestamp = timestamp;
            Address = address;
        }

        public TimestampedNetworkAddress() { }
        public TimestampedNetworkAddress(Stream stream) : base(stream) { }
        public TimestampedNetworkAddress(byte[] buffer, int offset) : base(buffer, offset) { }

        protected override void Deserialize(Stream stream)
        {
            Timestamp = (Timestamp)ReadUInt32(stream);
            Address = new NetworkAddress(stream);
        }

        public override void Serialize(Stream stream)
        {
            Write(stream, Timestamp);
            Write(stream, Address);
        }

        public override int SerializedByteSize
        {
            get { return BufferOperations.UINT32_SIZE + Address.SerializedByteSize; }
        }

        public override string ToString()
        {
            return string.Format("{0}@{1:}", Address, Timestamp);
        }


        [ContractInvariantMethod]
        private void __Invariant()
        {
            Contract.Invariant(Address != null);
        }
    }
}
