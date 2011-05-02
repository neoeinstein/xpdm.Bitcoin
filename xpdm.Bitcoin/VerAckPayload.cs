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

        public override uint ByteSize
        {
            get 
            {
                return (uint)VerAckPayload.MinimumByteSize;
            }
        }

        public VerAckPayload()
        {
        }

        public VerAckPayload(byte[] buffer, int offset)
            : base(buffer, offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Length >= VerAckPayload.MinimumByteSize, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - VerAckPayload.MinimumByteSize, "offset");
        }

        public static int MinimumByteSize
        {
            get
            {
                return 0;
            }
        }

        [Pure]
        public override void WriteToBitcoinBuffer(byte[] buffer, int offset)
        {
        }
    }
}
