using System.IO;

namespace xpdm.Bitcoin.Messaging.Payloads
{
    public class GetAddrPayload : PayloadBase
    {
        public override string Command
        {
            get { return GetAddrPayload.CommandText; }
        }

        public GetAddrPayload(Stream stream) : base(stream) { }
        public GetAddrPayload(byte[] buffer, int offset) : base(buffer, offset) { }

        protected override void Deserialize(Stream stream)
        {
        }

        public override void Serialize(Stream stream)
        {
        }

        public override int SerializedByteSize
        {
            get { return 0; }
        }

        public override string ToString()
        {
            return "{ getaddr }";
        }

        public static string CommandText
        {
            get
            {
                return "getaddr";
            }
        }
    }
}
