using System.Diagnostics.Contracts;
using System.IO;
using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Messaging.Payloads
{
    public class VersionPayload : PayloadBase
    {
        public override string Command
        {
            get { return VersionPayload.CommandText; }
        }

        public override bool IncludeChecksum
        {
            get { return false; }
        }

        public uint Version { get; private set; } //31402
        public Services Services { get; private set; }
        public Timestamp Timestamp { get; private set; }
        public NetworkAddress EmittingAddress { get; private set; }
        public NetworkAddress ReceivingAddress { get; private set; }
        public ulong Nonce { get; private set; }
        public byte[] SubVersionNum { get; private set; }
        public uint StartHeight { get; private set; }

        public VersionPayload(uint version, Services services, Timestamp timestamp, NetworkAddress emittingAddress,
            NetworkAddress receivingAddress, ulong nonce, byte[] subVersionNum, uint startHeight)
        {
            ContractsCommon.NotNull(emittingAddress, "emittingAddress");
            ContractsCommon.NotNull(receivingAddress, "receivingAddress");
            ContractsCommon.NotNull(subVersionNum, "subVersionNum");

            Version = version;
            Services = services;
            Timestamp = timestamp;
            EmittingAddress = emittingAddress;
            ReceivingAddress = receivingAddress;
            Nonce = nonce;
            SubVersionNum = subVersionNum;
            StartHeight = startHeight;
        }

        public VersionPayload(Stream stream) : base(stream) { }
        public VersionPayload(byte[] buffer, int offset) : base(buffer, offset) { }

        protected override void Deserialize(Stream stream)
        {
            Version = ReadUInt32(stream);
            Services = (Services)ReadUInt64(stream);
            Timestamp = (Timestamp)ReadUInt64(stream);
            EmittingAddress = Read<NetworkAddress>(stream);

            if (Version >= 106)
            {
                ReceivingAddress = Read<NetworkAddress>(stream);
                Nonce = ReadUInt64(stream);
                var subVersionLen = (int)ReadVarInt(stream);
                SubVersionNum = ReadBytes(stream, subVersionLen);

                if (Version >= 209)
                {
                    StartHeight = ReadUInt32(stream);
                }
            }
            else
            {
                ReceivingAddress = NetworkAddress.IPv6Any;
            }
        }

        public override void Serialize(Stream stream)
        {
            Write(stream, Version);
            Write(stream, (ulong)Services);
            Write(stream, (ulong)Timestamp);
            Write(stream, EmittingAddress);

            if (Version >= 106)
            {
                Write(stream, ReceivingAddress);
                Write(stream, Nonce);
                WriteVarInt(stream, SubVersionNum.Length);
                WriteBytes(stream, SubVersionNum);

                if (Version >= 209)
                {
                    Write(stream, StartHeight);
                }
            }
        }

        public override int SerializedByteSize
        {
            get
            {
                var size = BufferOperations.UINT32_SIZE + BufferOperations.UINT64_SIZE + BufferOperations.UINT64_SIZE + EmittingAddress.SerializedByteSize;
                if (Version >= 106)
                {
                    size += ReceivingAddress.SerializedByteSize + BufferOperations.UINT64_SIZE + VarIntByteSize(SubVersionNum.Length) + SubVersionNum.Length;

                    if (Version >= 209)
                    {
                        size += BufferOperations.UINT32_SIZE;
                    }
                }
                return size;
            }
        }


        public static string CommandText
        {
            get { return "version"; }
        }

        [ContractInvariantMethod]
        private void __Invariant()
        {
            Contract.Invariant(EmittingAddress != null);
            Contract.Invariant(ReceivingAddress != null);
        }
    }
}
