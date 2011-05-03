using System;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin
{
    public class ReplyPayload : MessagePayloadBase
    {
        public override string Command
        {
            get { return ReplyPayload.CommandText; }
        }

        public override uint ByteSize
        {
            get 
            {
                return (uint)ReplyPayload.MinimumByteSize;
            }
        }

        public TransactionReply Reply { get; private set; }

        public ReplyPayload(TransactionReply reply)
        {
            Reply = reply;
        }

        public ReplyPayload(byte[] buffer, int offset)
            : base(buffer, offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Length >= ReplyPayload.MinimumByteSize, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - ReplyPayload.MinimumByteSize, "offset");

            Reply = (TransactionReply) buffer.ReadUInt32(offset);
        }

        public static string CommandText
        {
            get
            {
                return "reply";
            }
        }

        public static int MinimumByteSize
        {
            get
            {
                return BitcoinBufferOperations.UINT32_SIZE;
            }
        }

        [Pure]
        public override void WriteToBitcoinBuffer(byte[] buffer, int offset)
        {
            ((uint)Reply).WriteBytes(buffer, offset);
        }
    }
}
