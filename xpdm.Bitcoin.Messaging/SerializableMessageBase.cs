using System;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin.Messaging
{
    public abstract class SerializableMessageBase : ISerializableMessage
    {
        public uint ByteSize { get; protected set; }

        protected SerializableMessageBase() { }

        protected SerializableMessageBase(byte[] buffer, int offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length, "offset");
        }

        [Pure]
        public byte[] ToBytes()
        {
            Contract.Ensures(Contract.Result<byte[]>() != null);
            Contract.Ensures(Contract.Result<byte[]>().Length == this.ByteSize);

            var bytes = new byte[this.ByteSize];

            this.WriteToBitcoinBuffer(bytes, 0);

            return bytes;
        }

        [Pure]
        public abstract void WriteToBitcoinBuffer(byte[] buffer, int offset);

        public static T ReadFromBitcoinBuffer<T>(byte[] buffer, int offset)
        {
            return (T)typeof(T).GetConstructor(new[] { typeof(byte[]), typeof(int) }).Invoke(new object[] { buffer, offset });
        }
    }
}
