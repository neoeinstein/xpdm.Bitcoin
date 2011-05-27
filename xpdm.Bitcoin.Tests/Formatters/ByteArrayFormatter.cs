using MbUnit.Framework;

namespace xpdm.Bitcoin.Tests.Formatters
{
    public static class ByteArrayFormatter
    {
        [Formatter]
        public static string FormatByteArray(byte[] array)
        {
            var byteStr = BufferOperations.ToByteString(array, Endianness.BigEndian);
            if (byteStr.Length > 30)
            {
                byteStr = byteStr.Substring(0, 28) + "...";
            }
            return byteStr;
        }
    }
}
