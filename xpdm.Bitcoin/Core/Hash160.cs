﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;

namespace xpdm.Bitcoin.Core
{
    public sealed class Hash160 : Hash
    {
        public Hash160() { }
        public Hash160(byte[] hash) : base(hash)
        {
            Contract.Requires<ArgumentException>(hash.Length == HASH_LEN, "hash");
        }
        public Hash160(Stream stream) : base(stream) { }
        public Hash160(byte[] buffer, int offset) : base(buffer, offset) { }

        private const int HASH_LEN = 20;
        public override int HashByteSize
        {
            get { return HASH_LEN; }
        }

        public static Hash160 Parse(string hashString)
        {
            Contract.Requires<ArgumentNullException>(hashString != null, "hashString");
            Contract.Requires<FormatException>(hashString.Trim().Length == HASH_LEN * 2, "Hash string not of expected length.");
            Contract.Ensures(Contract.Result<Hash160>() != null);

            var bytes = BufferOperations.FromByteString(hashString.Trim(), Endianness.LittleEndian);
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
    }
}