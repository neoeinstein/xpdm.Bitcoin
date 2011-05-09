using System;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin
{
    public class BlockPayload : MessagePayloadBase
    {
        public override string Command
        {
            get { return BlockPayload.CommandText; }
        }

        public Block Block { get; private set; }

        public BlockPayload(Block block)
        {
            Block = block;

            ByteSize = Block.ByteSize;
        }

        public BlockPayload(byte[] buffer, int offset)
            : base(buffer, offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Length >= BlockPayload.MinimumByteSize, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - BlockPayload.MinimumByteSize, "offset");

            Block = new Block(buffer, offset);

            ByteSize = Block.ByteSize;
        }

        [Pure]
        public override void WriteToBitcoinBuffer(byte[] buffer, int offset)
        {
            Block.WriteToBitcoinBuffer(buffer, offset);
        }

        public static string CommandText
        {
            get { return "block"; }
        }

        public static int MinimumByteSize
        {
            get
            {
                return Block.MinimumByteSize;
            }
        }
    }
}
