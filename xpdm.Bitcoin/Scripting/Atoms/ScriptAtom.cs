using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace xpdm.Bitcoin.Scripting.Atoms
{
    [ContractClass(typeof(Contracts.ScriptAtomContract))]
    public abstract class ScriptAtom : Core.BitcoinSerializable, IScriptAtom
    {
        public static readonly int MaximumAtomSize = 520;

        public virtual int OperandCount(IExecutionContext context)
        {
            return 0;
        }
        public virtual int ResultCount(IExecutionContext context)
        {
            return 0;
        }
        public virtual int AltStackChange(IExecutionContext context)
        {
            return 0;
        }

        [Pure]
        public bool CanExecute(IExecutionContext context)
        {
            return
                context.ValueStack.Count >= OperandCount(context)
                && context.AltStack.Count <= AltStackChange(context)
                && CanExecuteImpl(context);
        }

        [Pure]
        protected virtual bool CanExecuteImpl(IExecutionContext context)
        {
            ContractsCommon.NotNull(context, "context");

            return true;
        }

        [Pure]
        public void Execute(IExecutionContext context)
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
        protected abstract void ExecuteImpl(IExecutionContext context);

        protected ScriptAtom() { }
        protected ScriptAtom(Stream stream) : base(stream) { }
        protected ScriptAtom(byte[] buffer, int offset) : base(buffer, offset) { }

        #region IEquatable<IScriptAtom> Members

        [Pure]
        public abstract bool Equals(IScriptAtom other);

        #endregion
    }

    namespace Contracts
    {
        [ContractClassFor(typeof(ScriptAtom))]
        internal abstract class ScriptAtomContract : ScriptAtom
        {
            protected override void ExecuteImpl(IExecutionContext context)
            {
                ContractsCommon.NotNull(context, "context");
            }

            public override bool Equals(IScriptAtom other)
            {
                return default(bool);
            }
        }
    }
}
