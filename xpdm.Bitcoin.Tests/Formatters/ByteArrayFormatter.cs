using MbUnit.Framework;

namespace xpdm.Bitcoin.Tests.Formatters
{
    public static class ByteArrayFormatter
    {
        [Formatter]
        public static string FormatByteArray(byte[] array)
        {
            return BufferOperations.ToByteString(array, Endianness.BigEndian);
        }
    }
}
