using System.Diagnostics.Contracts;
using C5;

namespace xpdm.Bitcoin.Scripting
{
    [ContractClass(typeof(IScriptAtomContract))]
    public interface IScriptAtom
    {
        int OperandCount { get; }
        int ResultCount { get; }
        [Pure] bool CanExecute(IStack<IScriptAtom> stack);
        [Pure] void Execute(IStack<IScriptAtom> stack);
    }
}
