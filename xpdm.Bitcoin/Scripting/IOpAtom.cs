using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin.Scripting
{
    [ContractClass(typeof(IOpAtomContract))]
    public interface IOpAtom
    {
        ScriptOpCode OpCode { get; }
    }
}
