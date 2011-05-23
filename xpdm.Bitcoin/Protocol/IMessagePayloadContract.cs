using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin.Protocol
{
    [ContractClassFor(typeof(IMessagePayload))]
    abstract class IMessagePayloadContract : IMessagePayload
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
    }
}
