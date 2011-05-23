using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin.Scripting.Atoms
{
    [ContractClass(typeof(Contracts.IScriptValueAtomContract))]
    public interface IScriptValueAtom
    {
        byte[] Value { get; }
    }

    namespace Contracts
    {
        [ContractClassFor(typeof(IScriptValueAtom))]
        internal abstract class IScriptValueAtomContract : IScriptValueAtom
        {
            public byte[] Value
            {
                get
                {
                    ContractsCommon.ResultIsNonNull<byte[]>();

                    return default(byte[]);
                }
            }
        }
    }
}
