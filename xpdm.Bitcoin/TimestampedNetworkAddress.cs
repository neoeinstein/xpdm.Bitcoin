using System;
using System.Diagnostics.Contracts;
using System.Net;
using System.Net.Sockets;

namespace xpdm.Bitcoin
{
    public class TimestampedNetworkAddress : BitcoinSerializableBase
    {
        public uint Timestamp { get; private set; }
        public NetworkAddress Address { get; private set; }

        public override uint ByteSize
        {
            get { return (uint)TimestampedNetworkAddress.MinimumByteSize; }
        }

        public TimestampedNetworkAddress(NetworkAddress address, uint timestamp)
        {
            Contract.Requires<ArgumentNullException>(address != null, "address");

            Timestamp = timestamp;
            Address = address;
        }

        public TimestampedNetworkAddress(byte[] buffer, int offset)
            : base(buffer, offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Length >= TimestampedNetworkAddress.MinimumByteSize, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - TimestampedNetworkAddress.MinimumByteSize, "offset");

            Timestamp = buffer.ReadUInt32(offset);
            Address = new NetworkAddress(buffer, offset + NETADDR_OFFSET);
        }

        private const int NETADDR_OFFSET = BitcoinBufferOperations.UINT32_SIZE;

        [Pure]
        public override void WriteToBitcoinBuffer(byte[] buffer, int offset)
        {
            Timestamp.WriteBytes(buffer, offset);
            Address.WriteToBitcoinBuffer(buffer, offset + NETADDR_OFFSET);
        }

        public static int MinimumByteSize
        {
            get
            {
                return BitcoinBufferOperations.UINT32_SIZE + NetworkAddress.MinimumByteSize;
            }
        }

        [ContractInvariantMethod]
        private void __Invariant()
        {
            Contract.Invariant(Address != null);
        }
    }
}
