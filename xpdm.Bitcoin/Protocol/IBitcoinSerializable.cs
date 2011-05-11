using System.Diagnostics.Contracts;
using xpdm.Bitcoin;

namespace xpdm.Bitcoin.Protocol
{
    [ContractClass(typeof(IBitcoinSerializableContract))]
    public interface IBitcoinSerializable
    {
        uint ByteSize { get; }
        [Pure]
        byte[] ToBytes();
        [Pure]
        void WriteToBitcoinBuffer(byte[] buffer, int offset);
    }
}
