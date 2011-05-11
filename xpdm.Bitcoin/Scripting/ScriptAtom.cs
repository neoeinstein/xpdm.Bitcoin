using System.Diagnostics.Contracts;
using C5;
using SCG=System.Collections.Generic;

namespace xpdm.Bitcoin.Scripting
{
    public abstract class ScriptAtom : IScriptAtom
    {
        public abstract int OperandCount { get; }
        public virtual int ResultCount { get { return 1; } }
        [Pure] public virtual bool CanExecute(IStack<IScriptAtom> stack) { return true; }
        [Pure] public abstract void Execute(IStack<IScriptAtom> stack);
    }
}
