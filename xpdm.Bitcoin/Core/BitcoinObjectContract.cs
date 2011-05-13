using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace xpdm.Bitcoin.Core
{
    [ContractClassFor(typeof(BitcoinObject))]
    internal abstract class BitcoinObjectContract : BitcoinObject
    {
        protected sealed override void Deserialize(System.IO.Stream stream)
        {
            Contract.Requires<ArgumentNullException>(stream != null, "stream");
            //Contract.Requires<ArgumentOutOfRangeException>(length <= stream.Length, "length");
            //Contract.Requires<ArgumentOutOfRangeException>(stream.Position + length <= stream.Length, "length");
        }

        public sealed override void Serialize(System.IO.Stream stream)
        {
            Contract.Requires<ArgumentNullException>(stream != null, "stream");
            //Contract.Requires<ArgumentOutOfRangeException>(stream.Position + SerializedByteSize <= stream.Length, "length");
        }

        protected sealed override int SerializedByteSize
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);

                return default(int);
            }
        }
    }
}
