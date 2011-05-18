using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin.Scripting.Atoms
{
    class OpInvalidAtom : OpAtom
    {
        protected override void ExecuteImpl(ExecutionContext context)
        {
            Contract.Ensures(context.HardFailure == true);

            context.HardFailure = true;
        }

        public OpInvalidAtom() : this(ScriptOpCode.OP_INVALIDOPCODE) { }
        public OpInvalidAtom(ScriptOpCode opcode) : base(opcode) { }

    }
}
