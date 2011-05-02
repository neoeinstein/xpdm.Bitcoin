using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin
{
    public class Message : BitcoinSerializableBase
    {
        /// <summary>
        /// Indicates the network from which this message originated.
        /// </summary>
        public MessageNetwork Network
        {
            get;
            private set;
        }

        private const int MAX_COMMAND_LENGTH = 12;
        private const int COMMAND_OFFSET = BitcoinBufferOperations.UINT32_SIZE;
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

        private const int LENGTH_OFFSET = COMMAND_OFFSET + MAX_COMMAND_LENGTH * BitcoinBufferOperations.UINT8_SIZE;
        /// <summary>
        /// Length in bytes of the payload (not including the headers). For
        /// a calculated value, use <c ref="NormativePayloadLength"/>.
        /// </summary>
        public uint PayloadLength
        {
            get;
            private set;
        }

        private const int CHECKSUM_OFFSET = LENGTH_OFFSET + BitcoinBufferOperations.UINT32_SIZE;
        /// <summary>
        /// The first 4 bytes of SHA256(SHA256(Payload))
        /// </summary>
        public uint Checksum
        {
            get;
            private set;
        }

        public MessagePayloadBase Payload
        {
            get;
            private set;
        }

        protected int PayloadOffset
        {
            get { return CHECKSUM_OFFSET + (MessagePayloadFactory.PayloadRequiresChecksum(Command) ? (int)BitcoinBufferOperations.UINT32_SIZE : 0); }
        }

        public Message(byte[] buffer, int offset)
            : base(buffer, offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Length >= Message.MinimumByteSize, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - Message.MinimumByteSize, "offset");

            Network = (MessageNetwork)buffer.ReadUInt32(offset);
            Command = Encoding.ASCII.GetString(buffer, offset + COMMAND_OFFSET, MAX_COMMAND_LENGTH).TrimEnd('\0');
            PayloadLength = buffer.ReadUInt32(offset + LENGTH_OFFSET);
            if (MessagePayloadFactory.PayloadRequiresChecksum(Command))
            {
                Checksum = buffer.ReadUInt32(offset + CHECKSUM_OFFSET);
            }
            Payload = MessagePayloadFactory.ConstructMessagePayload(Command, buffer, offset + PayloadOffset, PayloadLength);
        }

        public override uint ByteSize
        {
            get
            {
                return PayloadLength + (uint)PayloadOffset;
            }
        }

        [Pure]
        public override void WriteToBitcoinBuffer(byte[] buffer, int offset)
        {
            ((uint)Network).WriteBytes(buffer, offset);
            Encoding.ASCII.GetBytes(Command, 0, Command.Length < MAX_COMMAND_LENGTH ? Command.Length : MAX_COMMAND_LENGTH, buffer, offset + COMMAND_OFFSET);
            PayloadLength.WriteBytes(buffer, offset + LENGTH_OFFSET);
            if (Payload.IncludeChecksum)
            {
                Checksum.WriteBytes(buffer, offset + CHECKSUM_OFFSET);
            }
            Payload.WriteToBitcoinBuffer(buffer, offset + PayloadOffset);
        }

        public static int MinimumByteSize
        {
            get
            {
                return BitcoinBufferOperations.UINT32_SIZE * 2 + BitcoinBufferOperations.UINT8_SIZE * MAX_COMMAND_LENGTH;
            }
        }

        [ContractInvariantMethod]
        private void __Invariant()
        {
            Contract.Invariant(!string.IsNullOrWhiteSpace(Command), "Command cannot be null");
            Contract.Invariant(PayloadLength <= uint.MaxValue - 24, "PayloadLength is too large");
            Contract.Invariant(Payload != null);
        }
    }
}
