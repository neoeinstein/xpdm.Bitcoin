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

        private static readonly int VERSION_106_LENGTH = VersionPayload.MinimumByteSize + NetworkAddress.ConstantByteSize + BitcoinBufferOperations.UINT64_SIZE;
        private static readonly int VERSION_209_LENGTH = VERSION_106_LENGTH + BitcoinBufferOperations.UINT32_SIZE;
        private uint CalculateByteSize()
        {
            if (Version >= 209)
            {
                return (uint)(VERSION_209_LENGTH + SubVersionNum.ByteSize);
            }
            if (Version >= 106)
            {
                return (uint)(VERSION_106_LENGTH + SubVersionNum.ByteSize);
            }
            return (uint)VersionPayload.MinimumByteSize;
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

            ByteSize = CalculateByteSize();
        }

        public VersionPayload(byte[] buffer, int offset)
            : base(buffer, offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Length >= VersionPayload.MinimumByteSize, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - VersionPayload.MinimumByteSize, "offset");

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

            ByteSize = CalculateByteSize();
        }

        private const int SERVICES_OFFSET = BitcoinBufferOperations.UINT32_SIZE;
        private const int TIMESTAMP_OFFSET = SERVICES_OFFSET + BitcoinBufferOperations.UINT64_SIZE;
        private const int EMIT_ADDRESS_OFFSET = TIMESTAMP_OFFSET + BitcoinBufferOperations.UINT64_SIZE;
        private static readonly int RECV_ADDRESS_OFFSET = EMIT_ADDRESS_OFFSET + NetworkAddress.ConstantByteSize;
        private static readonly int NONCE_OFFSET = RECV_ADDRESS_OFFSET + NetworkAddress.ConstantByteSize;
        private static readonly int SUBVER_OFFSET = NONCE_OFFSET + BitcoinBufferOperations.UINT64_SIZE;
        private static readonly int STARTHEIGHT_OFFSET = SUBVER_OFFSET;

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
            get { return BitcoinBufferOperations.UINT32_SIZE + BitcoinBufferOperations.UINT64_SIZE * 2 + NetworkAddress.ConstantByteSize; }
        }

        [ContractInvariantMethod]
        private void __Invariant()
        {
            Contract.Invariant(EmittingAddress != null);
            Contract.Invariant(ReceivingAddress != null);
        }
    }
}
