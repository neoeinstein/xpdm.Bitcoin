using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin.Scripting.Atoms
{
    [ContractClass(typeof(Contracts.IOpAtomContract))]
    public interface IOpAtom
    {
        ScriptOpCode OpCode { get; }
    }

    namespace Contracts
    {
        [ContractClassFor(typeof(IOpAtom))]
        internal abstract class IOpAtomContract : IOpAtom
        {
            public ScriptOpCode OpCode
            {
                get
                {
                    Contract.Ensures(Contract.Result<ScriptOpCode>() >= ScriptOpCode.OP_PUSHDATA1 || Contract.Result<ScriptOpCode>() == ScriptOpCode.OP_0);

                    return default(ScriptOpCode);
                }
            }
        }
    }
}
