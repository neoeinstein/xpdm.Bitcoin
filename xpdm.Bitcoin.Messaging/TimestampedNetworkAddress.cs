using System;
using System.Diagnostics.Contracts;
using System.IO;
using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Messaging
{
    public class TimestampedNetworkAddress : BitcoinSerializable
    {
        public uint Timestamp { get; private set; }
        public NetworkAddress Address { get; private set; }

        public TimestampedNetworkAddress(NetworkAddress address, uint timestamp)
        {
            Contract.Requires<ArgumentNullException>(address != null, "address");

            Timestamp = timestamp;
            Address = address;
        }

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

        [ContractInvariantMethod]
        private void __Invariant()
        {
            Contract.Invariant(Address != null);
        }
    }
}
