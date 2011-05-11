using System;
using System.Linq;
using System.Diagnostics.Contracts;
using xpdm.Bitcoin;

namespace xpdm.Bitcoin.Protocol
{
    public class HeadersPayload : MessagePayloadBase
    {
        public override string Command
        {
            get { return HeadersPayload.CommandText; }
        }

        public VarArray<Block> Headers { get; private set; }

        public HeadersPayload(Block[] headers)
            : this(headers, true)
        {
        }

        public HeadersPayload(Block[] headers, bool autoConvertBlocksToHeaders)
        {
            if (autoConvertBlocksToHeaders)
            {
                headers = (from b in headers select b.ConvertToBlockHeader()).ToArray();
            }
            Headers = new VarArray<Block>(headers);

            ByteSize = Headers.ByteSize;
        }

        public HeadersPayload(byte[] buffer, int offset)
            : base(buffer, offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Length >= HeadersPayload.MinimumByteSize, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - HeadersPayload.MinimumByteSize, "offset");

            Headers = new VarArray<Block>(buffer, offset);

            ByteSize = Headers.ByteSize;
        }

        [Pure]
        public override void WriteToBitcoinBuffer(byte[] buffer, int offset)
        {
            Headers.WriteToBitcoinBuffer(buffer, offset);
        }

        public static string CommandText
        {
            get { return "headers"; }
        }

        public static int MinimumByteSize
        {
            get
            {
                return VarArray<Block>.MinimumByteSize;
            }
        }
    }
}
