using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin.Protocol
{
    [ContractClass(typeof(IMessagePayloadContract))]
    public interface IMessagePayload
    {
        string Command { get; }
        bool IncludeChecksum { get; }
    }
}
