using System;
using System.Diagnostics.Contracts;
using System.Text;
using xpdm.Bitcoin.Messaging.Payloads;

namespace xpdm.Bitcoin.Messaging
{
    public class Message : SerializableMessageBase
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

        public Message(byte[] buffer, int offset)
            : base(buffer, offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Length >= Message.MinimumByteSize, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - Message.MinimumByteSize, "offset");

            Network = (Network)buffer.ReadUInt32(offset);
            Command = Encoding.ASCII.GetString(buffer, Command_Offset(ref offset), MAX_COMMAND_LENGTH).TrimEnd('\0');
            PayloadLength = buffer.ReadUInt32(PayloadLength_Offset(ref offset));

            ByteSize = ((uint)Network).ByteSize() + MAX_COMMAND_LENGTH * BufferOperations.UINT8_SIZE + PayloadLength.ByteSize();

            if (PayloadFactory.PayloadRequiresChecksum(Command))
            {
                Checksum = buffer.ReadUInt32(Checksum_Offset(ref offset));

                ByteSize += Checksum.ByteSize();
            }

            Payload = PayloadFactory.ConstructPayload(Command, buffer, Payload_Offset(ref offset), PayloadLength);

            ByteSize += Payload.ByteSize;
        }

        private int Command_Offset(ref int offset) { return offset += (int)((uint)Network).ByteSize(); }
        private int PayloadLength_Offset(ref int offset) { return offset += BufferOperations.UINT8_SIZE * MAX_COMMAND_LENGTH; }
        private int Checksum_Offset(ref int offset) { return offset += (int)PayloadLength.ByteSize(); }
        private int Payload_Offset(ref int offset) { return offset += (int)(PayloadFactory.PayloadRequiresChecksum(Command) ? Checksum.ByteSize() : 0); }

        [Pure]
        public override void WriteToBitcoinBuffer(byte[] buffer, int offset)
        {
            ((uint)Network).WriteBytes(buffer, offset);
            Encoding.ASCII.GetBytes(Command, 0, Command.Length < MAX_COMMAND_LENGTH ? Command.Length : MAX_COMMAND_LENGTH, buffer, Command_Offset(ref offset));
            PayloadLength.WriteBytes(buffer, PayloadLength_Offset(ref offset));
            if (Payload.IncludeChecksum)
            {
                Checksum.WriteBytes(buffer, Checksum_Offset(ref offset));
            }
            Payload.WriteToBitcoinBuffer(buffer, Payload_Offset(ref offset));
        }

        public static int MinimumByteSize
        {
            get
            {
                return BufferOperations.UINT32_SIZE * 2 + BufferOperations.UINT8_SIZE * MAX_COMMAND_LENGTH;
            }
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
