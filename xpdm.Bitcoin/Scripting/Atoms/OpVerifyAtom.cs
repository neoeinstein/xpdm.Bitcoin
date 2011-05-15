using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xpdm.Bitcoin.Scripting.Atoms
{
    public class OpVerifyAtom : OpAtom
    {
        public override int OperandCount
        {
            get
            {
                return 1;
            }
        }

        protected bool VerifyOrFail { get; private set; }

        protected override void ExecuteImpl(ExecutionContext context)
        {
            if (!VerifyOrFail)
            {
                return;
            }
            if (ExecutionContext.ToBool(context.ValueStack.Peek()))
            {
                context.ValueStack.Pop();
            }
            else
            {
                context.HardFailure = true;
            }
        }

        public OpVerifyAtom() : this(ScriptOpCode.OP_VERIFY, true) { }

        protected OpVerifyAtom(ScriptOpCode opcode, bool verifyOrFail) : base(opcode)
        {
            VerifyOrFail = verifyOrFail;
        }
    }
}
