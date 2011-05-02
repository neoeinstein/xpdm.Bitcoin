using System;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin
{
    public class VersionPayload : MessagePayloadBase
    {
        public override string Command
        {
            get { return VersionPayload.CommandText; }
        }

        public override bool IncludeChecksum
        {
            get { return false; }
        }

        private const uint BASIC_VERSION_LENGTH = 46;
        private const uint VERSION_106_LENGTH = BASIC_VERSION_LENGTH + 34;
        private const uint VERSION_209_LENGTH = VERSION_106_LENGTH + 4;
        public override uint ByteSize
        {
            get 
            {
                if (Version >= 209)
                {
                    return VERSION_209_LENGTH + (uint)SubVersionNum.ByteSize;
                }
                if (Version >= 106)
                {
                    return VERSION_106_LENGTH + (uint)SubVersionNum.ByteSize;
                }
                return BASIC_VERSION_LENGTH;
            }
        }

        public uint Version { get; private set; } //31402
        public Services Services { get; private set; }
        public ulong Timestamp { get; private set; }
        public NetworkAddress EmittingAddress { get; private set; }
        public NetworkAddress ReceivingAddress { get; private set; }
        public ulong Nonce { get; private set; }
        public VarString SubVersionNum { get; private set; }
        public uint StartHeight { get; private set; }

        public VersionPayload(uint version, Services services, ulong timestamp, NetworkAddress emittingAddress,
            NetworkAddress receivingAddress, ulong nonce, VarString subVersionNum, uint startHeight)
        {
            Contract.Requires<ArgumentNullException>(emittingAddress != null);
            Contract.Requires<ArgumentNullException>(receivingAddress != null);

            Version = version;
            Services = services;
            Timestamp = timestamp;
            EmittingAddress = emittingAddress;
            ReceivingAddress = receivingAddress;
            Nonce = nonce;
            SubVersionNum = subVersionNum;
            StartHeight = startHeight;
        }

        public VersionPayload(byte[] buffer, int offset)
            : base(buffer, offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Length >= BASIC_VERSION_LENGTH, "buffer");
            //Contract.Requires<ArgumentException>(buffer[offset] >= 106 && buffer.Length >= VERSION_106_LENGTH, "buffer");
            //Contract.Requires<ArgumentException>(buffer[offset] >= 209 && buffer.Length >= VERSION_209_LENGTH, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - BASIC_VERSION_LENGTH, "offset");
            //Contract.Requires<ArgumentOutOfRangeException>(buffer[offset] >= 106 && offset <= buffer.Length - VERSION_106_LENGTH, "offset");
            //Contract.Requires<ArgumentOutOfRangeException>(buffer[offset] >= 209 && offset <= buffer.Length - VERSION_209_LENGTH, "offset");

            Version = buffer.ReadUInt32(offset);
            Services = (Services)buffer.ReadUInt64(offset + SERVICES_OFFSET);
            Timestamp = buffer.ReadUInt64(offset + TIMESTAMP_OFFSET);
            EmittingAddress = new NetworkAddress(buffer, offset + EMIT_ADDRESS_OFFSET);
            if (Version >= 106)
            {
                ReceivingAddress = new NetworkAddress(buffer, offset + RECV_ADDRESS_OFFSET);
                Nonce = buffer.ReadUInt64(offset + NONCE_OFFSET);
                SubVersionNum = new VarString(buffer, offset + SUBVER_OFFSET);
            }
            else
            {
                ReceivingAddress = NetworkAddress.IPv6Any;
            }
            if (Version >= 209)
            {
                StartHeight = buffer.ReadUInt32(offset + STARTHEIGHT_OFFSET);
            }
        }

        private const int SERVICES_OFFSET = 4;
        private const int TIMESTAMP_OFFSET = SERVICES_OFFSET + 8;
        private const int EMIT_ADDRESS_OFFSET = TIMESTAMP_OFFSET + 8;
        private const int RECV_ADDRESS_OFFSET = EMIT_ADDRESS_OFFSET + 26;
        private const int NONCE_OFFSET = RECV_ADDRESS_OFFSET + 26;
        private const int SUBVER_OFFSET = NONCE_OFFSET + 8;
        private const int STARTHEIGHT_OFFSET = SUBVER_OFFSET;

        [Pure]
        public override void WriteToBitcoinBuffer(byte[] buffer, int offset)
        {
            Version.WriteBytes(buffer, offset);
            ((ulong)Services).WriteBytes(buffer, offset + SERVICES_OFFSET);
            Timestamp.WriteBytes(buffer, offset + TIMESTAMP_OFFSET);
            EmittingAddress.WriteToBitcoinBuffer(buffer, offset + EMIT_ADDRESS_OFFSET);
            if (Version >= 106)
            {
                ReceivingAddress.WriteToBitcoinBuffer(buffer, offset + RECV_ADDRESS_OFFSET);
                Nonce.WriteBytes(buffer, offset + NONCE_OFFSET);
                SubVersionNum.WriteToBitcoinBuffer(buffer, offset + SUBVER_OFFSET);
            }
            if (Version >= 209)
            {
                StartHeight.WriteBytes(buffer, offset + STARTHEIGHT_OFFSET);
            }
        }

        public static string CommandText
        {
            get { return "version"; }
        }

        public static int MinimumByteSize
        {
            get { return BitcoinBufferOperations.UINT32_SIZE + BitcoinBufferOperations.UINT64_SIZE * 2 + NetworkAddress.MinimumByteSize; }
        }

        [ContractInvariantMethod]
        private void __Invariant()
        {
            Contract.Invariant(EmittingAddress != null);
            Contract.Invariant(ReceivingAddress != null);
        }
    }
}
