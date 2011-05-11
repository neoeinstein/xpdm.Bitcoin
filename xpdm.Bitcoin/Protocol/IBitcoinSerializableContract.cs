using System;
using System.Diagnostics.Contracts;
using xpdm.Bitcoin;

namespace xpdm.Bitcoin.Protocol
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

        [Pure]
        public byte[] ToBytes()
        {
            Contract.Ensures(Contract.Result<byte[]>() != null);
            Contract.Ensures(Contract.Result<byte[]>().Length == this.ByteSize);

            return default(byte[]);
        }

        [Pure]
        public void WriteToBitcoinBuffer(byte[] buffer, int offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null);
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0);
            Contract.Requires<ArgumentOutOfRangeException>(buffer.Length >= offset + this.ByteSize);
        }
    }
}
