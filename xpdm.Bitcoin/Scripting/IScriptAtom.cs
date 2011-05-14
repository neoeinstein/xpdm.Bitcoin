using System.Diagnostics.Contracts;
using C5;
using System.Numerics;

namespace xpdm.Bitcoin.Scripting
{
    [ContractClass(typeof(IScriptAtomContract))]
    public interface IScriptAtom
    {
        int OperandCount { get; }
        int ResultCount { get; }
        int AltStackChange { get; }
        [Pure] bool CanExecute(ExecutionContext context);
        [Pure] void Execute(ExecutionContext context);
    }
}
