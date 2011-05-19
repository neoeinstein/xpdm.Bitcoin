using System;
using System.Diagnostics.Contracts;
using System.Numerics;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Cryptography
{
    public sealed class SystemCryptoFunctionProvider : CryptoFunctionProviderBase<HashAlgorithm>
    {
        public override Hash256 Hash256(byte[] buffer, int offset, int length)
        {
            return DoubleHash256<SHA256Cng, SHA256Cng>(buffer, offset, length);
        }

        public override Hash256 Hash256(byte[] bufferA, int offsetA, int lengthA, byte[] bufferB, int offsetB, int lengthB)
        {
            return DoubleHash256<SHA256Cng, SHA256Cng>(bufferA, offsetA, lengthA, bufferB, offsetB, lengthB);
        }

        public override Hash160 Hash160(byte[] buffer, int offset, int length)
        {
            return DoubleHash160<SHA256Cng, RIPEMD160Managed>(buffer, offset, length);
        }

        public override Hash160 Ripemd160(byte[] buffer, int offset, int length)
        {
            return SingleHash160<RIPEMD160Managed>(buffer, offset, length);
        }

        public override Hash160 Sha1(byte[] buffer, int offset, int length)
        {
            return SingleHash160<SHA1Cng>(buffer, offset, length);
        }

        public override Hash256 Sha256(byte[] buffer, int offset, int length)
        {
            return SingleHash256<SHA256Cng>(buffer, offset, length);
        }

        protected override byte[] SingleHash<D>(byte[] buffer, int offset, int length)
        {
            // Visual Studio will complain about this, but it works!
            using (var hash = new D())
            {
                hash.ComputeHash(buffer, offset, length);

                return hash.Hash;
            }
        }

        protected override byte[] SingleHash<D>(byte[] bufferA, int offsetA, int lengthA, byte[] bufferB, int offsetB, int lengthB)
        {
            // Visual Studio will complain about this, but it works!
            using (var hash = new D())
            {
                hash.TransformBlock(bufferA, offsetA, lengthA, bufferA, offsetA);
                hash.TransformBlock(bufferB, offsetB, lengthB, bufferB, offsetB);
                hash.TransformFinalBlock(new byte[0], 0, 0);

                return hash.Hash;
            }
        }

        protected override byte[] SingleHash<D>(byte[] bufferA, int offsetA, int lengthA, byte[] bufferB, int offsetB, int lengthB, byte[] bufferC, int offsetC, int lengthC)
        {
            // Visual Studio will complain about this, but it works!
            using (var hash = new D())
            {
                hash.TransformBlock(bufferA, offsetA, lengthA, bufferA, offsetA);
                hash.TransformBlock(bufferB, offsetB, lengthB, bufferB, offsetB);
                hash.TransformBlock(bufferC, offsetC, lengthC, bufferC, offsetC);
                hash.TransformFinalBlock(new byte[0], 0, 0);

                return hash.Hash;
            }
        }
    }
}
