using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Numerics;

namespace xpdm.Bitcoin.Scripting.Atoms
{
    public sealed class OpEqualAtom : OpVerifyAtom
    {
        public override int OperandCount
        {
            get
            {
                return 2;
            }
        }

        public override int ResultCount
        {
            get
            {
                return 1 - (VerifyOrFail ? base.OperandCount : 0);
            }
        }

        protected override void ExecuteImpl(ExecutionContext context)
        {
            bool equal = context.ValueStack.Peek(0).Length == context.ValueStack.Peek(1).Length
                         && context.ValueStack.Peek(0).SequenceEqual(context.ValueStack.Peek(1));

            context.ValueStack.Pop();
            context.ValueStack.Pop();
            context.ValueStack.Push(ExecutionContext.ToStackValue(equal));

            base.ExecuteImpl(context);
        }

        public OpEqualAtom() : this(false) { }
        public OpEqualAtom(bool verifyOrFail) : base(verifyOrFail ? ScriptOpCode.OP_EQUALVERIFY : ScriptOpCode.OP_EQUAL, verifyOrFail) { }
        public OpEqualAtom(ScriptOpCode opcode) : base(opcode, opcode == ScriptOpCode.OP_EQUALVERIFY)
        {
            Contract.Requires(opcode == ScriptOpCode.OP_EQUAL || opcode == ScriptOpCode.OP_EQUALVERIFY);
        }
    }
}
