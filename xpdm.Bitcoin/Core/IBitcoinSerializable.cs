using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace xpdm.Bitcoin.Core
{
    [ContractClass(typeof(IBitcoinSerializableContract))]
    public interface IBitcoinSerializable
    {
        void Serialize(Stream stream);
        int SerializedByteSize { get; }
    }

    [ContractClassFor(typeof(IBitcoinSerializable))]
    internal abstract class IBitcoinSerializableContract : IBitcoinSerializable
    {
        public void Serialize(Stream stream)
        {
            Contract.Requires<ArgumentNullException>(stream != null, "stream");
            //Contract.Requires<ArgumentOutOfRangeException>(stream.Position + SerializedByteSize <= stream.Length, "length");
        }

        public int SerializedByteSize
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);

                return default(int);
            }
        }
    }
}
