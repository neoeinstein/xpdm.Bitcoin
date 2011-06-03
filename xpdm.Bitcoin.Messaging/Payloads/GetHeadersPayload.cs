using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Messaging.Payloads
{
    public class GetHeadersPayload : PayloadBase
    {
        public override string Command
        {
            get { return GetHeadersPayload.CommandText; }
        }

        public uint Version { get; private set; }
        public Hash256[] HashStart { get; private set; }
        public Hash256 HashStop { get; private set; }

        public GetHeadersPayload(uint version, Hash256[] hashStart, Hash256 hashStop)
        {
            Contract.Requires<ArgumentNullException>(hashStart != null, "hashStart");
            Contract.Requires<ArgumentNullException>(hashStop != null, "hashStop");

            Version = version;
            HashStart = hashStart;
            HashStop = hashStop;
        }

        public GetHeadersPayload(Stream stream) : base(stream) { }
        public GetHeadersPayload(byte[] buffer, int offset) : base(buffer, offset) { }

        protected override void Deserialize(Stream stream)
        {
            Version = ReadUInt32(stream);
            HashStart = ReadVarArray<Hash256>(stream);
            HashStop = Read<Hash256>(stream);
        }

        public override void Serialize(Stream stream)
        {
            Write(stream, Version);
            WriteVarArray(stream, HashStart);
            Write(stream, HashStop);
        }

        public override int SerializedByteSize
        {
            get { return BufferOperations.UINT32_SIZE + VarIntByteSize(HashStart.Length) + HashStart.Sum(h => h.SerializedByteSize) + HashStop.SerializedByteSize; }
        }

        public static string CommandText
        {
            get { return "getheaders"; }
        }
    }
}
