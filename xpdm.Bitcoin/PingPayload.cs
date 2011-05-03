using System;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin
{
    public class PingPayload : MessagePayloadBase
    {
        public override string Command
        {
            get { return PingPayload.CommandText; }
        }

        public override uint ByteSize
        {
            get 
            {
                return (uint)PingPayload.MinimumByteSize;
            }
        }

        public PingPayload()
        {
        }

        public PingPayload(byte[] buffer, int offset)
            : base(buffer, offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Length >= PingPayload.MinimumByteSize, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - PingPayload.MinimumByteSize, "offset");
        }

        public static string CommandText
        {
            get
            {
                return "ping";
            }
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
