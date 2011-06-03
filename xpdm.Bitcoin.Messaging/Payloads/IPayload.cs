using System.Diagnostics.Contracts;
using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Messaging.Payloads
{
    [ContractClass(typeof(Contracts.IPayloadContract))]
    public interface IPayload : IBitcoinSerializable
    {
        string Command { get; }
        bool IncludeChecksum { get; }
    }

    namespace Contracts
    {
        [ContractClassFor(typeof(IPayload))]
        abstract class IPayloadContract : IPayload
        {
            public string Command
            {
                get
                {
                    Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                    Contract.Ensures(Contract.Result<string>().Length <= 12);

                    return default(string);
                }
            }

            public bool IncludeChecksum
            {
                get
                {
                    return default(bool);
                }
            }

            public abstract void Serialize(System.IO.Stream stream);
            public abstract int SerializedByteSize { get; }
        }

    }
}
