using System;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin
{
    public class GetHeadersPayload : MessagePayloadBase
    {
        public override string Command
        {
            get { return GetHeadersPayload.CommandText; }
        }

        public override uint ByteSize
        {
            get { return BitcoinBufferOperations.UINT32_SIZE + StartCount.ByteSize + (uint)(StartCount > 0 ? StartCount * HashStart[0].ByteSize : 0) + HashStop.ByteSize; }
        }

        public uint Version { get; private set; }
        public VarInt StartCount { get; private set; }
        private readonly Hash[] _hashStart;
        public Hash[] HashStart
        {
            get
            {
                Contract.Ensures(Contract.Result<Hash[]>() != null);
                Contract.Ensures((uint)Contract.Result<Hash[]>().Length == StartCount);

                var retVal = new Hash[_hashStart.Length];
                Array.Copy(_hashStart, retVal, _hashStart.Length);

                return retVal;
            }
        }
        public Hash HashStop { get; private set; }

        public GetHeadersPayload(uint version, Hash[] hashStart, Hash hashStop)
        {
            Contract.Requires<ArgumentNullException>(hashStart != null, "hashStart");
            Contract.Requires<ArgumentNullException>(hashStop != null, "hashStop");

            Version = version;
            _hashStart = new Hash[hashStart.Length];
            Array.Copy(hashStart, _hashStart, hashStart.Length);
            StartCount = new VarInt((uint)hashStart.Length);
            HashStop = hashStop;
        }

        public GetHeadersPayload(byte[] buffer, int offset)
            : base(buffer, offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Length >= GetHeadersPayload.MinimumByteSize, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - GetHeadersPayload.MinimumByteSize, "offset");

            Version = buffer.ReadUInt32(offset);
            StartCount = new VarInt(buffer, offset + STARTCOUNT_OFFSET);
            _hashStart = buffer.ReadArray<Hash>(offset + STARTCOUNT_OFFSET + (int)StartCount.ByteSize, StartCount);
            HashStop = new Hash(buffer, offset + STARTCOUNT_OFFSET + (int)StartCount.ByteSize + (int)(StartCount > 0 ? StartCount * HashStart[0].ByteSize : 0));
        }

        private const int STARTCOUNT_OFFSET = BitcoinBufferOperations.UINT32_SIZE;

        [Pure]
        public override void WriteToBitcoinBuffer(byte[] buffer, int offset)
        {
            Version.WriteBytes(buffer, offset);
            StartCount.WriteToBitcoinBuffer(buffer, offset + STARTCOUNT_OFFSET);
            BitcoinBufferOperations.WriteBytes(_hashStart, buffer, offset + STARTCOUNT_OFFSET + (int)StartCount.ByteSize, StartCount);
            HashStop.WriteToBitcoinBuffer(buffer, offset + STARTCOUNT_OFFSET + (int)StartCount.ByteSize + (int)(StartCount > 0 ? StartCount * HashStart[0].ByteSize : 0));
        }

        public static string CommandText
        {
            get { return "getheaders"; }
        }

        public static int MinimumByteSize
        {
            get { return BitcoinBufferOperations.UINT32_SIZE + VarInt.MinimumByteSize +  Hash.MinimumByteSize; }
        }
    }
}
