using System;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin.Protocol
{
    internal static class BitcoinBufferOperations
    {
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
                offset += (int)val[i].ByteSize;
            }
        }
    }
}
