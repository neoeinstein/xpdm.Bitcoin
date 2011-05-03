using System;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin
{
    public class GetAddrPayload : MessagePayloadBase
    {
        public override string Command
        {
            get { return GetAddrPayload.CommandText; }
        }

        public override uint ByteSize
        {
            get 
            {
                return (uint)GetAddrPayload.MinimumByteSize;
            }
        }

        public GetAddrPayload()
        {
        }

        public GetAddrPayload(byte[] buffer, int offset)
            : base(buffer, offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Length >= GetAddrPayload.MinimumByteSize, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - GetAddrPayload.MinimumByteSize, "offset");
        }

        public static string CommandText
        {
            get
            {
                return "getaddr";
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
