﻿using System.IO;
using System.Linq;
using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Messaging.Payloads
{
    public class GetBlocksPayload : PayloadBase
    {
        public override string Command
        {
            get { return GetBlocksPayload.CommandText; }
        }

        public uint Version { get; private set; }
        public Hash256[] HashStart { get; private set; }
        public Hash256 HashStop { get; private set; }

        public GetBlocksPayload(uint version, Hash256[] hashStart, Hash256 hashStop)
        {
            ContractsCommon.NotNull(hashStart, "hashStart");
            ContractsCommon.NotNull(hashStop, "hashStop");

            Version = version;
            HashStart = hashStart;
            HashStop = hashStop;
        }

        public GetBlocksPayload(Stream stream) : base(stream) { }
        public GetBlocksPayload(byte[] buffer, int offset) : base(buffer, offset) { }

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

        public override string ToString()
        {
            return string.Format("(ver={0} {{{1}}} => {2})", Version, string.Join<Hash256>(",", HashStart), HashStop);
        }

        public static string CommandText
        {
            get { return "getblocks"; }
        }
    }
}
