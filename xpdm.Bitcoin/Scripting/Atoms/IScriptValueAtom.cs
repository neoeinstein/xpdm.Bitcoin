using System.Numerics;

namespace xpdm.Bitcoin.Scripting.Atoms
{
    public interface IScriptValueAtom
    {
        byte[] Value { get; }
    }
}
