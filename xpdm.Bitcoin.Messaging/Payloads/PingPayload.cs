using System.IO;

namespace xpdm.Bitcoin.Messaging.Payloads
{
    public class PingPayload : PayloadBase
    {
        public override string Command
        {
            get { return PingPayload.CommandText; }
        }

        public PingPayload(Stream stream) : base(stream) { }
        public PingPayload(byte[] buffer, int offset) : base(buffer, offset) { }

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

        public static string CommandText
        {
            get
            {
                return "ping";
            }
        }
    }
}
