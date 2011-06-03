using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net;
using System.Net.Sockets;
using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Messaging
{
    public class NetworkAddress : BitcoinSerializable
    {
        public Services Services { get; private set; }
        public IPEndPoint Endpoint { get; private set; }

        public NetworkAddress(Services services, IPEndPoint endpoint)
        {
            Contract.Requires<ArgumentNullException>(endpoint != null, "endpoint");
            Contract.Requires<ArgumentException>(endpoint.AddressFamily == AddressFamily.InterNetwork || endpoint.AddressFamily == AddressFamily.InterNetworkV6, "Non-IP Addresses are not supported");

            Services = services;
            Endpoint = endpoint;
        }

        public NetworkAddress(Stream stream) : base(stream) { }
        public NetworkAddress(byte[] buffer, int offset) : base(buffer, offset) { }

        protected override void Deserialize(Stream stream)
        {
            Services = (Services)ReadUInt64(stream);
            var address = ReadBytes(stream, 16);
            var port = ReadUInt16(stream);
            port = (ushort)(port >> 8 & port << 8);
            Endpoint = new IPEndPoint(new IPAddress(address), port);
        }

        public override void Serialize(Stream stream)
        {
            Write(stream, (ulong)Services);
            if (Endpoint.AddressFamily == AddressFamily.InterNetwork)
            {
                WriteBytes(stream, new byte[10]);
                Write(stream, (ushort)0xFFFFU);
                WriteBytes(stream, Endpoint.Address.GetAddressBytes());
            }
            else if (Endpoint.AddressFamily == AddressFamily.InterNetworkV6)
            {
                WriteBytes(stream, Endpoint.Address.GetAddressBytes());
            }
            else
            {
                throw new InvalidOperationException("Unexpected AddressFamily");
            }
            var port = (ushort)(Endpoint.Port >> 8 & Endpoint.Port << 8);
            Write(stream, port);
        }

        public override int SerializedByteSize
        {
            get { return BufferOperations.UINT64_SIZE * 3 + BufferOperations.UINT16_SIZE; }
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
