using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin.Scripting
{
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
