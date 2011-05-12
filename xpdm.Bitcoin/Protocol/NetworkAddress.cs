using System;
using System.Diagnostics.Contracts;
using System.Net;
using System.Net.Sockets;
using xpdm.Bitcoin;

namespace xpdm.Bitcoin.Protocol
{
    public class NetworkAddress : BitcoinSerializableBase
    {
        public Services Services { get; private set; }
        public IPEndPoint Endpoint { get; private set; }

        public NetworkAddress(Services services, IPEndPoint endpoint)
        {
            Contract.Requires<ArgumentNullException>(endpoint != null, "endpoint");
            Contract.Requires<ArgumentException>(endpoint.AddressFamily == AddressFamily.InterNetwork || endpoint.AddressFamily == AddressFamily.InterNetworkV6, "Non-IP Addresses are not supported");

            Services = services;
            Endpoint = endpoint;

            ByteSize = (uint)NetworkAddress.ConstantByteSize;
        }

        public NetworkAddress(byte[] buffer, int offset)
            : base(buffer, offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Length >= NetworkAddress.ConstantByteSize, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - NetworkAddress.ConstantByteSize, "offset");

            Services = (Services)buffer.ReadUInt64(offset);
            var address = new byte[16];
            Array.Copy(buffer, AddressIPv6_Offset(ref offset), address, 0, 16);
            Endpoint = new IPEndPoint(
                          new IPAddress(address),
                          buffer.ReadUInt16BE(Port_Offset(ref offset)));

            ByteSize = (uint)NetworkAddress.ConstantByteSize;
        }

        private int AddressIPv6_Offset(ref int offset) { return offset += (int)((ulong)Services).ByteSize(); }
        private int AddressIPv4Marker_Offset(ref int offset) { return offset += BufferOperations.UINT64_SIZE; }
        private int AddressIPv4_Offset(ref int offset) { return offset += BufferOperations.UINT32_SIZE; }
        private int Port_Offset(ref int offset) { return offset += BufferOperations.UINT64_SIZE * 2; }

        [Pure]
        public override void WriteToBitcoinBuffer(byte[] buffer, int offset)
        {
            ((ulong)Services).WriteBytes(buffer, offset);
            var subOffset = offset;
            switch(Endpoint.AddressFamily)
            {
                case AddressFamily.InterNetwork:
                    ((ushort)0xFFFF).WriteBytes(buffer, AddressIPv4Marker_Offset(ref subOffset));
                    Endpoint.Address.GetAddressBytes().CopyTo(buffer, AddressIPv4_Offset(ref subOffset));
                    break;
                case AddressFamily.InterNetworkV6:
                    Endpoint.Address.GetAddressBytes().CopyTo(buffer, AddressIPv6_Offset(ref subOffset));
                    break;
            }
            ((ushort)Endpoint.Port).WriteBytesBE(buffer, Port_Offset(ref offset));
        }

        public static int ConstantByteSize
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
