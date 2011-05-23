using System;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin.Protocol
{
    public class AlertPayload : MessagePayloadBase
    {
        public override string Command
        {
            get { return AlertPayload.CommandText; }
        }

        public VarString Message { get; private set; }
        public VarString Signature { get; private set; }

        public AlertPayload(VarString message, VarString signature)
        {
            Contract.Requires<ArgumentNullException>(message != null, "message");
            Contract.Requires<ArgumentNullException>(signature != null, "signature");

            Message = message;
            Signature = signature;

            ByteSize = Message.ByteSize + Signature.ByteSize;
        }

        public AlertPayload(byte[] buffer, int offset)
            : base(buffer, offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Length >= AlertPayload.MinimumByteSize, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - AlertPayload.MinimumByteSize, "offset");

            Message = new VarString(buffer, offset);
            Signature = new VarString(buffer, offset + (int)Message.ByteSize);

            ByteSize = Message.ByteSize + Signature.ByteSize;
        }

        [Pure]
        public override void WriteToBitcoinBuffer(byte[] buffer, int offset)
        {
            Message.WriteToBitcoinBuffer(buffer, offset);
            Signature.WriteToBitcoinBuffer(buffer, offset + (int)Message.ByteSize);
        }

        public static string CommandText
        {
            get { return "alert"; }
        }

        public static int MinimumByteSize
        {
            get
            {
                return VarString.MinimumByteSize * 2;
            }
        }
    }
}
