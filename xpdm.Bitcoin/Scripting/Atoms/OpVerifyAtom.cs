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

        public override int ResultCount
        {
            get
            {
                return 0;
            }
        }

        protected bool VerifyOrFail { get; private set; }

        protected override void ExecuteImpl(ExecutionContext context)
        {
            ExecuteVerify(context, ExecutionContext.ToBool(context.ValueStack.Peek()));
        }

        protected void ExecuteVerify(ExecutionContext context, bool valid)
        {
            if (!VerifyOrFail)
            {
                PopArguments(context);
                context.ValueStack.Push(ExecutionContext.ToStackValue(valid));
            }
            else if (valid)
            {
                PopArguments(context);
            }
            else
            {
                context.HardFailure = true;
            }
        }

        protected virtual void PopArguments(ExecutionContext context)
        {
            context.ValueStack.Pop();
        }

        public OpVerifyAtom() : this(ScriptOpCode.OP_VERIFY, true) { }

        protected OpVerifyAtom(ScriptOpCode opcode, bool verifyOrFail) : base(opcode)
        {
            VerifyOrFail = verifyOrFail;
        }
    }
}
