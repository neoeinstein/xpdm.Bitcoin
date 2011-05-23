using System.Diagnostics.Contracts;

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
