using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xpdm.Bitcoin
{
    public abstract class MessagePayloadBase : BitcoinSerializableBase, IMessagePayload
    {
        public abstract string Command { get; }

        public virtual bool IncludeChecksum
        {
            get { return true; }
        }
    }
}
