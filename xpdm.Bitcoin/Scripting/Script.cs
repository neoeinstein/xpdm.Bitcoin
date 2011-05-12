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
            var context = new ExecutionContext();

            foreach (var atom in this)
            {
                if (context.ExecutionResult == false)
                {
                    return false;
                }
                if (!atom.CanExecute(context))
                {
                    return false;
                }
                atom.Execute(context);
            }

            return context.ExecutionResult == true;
        }
    }
}
