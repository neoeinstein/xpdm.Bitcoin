using System;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin.Messaging
{
    public class TimestampedNetworkAddress : SerializableMessageBase
    {
        public uint Timestamp { get; private set; }
        public NetworkAddress Address { get; private set; }

        public TimestampedNetworkAddress(NetworkAddress address, uint timestamp)
        {
            Contract.Requires<ArgumentNullException>(address != null, "address");

            Timestamp = timestamp;
            Address = address;

            ByteSize = (uint)TimestampedNetworkAddress.ConstantByteSize;
        }

        public TimestampedNetworkAddress(byte[] buffer, int offset)
            : base(buffer, offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Length >= TimestampedNetworkAddress.ConstantByteSize, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - TimestampedNetworkAddress.ConstantByteSize, "offset");

            Timestamp = buffer.ReadUInt32(offset);
            Address = new NetworkAddress(buffer, offset + NETADDR_OFFSET);

            ByteSize = (uint)TimestampedNetworkAddress.ConstantByteSize;
        }

        private const int NETADDR_OFFSET = BufferOperations.UINT32_SIZE;

        [Pure]
        public override void WriteToBitcoinBuffer(byte[] buffer, int offset)
        {
            Timestamp.WriteBytes(buffer, offset);
            Address.WriteToBitcoinBuffer(buffer, offset + NETADDR_OFFSET);
        }

        public static int ConstantByteSize
        {
            get
            {
                return BufferOperations.UINT32_SIZE + NetworkAddress.ConstantByteSize;
            }
        }

        [ContractInvariantMethod]
        private void __Invariant()
        {
            Contract.Invariant(Address != null);
        }
    }
}
