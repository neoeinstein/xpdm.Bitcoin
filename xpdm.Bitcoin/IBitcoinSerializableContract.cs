using System;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin
{
    [ContractClassFor(typeof(IBitcoinSerializable))]
    abstract class IBitcoinSerializableContract : IBitcoinSerializable
    {
        public uint ByteSize
        {
            get
            {
                return default(uint);
            }
        }

        public void WriteToBitcoinBuffer(byte[] buffer, int offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null);
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(buffer.Length >= offset + this.ByteSize);
        }
    }
}
