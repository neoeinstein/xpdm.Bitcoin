using System.Numerics;

namespace xpdm.Bitcoin.Scripting
{
    public interface IScriptValueAtom
    {
        byte[] Value { get; }
    }
}
