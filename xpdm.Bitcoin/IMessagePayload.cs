using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin
{
    [ContractClass(typeof(IMessagePayloadContract))]
    public interface IMessagePayload
    {
        string Command { get; }
        bool IncludeChecksum { get; }
    }
}
