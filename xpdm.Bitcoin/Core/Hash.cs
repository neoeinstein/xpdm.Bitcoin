﻿using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Numerics;

namespace xpdm.Bitcoin.Core
{
    public abstract class Hash : BitcoinObject, IEquatable<Hash>, IEquatable<BitcoinObject>, IComparable<Hash>, IComparable, IFormattable
    {
        [ContractPublicPropertyName("Bytes")]
        private byte[] _bytes;
        public byte[] Bytes
        {
            get
            {
                ContractsCommon.ResultIsNonNull<byte[]>();

                return (byte[])_bytes.Clone();
            }
        }

        public byte this[int index]
        {
            get
            {
                ContractsCommon.ValidIndex(0, _bytes.Length, index);

                return _bytes[index];
            }
        }

        protected Hash(byte[] hash)
        {
            ContractsCommon.NotNull(hash, "hash");
            Contract.Assert(hash.Length == HashByteSize);

            _bytes = (byte[])hash.Clone();
        }
        protected Hash(BigInteger bi, int hashSize)
        {
            Contract.Requires<ArgumentOutOfRangeException>(-BigInteger.Pow(2, hashSize * 8 - 1) <= bi && bi <= BigInteger.Pow(2, hashSize * 8 - 1) - 1, "bi");

            var bytes = bi.ToByteArray();
            if (bytes.Length < hashSize
                || (bytes.Length == hashSize + 1 && bytes[hashSize] == 0))
            {
                Array.Resize(ref bytes, hashSize);
            }
            _bytes = bytes;
        }

        protected Hash() { }
        protected Hash(Stream stream) : base(stream) { }
        protected Hash(byte[] buffer, int offset) : base(buffer, offset) { }

        protected sealed override void Deserialize(Stream stream)
        {
            var bytes = new byte[SerializedByteSize];
            stream.Read(bytes, 0, SerializedByteSize);
            _bytes = bytes;
        }

        public sealed override void Serialize(Stream stream)
        {
            stream.Write(_bytes, 0, _bytes.Length);
        }

        public sealed override int SerializedByteSize
        {
            get { return HashByteSize; }
        }

        public abstract int HashByteSize { get; }

        #region System.Object overrides

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Hash))
                return false;
            return this.Equals((Hash)obj);
        }

        public override int GetHashCode()
        {
            int code = 0;
            if (_bytes.Length >= 4)
            {
                code &= _bytes[3] << 24;
            }
            if (_bytes.Length >= 3)
            {
                code &= _bytes[2] << 16;
            }
            if (_bytes.Length >= 2)
            {
                code &= _bytes[1] << 8;
            }
            if (_bytes.Length >= 1)
            {
                code &= _bytes[0];
            }
            return code;
        }

        public override string ToString()
        {
            Contract.Ensures(Contract.Result<string>() != null);

            return _bytes.ToByteString();
        }

        public string ToString(string format)
        {
            Contract.Ensures(Contract.Result<string>() != null);

            return this.ToString(format, null);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            Contract.Ensures(Contract.Result<string>() != null);

            format = format ?? "";

            var byteString = _bytes.ToByteString();
            if (format.StartsWith("S", StringComparison.InvariantCultureIgnoreCase))
            {
                int length = 6;
                if (format.Length > 1)
                {
                    if (!int.TryParse(format.Substring(1), out length))
                    {
                        length = 6;
                    }
                }
                return byteString.Substring(0, length);
            }
            return byteString;
        }

        #endregion

        #region IEquatable<> Members

        [Pure]
        public sealed override bool Equals(Hash other)
        {
            return other != null && other._bytes != null && _bytes.Length == other._bytes.Length && _bytes.SequenceEqual(other._bytes);
        }

        [Pure]
        public sealed override bool Equals(BitcoinObject other)
        {
            return other.Equals(this);
        }

        #endregion

        public static explicit operator BigInteger(Hash hash)
        {
            ContractsCommon.NotNull(hash, "hash");

            return new BigInteger(hash._bytes);
        }

        [Pure]
        public int CompareTo(Hash other)
        {
            if (other == null)
                return 1;
            int compare = 0;
            int i = Math.Max(HashByteSize, other.HashByteSize) - 1;
            while (compare == 0 && i >= 0)
            {
                compare = (i < HashByteSize ? this[i] : 0).CompareTo(i < other.HashByteSize ? other[i] : 0);
                --i;
            }
            return compare;
        }

        [Pure]
        public int CompareTo(object obj)
        {
            return (obj is Hash ? CompareTo((Hash)obj) : 1);
        }

        [ContractInvariantMethod]
        private void __Invariant()
        {
            Contract.Invariant(_bytes != null);
        }
    }
}
