using System;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin
{
    public class VerAckPayload : MessagePayloadBase
    {
        public override string Command
        {
            get { return VerAckPayload.CommandText; }
        }

        public override bool IncludeChecksum
        {
            get { return false; }
        }

        public VerAckPayload()
        {
            ByteSize = (uint)VerAckPayload.ConstantByteSize;
        }

        public VerAckPayload(byte[] buffer, int offset)
            : base(buffer, offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Length >= VerAckPayload.ConstantByteSize, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - VerAckPayload.ConstantByteSize, "offset");

            ByteSize = (uint)VerAckPayload.ConstantByteSize;
        }

        public static string CommandText
        {
            get
            {
                return "verack";
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
