using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace xpdm.Bitcoin.Messaging.Payloads
{
    public class AlertPayload : PayloadBase
    {
        public override string Command
        {
            get { return AlertPayload.CommandText; }
        }

        public byte[] Message { get; private set; }
        public byte[] Signature { get; private set; }

        public AlertPayload(byte[] message, byte[] signature)
        {
            Contract.Requires<ArgumentNullException>(message != null, "message");
            Contract.Requires<ArgumentNullException>(signature != null, "signature");

            Message = message;
            Signature = signature;
        }

        public AlertPayload(Stream stream) : base(stream) { }
        public AlertPayload(byte[] buffer, int offset) : base(buffer, offset) { }

        protected override void Deserialize(Stream stream)
        {
            var messageLen = ReadVarInt(stream);
            Message = ReadBytes(stream, (int)messageLen);
            var sigLen = ReadVarInt(stream);
            Signature = ReadBytes(stream, (int)sigLen);
        }

        public override void Serialize(Stream stream)
        {
            WriteVarInt(stream, Message.LongLength);
            WriteBytes(stream, Message);
            WriteVarInt(stream, Signature.LongLength);
            WriteBytes(stream, Signature);
        }

        public override int SerializedByteSize
        {
            get { return VarIntByteSize(Message.Length) + Message.Length + VarIntByteSize(Signature.Length) + Signature.Length; }
        }

        public static string CommandText
        {
            get { return "alert"; }
        }
    }
}
