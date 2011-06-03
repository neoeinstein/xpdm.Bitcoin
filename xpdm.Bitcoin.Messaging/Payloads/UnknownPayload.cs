using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace xpdm.Bitcoin.Messaging.Payloads
{
    public class UnknownPayload : PayloadBase
    {
        private readonly string _command;
        public override string Command
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);

                return _command;
            }
        }

        public byte[] Bytes { get; private set; }

        public UnknownPayload(string command, Stream stream, int length)
        {
            _command = command;
            Bytes = ReadBytes(stream, length);
        }

        public UnknownPayload(string command, byte[] buffer, int offset, int length)
        {
            _command = command;
            Bytes = new byte[length];
            Array.Copy(buffer, offset, Bytes, 0, length);
        }

        protected override void Deserialize(Stream stream)
        {
            throw new NotSupportedException();
        }

        public override void Serialize(Stream stream)
        {
            WriteBytes(stream, Bytes);
        }

        public override int SerializedByteSize
        {
            get { return Bytes.Length; }
        }

        [ContractInvariantMethod]
        private void __Invariant()
        {
            Contract.Invariant(Command != null);
            Contract.Invariant(Bytes != null);
        }
    }
}
