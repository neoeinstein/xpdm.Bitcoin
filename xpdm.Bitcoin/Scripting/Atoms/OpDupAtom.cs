using System;
using System.Diagnostics.Contracts;
namespace xpdm.Bitcoin.Scripting.Atoms
{
    public sealed class OpDupAtom : OpAtom
    {
        public override int OperandCount(IExecutionContext context)
        {
            return (OpCode == ScriptOpCode.OP_3DUP ? 3 : OpCode == ScriptOpCode.OP_2DUP ? 2 : 1);
        }

        public override int ResultCount(IExecutionContext context)
        {
            return OperandCount(context) * 2;
        }

        protected override void ExecuteImpl(IExecutionContext context)
        {
            var index = OperandCount(context) - 1;
            switch (OpCode)
            {
                case ScriptOpCode.OP_3DUP:
                    context.ValueStack.Push(context.ValueStack.Peek(index));
                    goto case ScriptOpCode.OP_2DUP;
                case ScriptOpCode.OP_2DUP:
                    context.ValueStack.Push(context.ValueStack.Peek(index));
                    goto default;
                default:
                    context.ValueStack.Push(context.ValueStack.Peek(index));
                    break;
            }
        }

        public OpDupAtom()
            : this(ScriptOpCode.OP_DUP)
        {
        }

        public OpDupAtom(int depth)
            : base(depth == 3 ? ScriptOpCode.OP_3DUP : depth == 2 ? ScriptOpCode.OP_2DUP : ScriptOpCode.OP_DUP)
        {
            Contract.Requires<ArgumentOutOfRangeException>(1 <= depth && depth <= 3, "depth");
        }

        public OpDupAtom(ScriptOpCode opcode)
            : base(opcode)
        {
            Contract.Requires<ArgumentException>(opcode == ScriptOpCode.OP_DUP || opcode == ScriptOpCode.OP_2DUP || opcode == ScriptOpCode.OP_3DUP, "opcode");
        }
    }
}
