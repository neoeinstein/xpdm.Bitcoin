using System;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin
{
    public class Hash : BitcoinSerializableBase
    {
        private byte[] _bytes;
        public byte[] Bytes
        {
            get
            {
                Contract.Ensures(Contract.Result<byte[]>() != null);
                Contract.Ensures(Contract.Result<byte[]>().Length == HASH_LENGTH);

                // Use a copy of the byte array to ensure that the original array is
                // not altered, and subsequent calls will return the original hash.
                var retVal = new byte[HASH_LENGTH];
                Array.Copy(_bytes, retVal, HASH_LENGTH);
                return retVal;
            }
        }

        private const int BYTESIZE = 32;
        public override uint ByteSize
        {
            get { return BYTESIZE; }
        }

        public Hash(byte[] hash)
            : this(hash, 0)
        {
        }

        public Hash(byte[] buffer, int offset)
            : base(buffer, offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentNullException>(buffer.Length >= BYTESIZE, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - BYTESIZE, "offset");

            _bytes = new byte[HASH_LENGTH];
            Array.Copy(buffer, offset, _bytes, 0, HASH_LENGTH);
        }

        private const int HASH_LENGTH = 32;

        [Pure]
        public override void WriteToBitcoinBuffer(byte[] buffer, int offset)
        {
            Array.Copy(_bytes, 0, buffer, offset, HASH_LENGTH);
        }
    }
}
