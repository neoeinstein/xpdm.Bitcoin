using System;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin.Messaging.Payloads
{
    public static class PayloadFactory
    {
        public static PayloadBase ConstructPayload(string command, byte[] buffer, int offset)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(command), "command");
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");

            command = command.TrimEnd('\0');

            if (command.Equals(InvPayload.CommandText, StringComparison.Ordinal))
            {
                return new InvPayload(buffer, offset);
            }
            if (command.Equals(TxPayload.CommandText, StringComparison.Ordinal))
            {
                return new TxPayload(buffer, offset);
            }
            if (command.Equals(BlockPayload.CommandText, StringComparison.Ordinal))
            {
                return new BlockPayload(buffer, offset);
            }
            if (command.Equals(VersionPayload.CommandText, StringComparison.Ordinal))
            {
                return new VersionPayload(buffer, offset);
            }
            if (command.Equals(VerAckPayload.CommandText, StringComparison.Ordinal))
            {
                return new VerAckPayload(buffer, offset);
            }
            if (command.Equals(HeadersPayload.CommandText, StringComparison.Ordinal))
            {
                return new HeadersPayload(buffer, offset);
            }
            if (command.Equals(AddrPayload.CommandText, StringComparison.Ordinal))
            {
                return new AddrPayload(buffer, offset);
            }
            if (command.Equals(GetDataPayload.CommandText, StringComparison.Ordinal))
            {
                return new GetDataPayload(buffer, offset);
            }
            if (command.Equals(GetBlocksPayload.CommandText, StringComparison.Ordinal))
            {
                return new GetBlocksPayload(buffer, offset);
            }
            if (command.Equals(GetHeadersPayload.CommandText, StringComparison.Ordinal))
            {
                return new GetHeadersPayload(buffer, offset);
            }
            if (command.Equals(GetAddrPayload.CommandText, StringComparison.Ordinal))
            {
                return new GetAddrPayload(buffer, offset);
            }
            if (command.Equals(PingPayload.CommandText, StringComparison.Ordinal))
            {
                return new PingPayload(buffer, offset);
            }
            if (command.Equals(AlertPayload.CommandText, StringComparison.Ordinal))
            {
                return new AlertPayload(buffer, offset);
            }

            return null;
        }

        public static PayloadBase ConstructPayload(string command, byte[] buffer, int offset, int length)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(command), "command");
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Length >= length, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - length, "offset");
            Contract.Ensures(Contract.Result<PayloadBase>() != null);

            var payload = ConstructPayload(command, buffer, offset);

            if (payload == null)
            {
                payload = new UnknownPayload(command, buffer, offset, length);
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
