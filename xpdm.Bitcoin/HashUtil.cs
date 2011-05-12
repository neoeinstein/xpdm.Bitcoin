using System;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xpdm.Bitcoin
{
    public static class HashUtil
    {
        public static readonly int Hash256ByteLength = 256 / 8;
        public static readonly int Hash160ByteLength = 160 / 8;

        public static byte[] Hash256(byte[] buffer)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Ensures(Contract.Result<byte[]>() != null);
            Contract.Ensures(Contract.Result<byte[]>().Length == Hash256ByteLength);

            return Hash256(buffer, 0, buffer.Length);
        }

        public static byte[] Hash256(byte[] buffer, int offset, int length)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(0 <= offset && offset <= buffer.Length, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(0 <= length, "length");
            Contract.Requires<ArgumentOutOfRangeException>(offset + length <= buffer.Length, "length");
            Contract.Ensures(Contract.Result<byte[]>() != null);
            Contract.Ensures(Contract.Result<byte[]>().Length == Hash256ByteLength);

            using (var sha = SHA256.Create())
            {
                var hashBuffer = sha.ComputeHash(buffer, offset, length);
                hashBuffer = sha.ComputeHash(hashBuffer);
                return hashBuffer;
            }
        }

        public static byte[] Hash160(byte[] buffer)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Ensures(Contract.Result<byte[]>() != null);
            Contract.Ensures(Contract.Result<byte[]>().Length == Hash160ByteLength);

            return Hash160(buffer, 0, buffer.Length);
        }

        public static byte[] Hash160(byte[] buffer, int offset, int length)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(0 <= offset && offset <= buffer.Length, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(0 <= length, "length");
            Contract.Requires<ArgumentOutOfRangeException>(offset + length <= buffer.Length, "length");
            Contract.Ensures(Contract.Result<byte[]>() != null);
            Contract.Ensures(Contract.Result<byte[]>().Length == Hash160ByteLength);

            using (var sha = SHA256.Create())
            {
                var hashBuffer = sha.ComputeHash(buffer, offset, length);
                using (var ripemd = RIPEMD160.Create())
                {
                    hashBuffer = ripemd.ComputeHash(hashBuffer);
                    return hashBuffer;
                }
            }
        }
    }
}
