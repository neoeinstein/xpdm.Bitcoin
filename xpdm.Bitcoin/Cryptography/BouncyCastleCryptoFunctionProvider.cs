using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Cryptography
{
    public sealed class BouncyCastleCryptoFunctionProvider : CryptoFunctionProviderBase<IDigest>
    {
        public override Hash256 Hash256(byte[] buffer, int offset, int length)
        {
            return DoubleHash256<Sha256Digest, Sha256Digest>(buffer, offset, length);
        }

        public override Hash256 Hash256(byte[] bufferA, int offsetA, int lengthA, byte[] bufferB, int offsetB, int lengthB)
        {
            return DoubleHash256<Sha256Digest, Sha256Digest>(bufferA, offsetA, lengthA, bufferB, offsetB, lengthB);
        }

        public override Hash160 Hash160(byte[] buffer, int offset, int length)
        {
            return DoubleHash160<Sha256Digest, RipeMD160Digest>(buffer, offset, length);
        }

        public override Hash160 Ripemd160(byte[] buffer, int offset, int length)
        {
            return SingleHash160<RipeMD160Digest>(buffer, offset, length);
        }

        public override Hash160 Sha1(byte[] buffer, int offset, int length)
        {
            return SingleHash160<Sha1Digest>(buffer, offset, length);
        }

        public override Hash256 Sha256(byte[] buffer, int offset, int length)
        {
            return SingleHash256<Sha256Digest>(buffer, offset, length);
        }

        protected override byte[] SingleHash<D>(byte[] buffer, int offset, int length)
        {
            // Visual Studio will complain about this, but it works!
            var digest = new D();
            byte[] resBuf = new byte[digest.GetDigestSize()];

            digest.BlockUpdate(buffer, offset, length);
            digest.DoFinal(resBuf, 0);

            return resBuf;
        }

        protected override byte[] SingleHash<D>(byte[] bufferA, int offsetA, int lengthA, byte[] bufferB, int offsetB, int lengthB)
        {
            // Visual Studio will complain about this, but it works!
            var digest = new D();
            byte[] resBuf = new byte[digest.GetDigestSize()];

            digest.BlockUpdate(bufferA, offsetA, lengthA);
            digest.BlockUpdate(bufferB, offsetB, lengthB);
            digest.DoFinal(resBuf, 0);

            return resBuf;
        }

        protected override byte[] SingleHash<D>(byte[] bufferA, int offsetA, int lengthA, byte[] bufferB, int offsetB, int lengthB, byte[] bufferC, int offsetC, int lengthC)
        {
            // Visual Studio will complain about this, but it works!
            var digest = new D();
            byte[] resBuf = new byte[digest.GetDigestSize()];

            digest.BlockUpdate(bufferA, offsetA, lengthA);
            digest.BlockUpdate(bufferB, offsetB, lengthB);
            digest.BlockUpdate(bufferC, offsetC, lengthC);
            digest.DoFinal(resBuf, 0);

            return resBuf;
        }
    }
}
