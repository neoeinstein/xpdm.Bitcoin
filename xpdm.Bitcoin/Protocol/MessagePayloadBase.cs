using System;
using System.Text;
using xpdm.Bitcoin;

namespace xpdm.Bitcoin.Protocol
{
    public abstract class MessagePayloadBase : BitcoinSerializableBase, IMessagePayload
    {
        public abstract string Command { get; }

        public virtual bool IncludeChecksum
        {
            get { return true; }
        }

        protected MessagePayloadBase() { }

        protected MessagePayloadBase(byte[] buffer, int offset)
            : base(buffer, offset)
        {
        }
    }
}
