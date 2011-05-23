using MbUnit.Framework;

namespace xpdm.Bitcoin.Tests.Converters
{
    public static class StringToByteArrayConverter
    {
        [Converter]
        public static byte[] ConvertStringToByteArray(string s)
        {
            return BufferOperations.FromByteString(s, Endianness.BigEndian);
        }
    }
}
