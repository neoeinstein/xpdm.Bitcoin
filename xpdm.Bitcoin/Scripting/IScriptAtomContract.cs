using System;
using C5;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin.Scripting
{
    [ContractClassFor(typeof(IScriptAtom))]
    abstract class IScriptAtomContract : IScriptAtom
    {
        public int OperandCount
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);

                return default(int);
            }
        }

        public int ResultCount
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);

                return default(int);
            }
        }

        [Pure]
        public bool CanExecute(IStack<IScriptAtom> stack)
        {
            Contract.Ensures(stack.Count >= OperandCount || Contract.Result<bool>() == false);

            return default(bool);
        }

        [Pure]
        public void Execute(IStack<IScriptAtom> stack)
        {
            Contract.Requires(this.CanExecute(stack));
            Contract.Ensures(Contract.OldValue(stack.Count) - this.OperandCount + this.ResultCount == stack.Count );
            Contract.EnsuresOnThrow<Exception>(Contract.OldValue(stack.Count) == stack.Count);
        }
    }
}
