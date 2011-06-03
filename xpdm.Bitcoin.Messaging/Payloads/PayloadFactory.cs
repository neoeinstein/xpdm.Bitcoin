using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace xpdm.Bitcoin.Messaging.Payloads
{
    public static class PayloadFactory
    {
        public static IPayload ConstructPayload(string command, Stream stream)
        {
            ContractsCommon.NotNull(command, "command");
            ContractsCommon.NotNull(stream, "stream");
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(command), "command");

            command = command.TrimEnd('\0');

            if (command.Equals(InvPayload.CommandText, StringComparison.Ordinal))
            {
                return new InvPayload(stream);
            }
            if (command.Equals(TxPayload.CommandText, StringComparison.Ordinal))
            {
                return new TxPayload(stream);
            }
            if (command.Equals(BlockPayload.CommandText, StringComparison.Ordinal))
            {
                return new BlockPayload(stream);
            }
            if (command.Equals(VersionPayload.CommandText, StringComparison.Ordinal))
            {
                return new VersionPayload(stream);
            }
            if (command.Equals(VerAckPayload.CommandText, StringComparison.Ordinal))
            {
                return new VerAckPayload(stream);
            }
            if (command.Equals(HeadersPayload.CommandText, StringComparison.Ordinal))
            {
                return new HeadersPayload(stream);
            }
            if (command.Equals(AddrPayload.CommandText, StringComparison.Ordinal))
            {
                return new AddrPayload(stream);
            }
            if (command.Equals(GetDataPayload.CommandText, StringComparison.Ordinal))
            {
                return new GetDataPayload(stream);
            }
            if (command.Equals(GetBlocksPayload.CommandText, StringComparison.Ordinal))
            {
                return new GetBlocksPayload(stream);
            }
            if (command.Equals(GetHeadersPayload.CommandText, StringComparison.Ordinal))
            {
                return new GetHeadersPayload(stream);
            }
            if (command.Equals(GetAddrPayload.CommandText, StringComparison.Ordinal))
            {
                return new GetAddrPayload(stream);
            }
            if (command.Equals(PingPayload.CommandText, StringComparison.Ordinal))
            {
                return new PingPayload(stream);
            }
            if (command.Equals(AlertPayload.CommandText, StringComparison.Ordinal))
            {
                return new AlertPayload(stream);
            }

            return null;
        }

        public static IPayload ConstructPayload(string command, Stream stream, int length)
        {
            ContractsCommon.NotNull(command, "command");
            ContractsCommon.NotNull(stream, "stream");
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(command), "command");
            Contract.Ensures(stream.Position == Contract.OldValue(stream.Position) + length);
            ContractsCommon.ResultIsNonNull<IPayload>();

            var payload = ConstructPayload(command, stream);

            if (payload == null)
            {
                payload = new UnknownPayload(command, stream, length);
            }

            return payload;
        }

        public static bool PayloadRequiresChecksum(string command)
        {
            ContractsCommon.NotNull(command, "command");
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
