using System.IO;
using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Messaging.Payloads
{
    public abstract class PayloadBase : BitcoinSerializable, IMessagePayload
    {
        public abstract string Command { get; }

        public virtual bool IncludeChecksum
        {
            get { return true; }
        }

        protected PayloadBase() { }
        protected PayloadBase(Stream stream) : base(stream) { }
        protected PayloadBase(byte[] buffer, int offset) : base(buffer, offset) { }
    }
}
