using System;
using SCG=System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.Serialization;

namespace xpdm.Bitcoin.Core
{
    public abstract class BitcoinObject : BitcoinSerializable, IEquatable<BitcoinObject>, IEquatable<Hash>
    {
        protected BitcoinObject() { }
        protected BitcoinObject(Stream stream) : base(stream) { }
        protected BitcoinObject(byte[] buffer, int offset) : base(buffer, offset) { }

        #region Hashing

        private Hash256 _hash256;
        public Hash256 Hash256
        {
            get
            {
                ContractsCommon.ResultIsNonNull<Hash256>();

                if (_hash256 == null)
                {
                    _hash256 = CalculateBitcoinHash256();
                }
                return _hash256;
            }
        }

        private Hash256 CalculateBitcoinHash256()
        {
            ContractsCommon.ResultIsNonNull<Hash256>();

            return HashUtil.Hash256(this.BuildBitcoinHashByteArray());
        }

        private Hash160 _hash160;
        public Hash160 Hash160
        {
            get
            {
                ContractsCommon.ResultIsNonNull<Hash160>();

                if (_hash160 == null)
                {
                    _hash160 = CalculateBitcoinHash160();
                }
                return _hash160;
            }
        }
        private Hash160 CalculateBitcoinHash160()
        {
            ContractsCommon.ResultIsNonNull<Hash160>();

            return HashUtil.Hash160(this.BuildBitcoinHashByteArray());
        }

        [Pure]
        protected virtual byte[] BuildBitcoinHashByteArray()
        {
            ContractsCommon.ResultIsNonNull<byte[]>();

            return this.SerializeToByteArray();
        }

        protected void InvalidateBitcoinHashes()
        {
            InvalidateBitcoinHashes(this, EventArgs.Empty);
        }

        protected void InvalidateBitcoinHashes(object sender, EventArgs e)
        {
            _hash256 = null;
            _hash160 = null;
            if (HashesInvalidated != null)
            {
                HashesInvalidated(this, EventArgs.Empty);
            }
        }

        public event EventHandler HashesInvalidated;

        #endregion

        #region IEquatable<> Members

        [Pure]
        public virtual bool Equals(BitcoinObject other)
        {
            return other != null && this.Hash256.Equals(other.Hash256);
        }

        [Pure]
        public virtual bool Equals(Hash other)
        {
            return other != null && this.Hash256.Equals(other);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as BitcoinObject);
        }

        public override int GetHashCode()
        {
            return this.Hash256.GetHashCode();
        }

        #endregion
    }
}
