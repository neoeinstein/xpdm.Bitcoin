using System;

namespace xpdm.Bitcoin.Core
{
    [Serializable]
    public class ObjectFrozenException : Exception
    {
        public ObjectFrozenException() { }
        public ObjectFrozenException(string message) : base(message) { }
        public ObjectFrozenException(string message, Exception inner) : base(message, inner) { }
        protected ObjectFrozenException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
