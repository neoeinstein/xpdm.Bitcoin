using System.Diagnostics.Contracts;
using xpdm.Bitcoin;

namespace xpdm.Bitcoin.Protocol
{
    [ContractClass(typeof(IMessagePayloadContract))]
    public interface IMessagePayload
    {
        string Command { get; }
        bool IncludeChecksum { get; }
    }
}
