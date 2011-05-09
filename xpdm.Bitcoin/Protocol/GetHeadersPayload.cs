using System;
using System.Diagnostics.Contracts;
using xpdm.Bitcoin;

namespace xpdm.Bitcoin.Protocol
{
    public class GetHeadersPayload : MessagePayloadBase
    {
        public override string Command
        {
            get { return GetHeadersPayload.CommandText; }
        }

        public uint Version { get; private set; }
        public VarInt StartCount { get; private set; }
        public VarArray<Hash> HashStart { get; private set; }
        public Hash HashStop { get; private set; }

        public GetHeadersPayload(uint version, Hash[] hashStart, Hash hashStop)
        {
            Contract.Requires<ArgumentNullException>(hashStart != null, "hashStart");
            Contract.Requires<ArgumentNullException>(hashStop != null, "hashStop");

            Version = version;
            HashStart = new VarArray<Hash>(hashStart);
            StartCount = new VarInt((uint)hashStart.Length);
            HashStop = hashStop;

            ByteSize = Version.ByteSize() + StartCount.ByteSize + HashStart.ByteSize + HashStop.ByteSize;
        }

        public GetHeadersPayload(byte[] buffer, int offset)
            : base(buffer, offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Length >= GetHeadersPayload.MinimumByteSize, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - GetHeadersPayload.MinimumByteSize, "offset");

            Version = buffer.ReadUInt32(offset);
            StartCount = new VarInt(buffer, StartCount_Offset(ref offset));
            HashStart = new VarArray<Hash>(buffer, HashStart_Offset(ref offset));
            HashStop = new Hash(buffer, HashStop_Offset(ref offset));

            ByteSize = Version.ByteSize() + StartCount.ByteSize + HashStart.ByteSize + HashStop.ByteSize;
        }

        private int StartCount_Offset(ref int offset) { return offset += (int)Version.ByteSize(); }
        private int HashStart_Offset(ref int offset) { return offset += (int)StartCount.ByteSize; }
        private int HashStop_Offset(ref int offset) { return offset += (int)HashStart.ByteSize; }

        [Pure]
        public override void WriteToBitcoinBuffer(byte[] buffer, int offset)
        {
            Version.WriteBytes(buffer, offset);
            StartCount.WriteToBitcoinBuffer(buffer, StartCount_Offset(ref offset));
            HashStart.WriteToBitcoinBuffer(buffer, HashStart_Offset(ref offset));
            HashStop.WriteToBitcoinBuffer(buffer, HashStop_Offset(ref offset));
        }

        public static string CommandText
        {
            get { return "getheaders"; }
        }

        public static int MinimumByteSize
        {
            get { return BitcoinBufferOperations.UINT32_SIZE + VarArray<Hash>.MinimumByteSize + Hash.ConstantByteSize; }
        }
    }
}
