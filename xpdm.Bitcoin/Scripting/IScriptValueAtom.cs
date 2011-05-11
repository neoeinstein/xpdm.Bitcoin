using System.Numerics;

namespace xpdm.Bitcoin.Scripting
{
    public interface IScriptValueAtom
    {
        BigInteger Value { get; }
    }
}
