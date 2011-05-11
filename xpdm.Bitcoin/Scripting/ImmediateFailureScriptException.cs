using System;
using System.Runtime.Serialization;

namespace xpdm.Bitcoin.Scripting
{
    [Serializable]
    public class ImmediateFailureScriptException : Exception
    {
        public ImmediateFailureScriptException()
            : this("Script threw an immediate failure.")
        {
        }
        public ImmediateFailureScriptException(string message)
            : this(message, null)
        {
        }
        public ImmediateFailureScriptException(string message, Exception inner)
            : base("Script threw an immediate failure: " + message, inner)
        {
        }
        // This constructor is needed for serialization.
        protected ImmediateFailureScriptException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
