using System;
using System.Diagnostics.Contracts;
using System.Numerics;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin
{
    public static class HashUtil
    {
        public static readonly int Hash256ByteLength = 256 / 8;
        public static readonly int Hash160ByteLength = 160 / 8;

        public static BigInteger Hash256(BigInteger bi)
        {
            return ExecuteHashFunction(Hash256, bi);
        }

        public static Hash256 Hash256(byte[] buffer)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Ensures(Contract.Result<Hash256>() != null);

            return Hash256(buffer, 0, buffer.Length);
        }

        public static Hash256 Hash256(byte[] buffer, int offset, int length)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(0 <= offset && offset <= buffer.Length, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(0 <= length, "length");
            Contract.Requires<ArgumentOutOfRangeException>(offset + length <= buffer.Length, "length");
            Contract.Ensures(Contract.Result<Hash256>() != null);

            using (var sha = SHA256.Create())
            {
                var hashBuffer = sha.ComputeHash(buffer, offset, length);
                hashBuffer = sha.ComputeHash(hashBuffer);
                return new Hash256(hashBuffer);
            }
        }

        public static Hash256 Hash256(byte[] bufferA, byte[] bufferB)
        {
            Contract.Requires<ArgumentNullException>(bufferA != null, "bufferA");
            Contract.Requires<ArgumentNullException>(bufferB != null, "bufferB");
            Contract.Ensures(Contract.Result<Hash256>() != null);

            return Hash256(bufferA, 0, bufferA.Length, bufferB, 0, bufferB.Length);
        }

        public static Hash256 Hash256(byte[] bufferA, int offsetA, int lengthA, byte[] bufferB, int offsetB, int lengthB)
        {
            Contract.Requires<ArgumentNullException>(bufferA != null, "bufferA");
            Contract.Requires<ArgumentOutOfRangeException>(0 <= offsetA && offsetA <= bufferA.Length, "offsetA");
            Contract.Requires<ArgumentOutOfRangeException>(0 <= lengthA, "lengthA");
            Contract.Requires<ArgumentOutOfRangeException>(offsetA + lengthA <= bufferA.Length, "lengthA");
            Contract.Requires<ArgumentNullException>(bufferB != null, "bufferB");
            Contract.Requires<ArgumentOutOfRangeException>(0 <= offsetB && offsetB <= bufferB.Length, "offsetB");
            Contract.Requires<ArgumentOutOfRangeException>(0 <= lengthB, "lengthB");
            Contract.Requires<ArgumentOutOfRangeException>(offsetB + lengthB <= bufferB.Length, "lengthB");
            Contract.Ensures(Contract.Result<Hash256>() != null);

            using (var sha = SHA256.Create())
            {
                sha.TransformBlock(bufferA, offsetA, lengthA, bufferA, offsetA);
                var hashBuffer = sha.TransformFinalBlock(bufferB, offsetB, lengthB);
                hashBuffer = sha.ComputeHash(hashBuffer);
                return new Hash256(hashBuffer);
            }
        }

        public static BigInteger Hash160(BigInteger bi)
        {
            return ExecuteHashFunction(Hash160, bi);
        }

        public static Hash160 Hash160(byte[] buffer)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Ensures(Contract.Result<Hash160>() != null);

            return Hash160(buffer, 0, buffer.Length);
        }

        public static Hash160 Hash160(byte[] buffer, int offset, int length)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(0 <= offset && offset <= buffer.Length, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(0 <= length, "length");
            Contract.Requires<ArgumentOutOfRangeException>(offset + length <= buffer.Length, "length");
            Contract.Ensures(Contract.Result<Hash160>() != null);

            return Ripemd160(Sha256(buffer, offset, length).Bytes);
        }

        public static BigInteger Sha1(BigInteger bi)
        {
            return ExecuteHashFunction(Sha1, bi);
        }

        public static Hash160 Sha1(byte[] buffer)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Ensures(Contract.Result<Hash160>() != null);

            return Sha1(buffer, 0, buffer.Length);
        }

        public static Hash160 Sha1(byte[] buffer, int offset, int length)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(0 <= offset && offset <= buffer.Length, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(0 <= length, "length");
            Contract.Requires<ArgumentOutOfRangeException>(offset + length <= buffer.Length, "length");
            Contract.Ensures(Contract.Result<Hash160>() != null);

            using (var sha = SHA1.Create())
            {
                return new Hash160(sha.ComputeHash(buffer, offset, length));
            }
        }

        public static BigInteger Ripemd160(BigInteger bi)
        {
            return ExecuteHashFunction(Ripemd160, bi);
        }

        public static Hash160 Ripemd160(byte[] buffer)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Ensures(Contract.Result<Hash160>() != null);

            return Ripemd160(buffer, 0, buffer.Length);
        }

        public static Hash160 Ripemd160(byte[] buffer, int offset, int length)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(0 <= offset && offset <= buffer.Length, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(0 <= length, "length");
            Contract.Requires<ArgumentOutOfRangeException>(offset + length <= buffer.Length, "length");
            Contract.Ensures(Contract.Result<Hash160>() != null);

            using (var ripemd = RIPEMD160.Create())
            {
                return new Hash160(ripemd.ComputeHash(buffer, offset, length));
            }
        }

        public static BigInteger Sha256(BigInteger bi)
        {
            return ExecuteHashFunction(Sha256, bi);
        }

        public static Hash256 Sha256(byte[] buffer)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Ensures(Contract.Result<Hash256>() != null);

            return Sha256(buffer, 0, buffer.Length);
        }

        public static Hash256 Sha256(byte[] buffer, int offset, int length)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(0 <= offset && offset <= buffer.Length, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(0 <= length, "length");
            Contract.Requires<ArgumentOutOfRangeException>(offset + length <= buffer.Length, "length");
            Contract.Ensures(Contract.Result<Hash256>() != null);

            using (var sha = SHA256.Create())
            {
                return new Hash256(sha.ComputeHash(buffer, offset, length));
            }
        }

        private delegate Hash HashFunction(byte[] buffer);

        private static BigInteger ExecuteHashFunction(HashFunction hashFunc, BigInteger bi)
        {
            Contract.Requires(hashFunc != null);

            var intermediateArray = bi.ToByteArray();
            var hash = hashFunc(intermediateArray);
            return new BigInteger(hash.Bytes);
        }
    }
}
