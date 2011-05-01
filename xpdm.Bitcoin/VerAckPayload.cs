using System;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin
{
    public class VerAckPayload : MessagePayloadBase
    {
        public override string Command
        {
            get { return "verack"; }
        }

        public override bool IncludeChecksum
        {
            get { return false; }
        }

        private const uint BYTESIZE = 0;
        public override uint ByteSize
        {
            get 
            {
                return BYTESIZE;
            }
        }

        public VerAckPayload()
        {
        }

        public VerAckPayload(byte[] buffer, int offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Length >= BYTESIZE, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - BYTESIZE, "offset");
        }

        [Pure]
        public override void WriteToBitcoinBuffer(byte[] buffer, int offset)
        {
        }
    }
}
