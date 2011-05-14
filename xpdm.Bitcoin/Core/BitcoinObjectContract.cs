using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace xpdm.Bitcoin.Core
{
    [ContractClassFor(typeof(BitcoinObject))]
    internal abstract class BitcoinObjectContract : BitcoinObject
    {
        protected sealed override void Deserialize(Stream stream)
        {
            Contract.Requires<ArgumentNullException>(stream != null, "stream");
            //Contract.Requires<ArgumentOutOfRangeException>(length <= stream.Length, "length");
            //Contract.Requires<ArgumentOutOfRangeException>(stream.Position + length <= stream.Length, "length");
        }

        public sealed override void Serialize(Stream stream)
        {
        }

        public sealed override int SerializedByteSize
        {
            get { return default(int); }
        }
    }
}
