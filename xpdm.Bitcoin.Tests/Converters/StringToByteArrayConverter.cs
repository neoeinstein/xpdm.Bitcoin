using MbUnit.Framework;

namespace xpdm.Bitcoin.Tests.Converters
{
    public static class StringToByteArrayConverter
    {
        [Converter]
        public static byte[] ConvertStringToByteArray(string s)
        {
            if (s.StartsWith("0x"))
            {
                return BufferOperations.FromByteString(s.Substring(2), Endianness.BigEndian);
            }
            else
            {
                return BufferOperations.FromByteString(s, Endianness.LittleEndian);
            }
        }
    }
}
