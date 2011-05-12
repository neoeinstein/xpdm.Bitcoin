using System.Diagnostics.Contracts;
using System;
namespace xpdm.Bitcoin.Scripting.Atoms
{
    public class OpDupAtom : OpAtom
    {
        public override int OperandCount
        {
            get
            {
                return (OpCode == ScriptOpCode.OP_3DUP ? 3 : OpCode == ScriptOpCode.OP_2DUP ? 2 : 1);
            }
        }

        public override int ResultCount
        {
            get
            {
                return OperandCount * 2;
            }
        }

        protected override void ExecuteImpl(ExecutionContext context)
        {
            var index = OperandCount - 1;
            switch (OpCode)
            {
                case ScriptOpCode.OP_3DUP:
                    context.ValueStack.Push(context.ValueStack[index]);
                    goto case ScriptOpCode.OP_2DUP;
                case ScriptOpCode.OP_2DUP:
                    context.ValueStack.Push(context.ValueStack[index]);
                    goto default;
                default:
                    context.ValueStack.Push(context.ValueStack[index]);
                    break;
            }
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
