using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;

namespace xpdm.Bitcoin.Core
{
    public sealed class Hash256 : Hash
    {
        public Hash256() { }
        public Hash256(byte[] hash) : base(hash)
        {
            Contract.Requires<ArgumentException>(hash.Length == HASH_LEN, "hash");
        }
        public Hash256(Stream stream) : base(stream) { }
        public Hash256(byte[] buffer, int offset) : base(buffer, offset) { }

        private const int HASH_LEN = 32;
        public override int HashByteSize
        {
            get { return HASH_LEN; }
        }
    }
}
