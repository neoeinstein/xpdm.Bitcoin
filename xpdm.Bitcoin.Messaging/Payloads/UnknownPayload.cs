using System;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin.Messaging.Payloads
{
    public class UnknownPayload : PayloadBase
    {
        private readonly string _command;
        public override string Command
        {
            get { return _command; }
        }

        [ContractPublicPropertyName("Bytes")]
        private readonly byte[] _bytes;
        public byte[] Bytes
        {
            get
            {
                Contract.Ensures(Contract.Result<byte[]>() != null);
                Contract.Ensures(Contract.Result<byte[]>().Length == ByteSize);

                // Use a copy of the byte array to ensure that the original array is
                // not altered, and subsequent calls will return the original payload.
                var retVal = new byte[ByteSize];
                Array.Copy(_bytes, retVal, ByteSize);
                return retVal;

            }
        }

        public UnknownPayload()
        {
            _command = string.Empty;
            _bytes = new byte[0];

            ByteSize = 0;
        }

        public UnknownPayload(string command, byte[] buffer, int offset, uint length)
            : base(buffer, offset)
        {
            Contract.Requires<ArgumentNullException>(command != null, "command");
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Length >= length, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - length, "length");

            _command = command;
            _bytes = new byte[length];
            Array.Copy(buffer, offset, _bytes, 0, length);

            ByteSize = length;
        }

        [Pure]
        public override void WriteToBitcoinBuffer(byte[] buffer, int offset)
        {
            Array.Copy(_bytes, 0, buffer, offset, ByteSize);
        }

        public static int MinimumByteSize
        {
            get
            {
                return 0;
            }
        }

        [ContractInvariantMethod]
        private void __Invariant()
        {
            Contract.Invariant(ByteSize == _bytes.Length);
            Contract.Invariant(Command != null);
            Contract.Invariant(_bytes != null);
        }
    }
}
