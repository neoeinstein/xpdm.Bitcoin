using System;
using System.Diagnostics.Contracts;
using System.IO;
using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Messaging.Payloads
{
    public class BlockPayload : PayloadBase
    {
        public override string Command
        {
            get { return BlockPayload.CommandText; }
        }

        public Block Block { get; private set; }

        public BlockPayload(Block block)
        {
            ContractsCommon.NotNull(block, "block");

            Block = block;
        }

        public BlockPayload(Stream stream) : base(stream) { }
        public BlockPayload(byte[] buffer, int offset) : base(buffer, offset) { }

        protected override void Deserialize(Stream stream)
        {
            Block = Read<Block>(stream);
        }

        public override void Serialize(Stream stream)
        {
            Write(stream, Block);
        }

        public override int SerializedByteSize
        {
            get { return Block.SerializedByteSize; }
        }

        public static string CommandText
        {
            get { return "block"; }
        }
    }
}
