using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Numerics;

namespace xpdm.Bitcoin
{
    internal static class BufferOperations
    {
        public static void WriteBytes(this ulong val, byte[] buffer, int offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null);
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(buffer.Length >= offset + UINT64_SIZE);

            buffer[offset] = (byte)val;
            buffer[offset + 1] = (byte)(val >> 8);
            buffer[offset + 2] = (byte)(val >> 16);
            buffer[offset + 3] = (byte)(val >> 24);
            buffer[offset + 4] = (byte)(val >> 32);
            buffer[offset + 5] = (byte)(val >> 40);
            buffer[offset + 6] = (byte)(val >> 48);
            buffer[offset + 7] = (byte)(val >> 56);
        }

        public static void WriteBytesBE(this ulong val, byte[] buffer, int offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null);
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(buffer.Length >= offset + UINT64_SIZE);

            buffer[offset] = (byte)(val >> 56);
            buffer[offset + 1] = (byte)(val >> 48);
            buffer[offset + 2] = (byte)(val >> 40);
            buffer[offset + 3] = (byte)(val >> 32);
            buffer[offset + 4] = (byte)(val >> 24);
            buffer[offset + 5] = (byte)(val >> 16);
            buffer[offset + 6] = (byte)(val >> 8);
            buffer[offset + 7] = (byte)val;
        }

        public static ulong ReadUInt64(this byte[] buffer, int offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null);
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(buffer.Length >= offset + UINT64_SIZE);

            ulong val = buffer[offset];
            val |= ((ulong)buffer[offset + 1]) << 8;
            val |= ((ulong)buffer[offset + 2]) << 16;
            val |= ((ulong)buffer[offset + 3]) << 24;
            val |= ((ulong)buffer[offset + 4]) << 32;
            val |= ((ulong)buffer[offset + 5]) << 40;
            val |= ((ulong)buffer[offset + 6]) << 48;
            val |= ((ulong)buffer[offset + 7]) << 56;

            return val;
        }

        public static ulong ReadUInt64BE(this byte[] buffer, int offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null);
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(buffer.Length >= offset + UINT64_SIZE);

            ulong val = ((ulong)buffer[offset]) << 56;
            val |= ((ulong)buffer[offset + 1]) << 48;
            val |= ((ulong)buffer[offset + 2]) << 40;
            val |= ((ulong)buffer[offset + 3]) << 32;
            val |= ((ulong)buffer[offset + 4]) << 24;
            val |= ((ulong)buffer[offset + 5]) << 16;
            val |= ((ulong)buffer[offset + 6]) << 8;
            val |= buffer[offset + 7];

            return val;
        }

        public static void WriteBytes(this uint val, byte[] buffer, int offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null);
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(buffer.Length >= offset + UINT32_SIZE);

            buffer[offset] = (byte)val;
            buffer[offset + 1] = (byte)(val >> 8);
            buffer[offset + 2] = (byte)(val >> 16);
            buffer[offset + 3] = (byte)(val >> 24);
        }

        public static uint ReadUInt32(this byte[] buffer, int offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null);
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(buffer.Length >= offset + UINT32_SIZE);

            uint val = buffer[offset];
            val |= ((uint)buffer[offset + 1]) << 8;
            val |= ((uint)buffer[offset + 2]) << 16;
            val |= ((uint)buffer[offset + 3]) << 24;

            return val;
        }

        public static void WriteBytes(this ushort val, byte[] buffer, int offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null);
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(buffer.Length >= offset + UINT16_SIZE);

            buffer[offset] = (byte)val;
            buffer[offset + 1] = (byte)(val >> 8);
        }

        public static void WriteBytesBE(this ushort val, byte[] buffer, int offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null);
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(buffer.Length >= offset + UINT16_SIZE);

            buffer[offset] = (byte)(val >> 8);
            buffer[offset + 1] = (byte)val;
        }

        public static ushort ReadUInt16(this byte[] buffer, int offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null);
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(buffer.Length >= offset + UINT16_SIZE);

            ushort val = buffer[offset];
            val |= (ushort)(((uint)buffer[offset + 1]) << 8);

            return val;
        }

        public static ushort ReadUInt16BE(this byte[] buffer, int offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null);
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(buffer.Length >= offset + UINT16_SIZE);

            ushort val = (ushort)(((uint)buffer[offset]) << 8);
            val |= buffer[offset + 1];

            return val;
        }

        public const int UINT8_SIZE = 1;
        public static uint ByteSize(this byte dummy)
        {
            return UINT8_SIZE;
        }

        public const int UINT16_SIZE = 2;
        public static uint ByteSize(this ushort dummy)
        {
            return UINT16_SIZE;
        }

        public const int UINT32_SIZE = 4;
        public static uint ByteSize(this uint dummy)
        {
            return UINT32_SIZE;
        }

        public const int UINT64_SIZE = 8;
        public static uint ByteSize(this ulong dummy)
        {
            return UINT64_SIZE;
        }

        public static byte[] ToBytesArrayBE(this BigInteger bi)
        {
            Contract.Ensures(Contract.Result<byte[]>() != null);
            Contract.Ensures(Contract.Result<byte[]>().Length == bi.ToByteArray().Length);

            var resultArray = bi.ToByteArray();
            Array.Reverse(resultArray);
            return resultArray;
        }

        public static void WriteBytesBE(this BigInteger bi, byte[] buffer, int offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(0 <= offset && offset <= buffer.Length, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset + bi.ToByteArray().Length <= buffer.Length, "bi");

            var resultArray = bi.ToBytesArrayBE();
            Array.Copy(resultArray, 0, buffer, offset, resultArray.Length);
        }

        public static BigInteger ReadBigIntegerBE(this byte[] buffer)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");

            return buffer.ReadBigIntegerBE(0, buffer.Length);
        }

        public static BigInteger ReadBigIntegerBE(this byte[] buffer, int offset, int length)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(0 <= offset && offset <= buffer.Length, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(0 <= length, "length");
            Contract.Requires<ArgumentOutOfRangeException>(offset + length <= buffer.Length, "length");

            var intermediateBuffer = new byte[length];
            Array.Copy(buffer, offset, intermediateBuffer, 0, length);
            Array.Reverse(intermediateBuffer);
            return new BigInteger(intermediateBuffer);
        }

        public static string ToByteString(this byte[] buffer)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Ensures(Contract.Result<string>() != null);
            Contract.Ensures(Contract.Result<string>().Length == buffer.Length * 2);

            return buffer.ToByteString("x2", string.Empty);
        }

        public static string ToByteString(this byte[] buffer, string byteFormat)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Ensures(Contract.Result<string>() != null);

            return buffer.ToByteString(byteFormat, string.Empty);
        }

        public static string ToByteString(this byte[] buffer, string byteFormat, string byteJoin)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Ensures(Contract.Result<string>() != null);

            var sb = new System.Text.StringBuilder();
            int i = buffer.Length;
            for (;;)
            {
                sb.Append(buffer[--i].ToString(byteFormat));
                if (i > 0)
                {
                    sb.Append(byteJoin);
                }
                else
                {
                    return sb.ToString();
                }
            }
        }

        public static byte[] FromByteString(string byteString)
        {
            Contract.Requires<ArgumentNullException>(byteString != null, "byteString");
            Contract.Ensures(Contract.Result<byte[]>() != null);
            Contract.Ensures(Contract.Result<byte[]>().Length == (byteString.Trim().Length + 1) >> 1);

            byteString = byteString.Trim();

            var ba = new byte[(byteString.Length + 1) >> 1];
            var offset = 0;
            if (byteString.Length % 2 == 1)
            {
                ba[ba.Length - 1] = byte.Parse(byteString.Substring(0, 1), NumberStyles.AllowHexSpecifier);
                offset = 1;
            }

            for (int i = ba.Length - 1; i >= offset; --i)
            {
                ba[i] = byte.Parse(byteString.Substring(i*2 - offset, 2), NumberStyles.AllowHexSpecifier);
            }

            return ba;
        }
    }
}
