using System;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin
{
    public abstract class BitcoinSerializableBase : IBitcoinSerializable
    {
        public abstract uint ByteSize { get; }

        protected BitcoinSerializableBase() { }

        protected BitcoinSerializableBase(byte[] buffer, int offset)
        {
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
        public abstract void WriteToBitcoinBuffer(byte[] bytes, int offset);
    }
}
