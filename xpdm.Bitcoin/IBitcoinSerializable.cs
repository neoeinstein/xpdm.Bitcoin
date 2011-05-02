using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin
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
