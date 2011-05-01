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

        private const uint BYTESIZE = 26;
        public override uint ByteSize
        {
            get { return BYTESIZE; }
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
            Contract.Requires<ArgumentNullException>(buffer.Length >= BYTESIZE, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - BYTESIZE, "offset");

            Services = (Services)buffer.ReadUInt64(offset);
            var address = new byte[16];
            Array.Copy(buffer, offset + ADDRESS_IPV6_OFFSET, address, 0, 16);
            Endpoint = new IPEndPoint(
                          new IPAddress(address),
                          buffer.ReadUInt16BE(offset + PORT_OFFSET));
        }

        private const int ADDRESS_IPV6_OFFSET = 8;
        private const int ADDRESS_IPV4_OFFSET = ADDRESS_IPV6_OFFSET + 12;
        private const int PORT_OFFSET = ADDRESS_IPV6_OFFSET + 16;

        [Pure]
        public override void WriteToBitcoinBuffer(byte[] buffer, int offset)
        {
            ((ulong)Services).WriteBytes(buffer, offset);
            switch(Endpoint.AddressFamily)
            {
                case AddressFamily.InterNetwork:
                    ((ushort)0xFFFF).WriteBytes(buffer, ADDRESS_IPV4_OFFSET - 2);
                    Endpoint.Address.GetAddressBytes().CopyTo(buffer, offset + ADDRESS_IPV4_OFFSET);
                    break;
                case AddressFamily.InterNetworkV6:
                    Endpoint.Address.GetAddressBytes().CopyTo(buffer, offset + ADDRESS_IPV6_OFFSET);
                    break;
            }
            ((ushort)Endpoint.Port).WriteBytesBE(buffer, offset + PORT_OFFSET);
        }

        [ContractInvariantMethod]
        private void __Invariant()
        {
            Contract.Invariant(Endpoint != null);
            Contract.Invariant(Endpoint.AddressFamily == AddressFamily.InterNetwork || Endpoint.AddressFamily == AddressFamily.InterNetworkV6, "Non-IP Addresses are not supported");
        }
    }
}
