using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Numerics;

namespace xpdm.Bitcoin.Core
{
    public sealed class Hash256 : Hash
    {
        public Hash256() : base(new byte[HASH_LEN]) { }
        public Hash256(byte[] hash)
            : base(hash)
        {
            ContractsCommon.NotNull(hash, "hash");
            Contract.Requires<ArgumentException>(hash.Length == HASH_LEN, "hash");
        }
        public Hash256(BigInteger bi) : base(bi, HASH_LEN) { }
        public Hash256(Stream stream) : base(stream) { }
        public Hash256(byte[] buffer, int offset) : base(buffer, offset) { }

        private const int HASH_LEN = 32;
        public override int HashByteSize
        {
            get { return HASH_LEN; }
        }

        public static Hash256 Parse(string hashString)
        {
            ContractsCommon.NotNull(hashString, "hashString");
            Contract.Requires<FormatException>(hashString.Trim().Length == HASH_LEN * 2, "Hash string not of expected length.");
            ContractsCommon.ResultIsNonNull<Hash256>();

            var bytes = BufferOperations.FromByteString(hashString.Trim(), Endianness.BigEndian);
            return new Hash256(bytes);
        }

        public static bool TryParse(string hashString, out Hash256 hash)
        {
            Contract.Ensures(Contract.Result<bool>() == true || Contract.ValueAtReturn(out hash) == default(Hash256));
            Contract.Ensures(Contract.Result<bool>() == false || Contract.ValueAtReturn(out hash) != null);

            try
            {
                hash = Parse(hashString);
                return true;
            }
            catch (ArgumentException) { }
            catch (FormatException) { }
            catch (OverflowException) { }

            hash = default(Hash256);
            return false;
        }

        public static readonly Hash256 Empty = new Hash256();
    }
}
