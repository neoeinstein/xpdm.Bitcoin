using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xpdm.Bitcoin.Scripting.Atoms
{
    class OpInvalidAtom : OpAtom
    {
        protected override void ExecuteImpl(ExecutionContext context)
        {
            context.HardFailure = true;
        }

        public OpInvalidAtom() : this(ScriptOpCode.OP_INVALIDOPCODE) { }
        public OpInvalidAtom(ScriptOpCode opcode) : base(opcode) { }

    }
}
