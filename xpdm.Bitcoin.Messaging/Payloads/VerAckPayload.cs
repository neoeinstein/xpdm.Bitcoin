using System.IO;

namespace xpdm.Bitcoin.Messaging.Payloads
{
    public class VerAckPayload : PayloadBase
    {
        public override string Command
        {
            get { return VerAckPayload.CommandText; }
        }

        public override bool IncludeChecksum
        {
            get { return false; }
        }

        public VerAckPayload(Stream stream) : base(stream) { }
        public VerAckPayload(byte[] buffer, int offset) : base(buffer, offset) { }

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
                return "verack";
            }
        }
    }
}
