using System.IO;
using System.Linq;
using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Messaging.Payloads
{
    public class HeadersPayload : PayloadBase
    {
        public override string Command
        {
            get { return HeadersPayload.CommandText; }
        }

        public BlockHeader[] Headers { get; private set; }

        public HeadersPayload(BlockHeader[] headers)
        {
            Headers = headers;
        }

        public HeadersPayload(Block[] blocks)
        {
            var headers = (from b in blocks select b.Header).ToArray();
            Headers = headers;
        }

        public HeadersPayload(Stream stream) : base(stream) { }
        public HeadersPayload(byte[] buffer, int offset) : base(buffer, offset) { }

        protected override void Deserialize(Stream stream)
        {
            Headers = ReadVarArray<BlockHeader>(stream);
        }

        public override void Serialize(Stream stream)
        {
            WriteVarArray(stream, Headers);
        }

        public override int SerializedByteSize
        {
            get { return VarIntByteSize(Headers.Length) + Headers.Sum(a => a.SerializedByteSize); }
        }

        public override string ToString()
        {
            return "{" + string.Join<BlockHeader>("," + System.Environment.NewLine, Headers) + "}";
        }

        public static string CommandText
        {
            get { return "headers"; }
        }
    }
}
