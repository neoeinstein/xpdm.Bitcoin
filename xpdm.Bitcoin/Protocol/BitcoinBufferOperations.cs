using System;
using System.Diagnostics.Contracts;
using xpdm.Bitcoin;

namespace xpdm.Bitcoin.Protocol
{
    internal static class BitcoinBufferOperations
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

        public static T[] ReadArray<T>(this byte[] buffer, int offset, ulong count) where T : BitcoinSerializableBase
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Length >= VersionPayload.MinimumByteSize, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - VersionPayload.MinimumByteSize, "offset");

            var retVal = new T[count];
            for (ulong i = 0; i < count; ++i)
            {
                retVal[i] = BitcoinSerializableBase.ReadFromBitcoinBuffer<T>(buffer, offset);
                offset += (int)retVal[i].ByteSize;
            }
            return retVal;
        }

        public static void WriteBytes<T>(T[] val, byte[] buffer, int offset, ulong count) where T : BitcoinSerializableBase
        {
            Contract.Requires<ArgumentNullException>(val != null);
            Contract.Requires<ArgumentNullException>(buffer != null);
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(val.Length == 0 || (ulong)buffer.LongLength >= (uint)offset + count * val[0].ByteSize);

            for (ulong i = 0; i < count; ++i)
            {
                val[i].WriteToBitcoinBuffer(buffer, offset);
                offset += (int) val[i].ByteSize;
            }
        }
    }
}
