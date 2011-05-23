using System.Diagnostics.Contracts;
using System.IO;

namespace xpdm.Bitcoin.Core
{
    [ContractClass(typeof(Contracts.IBitcoinSerializableContract))]
    public interface IBitcoinSerializable
    {
        [Pure]
        void Serialize(Stream stream);
        int SerializedByteSize { get; }
    }

    namespace Contracts
    {
        [ContractClassFor(typeof(IBitcoinSerializable))]
        internal abstract class IBitcoinSerializableContract : IBitcoinSerializable
        {
            [Pure]
            public void Serialize(Stream stream)
            {
                ContractsCommon.CanWriteToStream(stream, SerializedByteSize);
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
}
