using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Numerics;

namespace xpdm.Bitcoin.Core
{
    public sealed class Hash160 : Hash
    {
        public Hash160() : base(new byte[HASH_LEN]) { }
        public Hash160(byte[] hash) : base(hash)
        {
            ContractsCommon.NotNull(hash, "hash");
            Contract.Requires<ArgumentException>(hash.Length == HASH_LEN, "hash");
        }
        public Hash160(BigInteger bi) : base(bi, HASH_LEN) { }
        public Hash160(Stream stream) : base(stream) { }
        public Hash160(byte[] buffer, int offset) : base(buffer, offset) { }

        private const int HASH_LEN = 20;
        public override int HashByteSize
        {
            get { return HASH_LEN; }
        }

        public static Hash160 Parse(string hashString)
        {
            ContractsCommon.NotNull(hashString, "hashString");
            Contract.Requires<FormatException>(hashString.Trim().Length == HASH_LEN * 2, "Hash string not of expected length.");
            ContractsCommon.ResultIsNonNull<Hash160>();

            var bytes = BufferOperations.FromByteString(hashString.Trim(), Endianness.BigEndian);
            return new Hash160(bytes);
        }

        public static bool TryParse(string hashString, out Hash160 hash)
        {
            Contract.Ensures(Contract.Result<bool>() == true || Contract.ValueAtReturn(out hash) == default(Hash160));
            Contract.Ensures(Contract.Result<bool>() == false || Contract.ValueAtReturn(out hash) != null);

            try
            {
                hash = Parse(hashString);
                return true;
            }
            catch (ArgumentException) { }
            catch (FormatException) { }
            catch (OverflowException) { }

            hash = default(Hash160);
            return false;
        }

        public static readonly Hash160 Empty = new Hash160();
    }
}
