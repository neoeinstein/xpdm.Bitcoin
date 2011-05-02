using System;
using System.Diagnostics.Contracts;
using System.Net;
using System.Net.Sockets;

namespace xpdm.Bitcoin
{
    public class NetworkAddress : BitcoinSerializableBase
    {
        public Services Services { get; private set; }
        public IPEndPoint Endpoint { get; private set; }

        public override uint ByteSize
        {
            get { return (uint)NetworkAddress.MinimumByteSize; }
        }

        public NetworkAddress(Services services, IPEndPoint endpoint)
        {
            Contract.Requires<ArgumentNullException>(endpoint != null, "endpoint");
            Contract.Requires<ArgumentException>(endpoint.AddressFamily == AddressFamily.InterNetwork || endpoint.AddressFamily == AddressFamily.InterNetworkV6, "Non-IP Addresses are not supported");

            Services = services;
            Endpoint = endpoint;
        }

        public NetworkAddress(byte[] buffer, int offset)
            : base(buffer, offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Length >= NetworkAddress.MinimumByteSize, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - NetworkAddress.MinimumByteSize, "offset");

            Services = (Services)buffer.ReadUInt64(offset);
            var address = new byte[16];
            Array.Copy(buffer, offset + ADDRESS_IPV6_OFFSET, address, 0, 16);
            Endpoint = new IPEndPoint(
                          new IPAddress(address),
                          buffer.ReadUInt16BE(offset + PORT_OFFSET));
        }

        private const int ADDRESS_IPV6_OFFSET = BitcoinBufferOperations.UINT64_SIZE;
        private const int ADDRESS_IPV4TO6_OFFSET = ADDRESS_IPV6_OFFSET + BitcoinBufferOperations.UINT64_SIZE;
        private const int ADDRESS_IPV4_OFFSET = ADDRESS_IPV4TO6_OFFSET + BitcoinBufferOperations.UINT32_SIZE;
        private const int PORT_OFFSET = ADDRESS_IPV6_OFFSET + BitcoinBufferOperations.UINT64_SIZE * 2;

        [Pure]
        public override void WriteToBitcoinBuffer(byte[] buffer, int offset)
        {
            ((ulong)Services).WriteBytes(buffer, offset);
            switch(Endpoint.AddressFamily)
            {
                case AddressFamily.InterNetwork:
                    ((ushort)0xFFFF).WriteBytes(buffer, ADDRESS_IPV4TO6_OFFSET);
                    Endpoint.Address.GetAddressBytes().CopyTo(buffer, offset + ADDRESS_IPV4_OFFSET);
                    break;
                case AddressFamily.InterNetworkV6:
                    Endpoint.Address.GetAddressBytes().CopyTo(buffer, offset + ADDRESS_IPV6_OFFSET);
                    break;
            }
            ((ushort)Endpoint.Port).WriteBytesBE(buffer, offset + PORT_OFFSET);
        }

        public static int MinimumByteSize
        {
            get { return BitcoinBufferOperations.UINT64_SIZE * 3 + BitcoinBufferOperations.UINT16_SIZE; }
        }

        private readonly static NetworkAddress s_IPv6Any = new NetworkAddress(0, new IPEndPoint(IPAddress.IPv6Any, 0));
        public static NetworkAddress IPv6Any
        {
            get
            {
                return s_IPv6Any;
            }
        }

        [ContractInvariantMethod]
        private void __Invariant()
        {
            Contract.Invariant(Endpoint != null);
            Contract.Invariant(Endpoint.AddressFamily == AddressFamily.InterNetwork || Endpoint.AddressFamily == AddressFamily.InterNetworkV6, "Non-IP Addresses are not supported");
        }
    }
}
