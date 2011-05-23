using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Cryptography
{
    public abstract class CryptoFunctionProviderBase<T> : ICryptoFunctionProvider
    {
        public static readonly int Hash256ByteLength = 256 / 8;
        public static readonly int Hash160ByteLength = 160 / 8;

        public Hash256 Hash256(byte[] buffer)
        {
            return Hash256(buffer, 0, buffer.Length);
        }
        public abstract Hash256 Hash256(byte[] buffer, int offset, int length);
        public Hash256 Hash256(byte[] bufferA, byte[] bufferB)
        {
            return Hash256(bufferA, 0, bufferA.Length, bufferB, 0, bufferB.Length);
        }
        public abstract Hash256 Hash256(byte[] bufferA, int offsetA, int lengthA, byte[] bufferB, int offsetB, int lengthB);
        public Hash160 Hash160(byte[] buffer)
        {
            return Hash160(buffer, 0, buffer.Length);
        }
        public abstract Hash160 Hash160(byte[] buffer, int offset, int length);
        public Hash160 Ripemd160(byte[] buffer)
        {
            return Ripemd160(buffer, 0, buffer.Length);
        }
        public abstract Hash160 Ripemd160(byte[] buffer, int offset, int length);
        public Hash160 Sha1(byte[] buffer)
        {
            return Sha1(buffer, 0, buffer.Length);
        }
        public abstract Hash160 Sha1(byte[] buffer, int offset, int length);
        public Hash256 Sha256(byte[] buffer)
        {
            return Sha256(buffer, 0, buffer.Length);
        }
        public abstract Hash256 Sha256(byte[] buffer, int offset, int length);

        protected Hash256 DoubleHash256<D1, D2>(byte[] buffer, int offset, int length)
            where D1 : T, new()
            where D2 : T, new()
        {
            return new Hash256(DoubleHash<D1, D2>(buffer, offset, length));
        }

        protected Hash256 DoubleHash256<D1, D2>(byte[] bufferA, int offsetA, int lengthA, byte[] bufferB, int offsetB, int lengthB)
            where D1 : T, new()
            where D2 : T, new()
        {
            return new Hash256(DoubleHash<D1, D2>(bufferA, offsetA, lengthA, bufferB, offsetB, lengthB));
        }

        protected Hash160 DoubleHash160<D1, D2>(byte[] buffer, int offset, int length)
            where D1 : T, new()
            where D2 : T, new()
        {
            return new Hash160(DoubleHash<D1, D2>(buffer, offset, length));
        }

        protected Hash256 SingleHash256<D>(byte[] buffer, int offset, int length)
            where D : T, new()
        {
            return new Hash256(SingleHash<D>(buffer, offset, length));
        }

        protected Hash160 SingleHash160<D>(byte[] buffer, int offset, int length)
            where D : T, new()
        {
            return new Hash160(SingleHash<D>(buffer, offset, length));
        }

        protected abstract byte[] SingleHash<D>(byte[] buffer, int offset, int length)
            where D : T, new();

        protected abstract byte[] SingleHash<D>(byte[] bufferA, int offsetA, int lengthA, byte[] bufferB, int offsetB, int lengthB)
            where D : T, new();

        protected abstract byte[] SingleHash<D>(byte[] bufferA, int offsetA, int lengthA, byte[] bufferB, int offsetB, int lengthB, byte[] bufferC, int offsetC, int lengthC)
            where D : T, new();

        protected byte[] DoubleHash<D1, D2>(byte[] buffer, int offset, int length)
            where D1 : T, new()
            where D2 : T, new()
        {
            byte[] intBuf = SingleHash<D1>(buffer, offset, length);
            byte[] resBuf = SingleHash<D2>(intBuf, 0, intBuf.Length);
            return resBuf;
        }

        protected byte[] DoubleHash<D1, D2>(byte[] bufferA, int offsetA, int lengthA, byte[] bufferB, int offsetB, int lengthB)
            where D1 : T, new()
            where D2 : T, new()
        {
            byte[] intBuf = SingleHash<D1>(bufferA, offsetA, lengthA, bufferB, offsetB, lengthB);
            byte[] resBuf = SingleHash<D2>(intBuf, 0, intBuf.Length);
            return resBuf;
        }

        protected byte[] DoubleHash<D1, D2>(byte[] bufferA, int offsetA, int lengthA, byte[] bufferB, int offsetB, int lengthB, byte[] bufferC, int offsetC, int lengthC)
            where D1 : T, new()
            where D2 : T, new()
        {
            byte[] intBuf = SingleHash<D1>(bufferA, offsetA, lengthA, bufferB, offsetB, lengthB, bufferC, offsetC, lengthC);
            byte[] resBuf = SingleHash<D2>(intBuf, 0, intBuf.Length);
            return resBuf;
        }
    }
}
