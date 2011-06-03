using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using xpdm.Bitcoin.Core;
using xpdm.Bitcoin.Messaging.Payloads;

namespace xpdm.Bitcoin.Messaging
{
    public class Message : BitcoinSerializable
    {
        /// <summary>
        /// Indicates the network from which this message originated.
        /// </summary>
        public Network Network
        {
            get;
            private set;
        }

        private const int MAX_COMMAND_LENGTH = 12;
        /// <summary>
        /// Identifies the contents of this message. When read from a buffer,
        /// this string is the literal command parameter. To get the normative
        /// command string, use <c ref="NormativeCommand"/>.
        /// </summary>
        public string Command
        {
            get;
            private set;
        }

        /// <summary>
        /// Length in bytes of the payload (not including the headers). For
        /// a calculated value, use <c ref="NormativePayloadLength"/>.
        /// </summary>
        public uint PayloadLength
        {
            get;
            private set;
        }

        /// <summary>
        /// The first 4 bytes of SHA256(SHA256(Payload))
        /// </summary>
        public uint Checksum
        {
            get;
            private set;
        }

        public IPayload Payload
        {
            get;
            private set;
        }

        public Message(Stream stream) : base(stream) { }
        public Message(byte[] buffer, int offset) : base(buffer, offset) { }

        protected override void Deserialize(Stream stream)
        {
            Network = (Network)ReadUInt32(stream);
            var commandBytes = ReadBytes(stream, MAX_COMMAND_LENGTH);
            Command = Encoding.ASCII.GetString(commandBytes).TrimEnd('\0');
            PayloadLength = ReadUInt32(stream);

            if (PayloadFactory.PayloadRequiresChecksum(Command))
            {
                Checksum = ReadUInt32(stream);
            }

            Payload = PayloadFactory.ConstructPayload(Command, stream, (int)PayloadLength);
        }

        public override void Serialize(Stream stream)
        {
            Write(stream, (uint)Network);
            var commandBytes = new byte[MAX_COMMAND_LENGTH];
            Encoding.ASCII.GetBytes(Command, 0, Math.Min(Command.Length, MAX_COMMAND_LENGTH), commandBytes, 0);
            WriteBytes(stream, commandBytes);
            Write(stream, PayloadLength);
            if (Payload.IncludeChecksum)
            {
                Write(stream, Checksum);
            }
            Write(stream, Payload);
        }

        public override int SerializedByteSize
        {
            get { return BufferOperations.UINT32_SIZE + MAX_COMMAND_LENGTH + BufferOperations.UINT32_SIZE + (Payload.IncludeChecksum ? BufferOperations.UINT32_SIZE : 0) + Payload.SerializedByteSize; }
        }

        [ContractInvariantMethod]
        private void __Invariant()
        {
            Contract.Invariant(!string.IsNullOrWhiteSpace(Command));
            Contract.Invariant(PayloadLength <= uint.MaxValue - 24);
            Contract.Invariant(Payload != null);
        }
    }
}
