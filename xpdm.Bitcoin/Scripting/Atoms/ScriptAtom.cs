using System;
using System.Diagnostics.Contracts;
using C5;
using System.Numerics;
using System.IO;

namespace xpdm.Bitcoin.Scripting.Atoms
{
    public abstract class ScriptAtom : Core.BitcoinSerializable, IScriptAtom
    {
        public virtual int OperandCount { get { return 0; } }
        public virtual int ResultCount { get { return 1; } }
        public virtual int AltStackChange { get { return 0; } }
        
        [Pure]
        public bool CanExecute(ExecutionContext context)
        {
            return
                context.ValueStack.Count >= OperandCount
                && context.AltStack.Count <= AltStackChange
                && CanExecuteImpl(context);
        }

        [Pure]
        protected virtual bool CanExecuteImpl(ExecutionContext context)
        {
            return true;
        }

        [Pure]
        public void Execute(ExecutionContext context)
        {
            try
            {
                ExecuteImpl(context);
            }
            catch (Exception)
            {
                context.HardFailure = true;
                throw;
            }
        }

        [Pure]
        protected abstract void ExecuteImpl(ExecutionContext context);

        protected ScriptAtom() { }
        protected ScriptAtom(Stream stream) : base(stream) { }
        protected ScriptAtom(byte[] buffer, int offset) : base(buffer, offset) { }
    }
}
