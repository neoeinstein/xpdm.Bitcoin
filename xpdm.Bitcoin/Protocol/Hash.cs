using System;
using System.Diagnostics.Contracts;
using xpdm.Bitcoin;

namespace xpdm.Bitcoin.Protocol
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
                return (byte[])_bytes.Clone();
            }
        }

        public Hash(byte[] hash)
            : this(hash, 0)
        {
            ByteSize = (uint)Hash.ConstantByteSize;
        }

        public Hash(byte[] buffer, int offset)
            : base(buffer, offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Length >= Hash.ConstantByteSize, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - Hash.ConstantByteSize, "offset");

            _bytes = new byte[HASH_LENGTH];
            Array.Copy(buffer, offset, _bytes, 0, HASH_LENGTH * BufferOperations.UINT8_SIZE);

            ByteSize = (uint)Hash.ConstantByteSize;
        }

        private const int HASH_LENGTH = 32;

        [Pure]
        public override void WriteToBitcoinBuffer(byte[] buffer, int offset)
        {
            Array.Copy(_bytes, 0, buffer, offset, HASH_LENGTH * BufferOperations.UINT8_SIZE);
        }

        public static int ConstantByteSize
        {
            get { return HASH_LENGTH * BufferOperations.UINT8_SIZE; }
        }
    }
}
