
namespace xpdm.Bitcoin.Messaging.Payloads
{
    public abstract class PayloadBase : SerializableMessageBase, IMessagePayload
    {
        public abstract string Command { get; }

        public virtual bool IncludeChecksum
        {
            get { return true; }
        }

        protected PayloadBase() { }

        protected PayloadBase(byte[] buffer, int offset)
            : base(buffer, offset)
        {
        }
    }
}
