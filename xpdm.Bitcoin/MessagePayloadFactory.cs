using System;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin
{
    public static class MessagePayloadFactory
    {
        public static MessagePayloadBase ConstructMessagePayload(string command, byte[] buffer, int offset)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(command), "command");
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");

            command = command.TrimEnd('\0');

            if (command.Equals("version", StringComparison.Ordinal))
            {
                return new VersionPayload(buffer, offset);
            }
            if (command.Equals("verack", StringComparison.Ordinal))
            {
                return new VerAckPayload(buffer, offset);
            }

            return null;
        }

        public static MessagePayloadBase ConstructMessagePayload(string command, byte[] buffer, int offset, uint length)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(command), "command");
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Length >= length, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - length, "offset");
            Contract.Ensures(Contract.Result<MessagePayloadBase>() != null);

            var payload = ConstructMessagePayload(command, buffer, offset);

            if (payload == null)
            {
                return new UnknownPayload(command, buffer, offset, length);
            }
            return payload;
        }

        public static bool PayloadRequiresChecksum(string command)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(command), "command");

            command = command.TrimEnd('\0');

            if (command.Equals("version", StringComparison.Ordinal)
                || command.Equals("verack", StringComparison.Ordinal))
            {
                return false;
            }

            return true;
        }
    }
}
