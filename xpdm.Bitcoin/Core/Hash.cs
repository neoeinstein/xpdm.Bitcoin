using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;

namespace xpdm.Bitcoin.Core
{
    public abstract class Hash : BitcoinObject, IEquatable<Hash>
    {
        private byte[] _bytes;
        public byte[] Bytes
        {
            get
            {
                Contract.Ensures(Contract.Result<byte[]>() != null);

                return (byte[])_bytes.Clone();
            }
        }

        protected Hash(byte[] hash)
        {
            Contract.Requires<ArgumentNullException>(hash != null, "hash");
            Contract.Assert(hash.Length == HashByteSize);

            _bytes = (byte[]) hash.Clone();
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

        public string ToString(string byteFormat)
        {
            Contract.Ensures(Contract.Result<string>() != null);

            return _bytes.ToByteString(byteFormat);
        }

        public string ToString(string byteFormat, string byteJoin)
        {
            Contract.Ensures(Contract.Result<string>() != null);

            return _bytes.ToByteString(byteFormat, byteJoin);
        }

        #endregion

        #region IEquatable<Hash> Members

        public bool Equals(Hash other)
        {
            return _bytes.Length == other._bytes.Length && _bytes.SequenceEqual(other._bytes);
        }

        #endregion

        #region Equality Operators
        public static bool operator ==(Hash h1, Hash h2)
        {
            return h1.Equals(h2);
        }

        public static bool operator !=(Hash h1, Hash h2)
        {
            return h1.Equals(h2);
        }
        #endregion
    }
}
