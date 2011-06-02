using System;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin.Messaging
{
    [ContractClass(typeof(Contracts.ISerializableMessageContract))]
    public interface ISerializableMessage
    {
        uint ByteSize { get; }
        [Pure]
        byte[] ToBytes();
        [Pure]
        void WriteToBitcoinBuffer(byte[] buffer, int offset);
    }

    namespace Contracts
    {
        [ContractClassFor(typeof(ISerializableMessage))]
        abstract class ISerializableMessageContract : ISerializableMessage
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
}
