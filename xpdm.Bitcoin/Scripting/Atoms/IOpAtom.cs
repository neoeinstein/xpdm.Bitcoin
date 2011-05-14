using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin.Scripting.Atoms
{
    [ContractClass(typeof(IOpAtomContract))]
    public interface IOpAtom
    {
        ScriptOpCode OpCode { get; }
    }

    [ContractClassFor(typeof(IOpAtom))]
    public abstract class IOpAtomContract : IOpAtom
    {
        public ScriptOpCode OpCode
        {
            get
            {
                Contract.Ensures(Contract.Result<ScriptOpCode>() >= ScriptOpCode.OP_PUSHDATA1);

                return default(ScriptOpCode);
            }
        }
    }
}
