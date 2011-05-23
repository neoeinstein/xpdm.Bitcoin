using System;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin.Protocol
{
    public class PingPayload : MessagePayloadBase
    {
        public override string Command
        {
            get { return PingPayload.CommandText; }
        }

        public PingPayload()
        {
            ByteSize = (uint)PingPayload.ConstantByteSize;
        }

        public PingPayload(byte[] buffer, int offset)
            : base(buffer, offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Length >= PingPayload.ConstantByteSize, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - PingPayload.ConstantByteSize, "offset");

            ByteSize = (uint)PingPayload.ConstantByteSize;
        }

        public static string CommandText
        {
            get
            {
                return "ping";
            }
        }

        public static int ConstantByteSize
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
