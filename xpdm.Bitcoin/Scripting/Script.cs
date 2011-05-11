using System.Numerics;
using SCG = System.Collections.Generic;
using C5;

namespace xpdm.Bitcoin.Scripting
{
    public class Script : LinkedList<IScriptAtom>
    {
        public bool Execute()
        {
            var stack = new CircularQueue<IScriptAtom>();

            foreach (var atom in this)
            {
                if (!atom.CanExecute(stack))
                {
                    return false;
                }
                try
                {
                    atom.Execute(stack);
                }
                catch (ImmediateFailureScriptException)
                {
                    return false;
                }
            }

            var finalatom = stack.Pop();

            return stack.Count == 0 && finalatom is IScriptValueAtom && ((IScriptValueAtom)finalatom).Value == BigInteger.One;
        }
    }
}
