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

        public Block[] Headers { get; private set; }

        public HeadersPayload(Block[] headers)
            : this(headers, true)
        {
        }

        public HeadersPayload(Block[] headers, bool autoConvertBlocksToHeaders)
        {
            if (autoConvertBlocksToHeaders)
            {
                headers = (from b in headers select b.ToBlockHeader()).ToArray();
            }
            Headers = headers;
        }

        public HeadersPayload(Stream stream) : base(stream) { }
        public HeadersPayload(byte[] buffer, int offset) : base(buffer, offset) { }

        protected override void Deserialize(Stream stream)
        {
            Headers = ReadVarArray<Block>(stream);
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
            return "{" + string.Join<Block>("," + System.Environment.NewLine, Headers) + "}";
        }

        public static string CommandText
        {
            get { return "headers"; }
        }
    }
}
