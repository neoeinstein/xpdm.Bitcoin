using System;
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
    }
}
