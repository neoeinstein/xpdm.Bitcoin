using System;
using System.Linq;
using System.Diagnostics.Contracts;
using xpdm.Bitcoin;

namespace xpdm.Bitcoin.Protocol
{
    public class VarArray<T> : BitcoinSerializableBase where T : BitcoinSerializableBase
    {
        public VarInt Count { get; private set; }

        private readonly T[] _values;
        public T[] Values
        {
            get
            {
                Contract.Ensures(Contract.Result<T[]>() != null);
                Contract.Ensures((uint)Contract.Result<T[]>().Length == Count);

                return (T[])_values.Clone();
            }
        }

        public VarArray(T[] values)
        {
            Contract.Requires<ArgumentNullException>(values != null, "values");

            _values = (T[])values.Clone();
            Count = new VarInt((ulong)values.LongLength);

            ByteSize = Count.ByteSize + (uint)_values.Sum(v => v.ByteSize);
        }

        public VarArray(byte[] buffer, int offset)
            : base(buffer, offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Length >= VarArray<T>.MinimumByteSize, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - VarArray<T>.MinimumByteSize, "offset");

            Count = new VarInt(buffer, offset);
            _values = buffer.ReadArray<T>(offset + (int)Count.ByteSize, Count);

            ByteSize = Count.ByteSize + (uint)_values.Sum(v => v.ByteSize);
        }

        [Pure]
        public override void WriteToBitcoinBuffer(byte[] buffer, int offset)
        {
            Count.WriteToBitcoinBuffer(buffer, offset);
            BitcoinBufferOperations.WriteBytes(_values, buffer, offset + (int)Count.ByteSize, Count);
        }

        public static int MinimumByteSize
        {
            get
            {
                return VarInt.MinimumByteSize;
            }
        }

        private static VarArray<T> s_empty = new VarArray<T>(new T[0]);
        public static VarArray<T> Empty
        {
            get
            {
                return s_empty;
            }
        }
    }
}
