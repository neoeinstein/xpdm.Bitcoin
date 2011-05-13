using System.Numerics;
using SCG = System.Collections.Generic;
using C5;

namespace xpdm.Bitcoin.Scripting
{
    public class Script : LinkedList<IScriptAtom>
    {
        public static readonly int MaximumScriptAtoms = 200;

        public bool Execute()
        {
            var context = new ExecutionContext(this);

            foreach (var atom in this)
            {
                if (context.ExecutionResult.HasValue)
                {
                    return context.ExecutionResult.Value;
                }
                if (!atom.CanExecute(context))
                {
                    return false;
                }
                atom.Execute(context);
                ++context.CurrentIndex;
            }

            context.InFinalState = true;

            return context.ExecutionResult == true;
        }
    }
}
