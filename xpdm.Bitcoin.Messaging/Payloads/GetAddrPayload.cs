using System;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin.Messaging.Payloads
{
    public class GetAddrPayload : PayloadBase
    {
        public override string Command
        {
            get { return GetAddrPayload.CommandText; }
        }

        public GetAddrPayload()
        {
            ByteSize = 0;
        }

        public GetAddrPayload(byte[] buffer, int offset)
            : base(buffer, offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Length >= GetAddrPayload.ConstantByteSize, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - GetAddrPayload.ConstantByteSize, "offset");

            ByteSize = 0;
        }

        public static string CommandText
        {
            get
            {
                return "getaddr";
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
