using System.Diagnostics.Contracts;
using System.Linq;

namespace xpdm.Bitcoin.Scripting.Atoms
{
    public sealed class OpEqualAtom : OpAtom, IVerifyAtom
    {
        public override int OperandCount(IExecutionContext context)
        {
            return 2;
        }

        public override int ResultCount(IExecutionContext context)
        {
            return 1;
        }

        public bool MustVerify { get; private set; }

        protected override void ExecuteImpl(IExecutionContext context)
        {
            bool equal = context.ValueStack.Peek(0).Length == context.ValueStack.Peek(1).Length
                         && context.ValueStack.Peek(0).SequenceEqual(context.ValueStack.Peek(1));

            context.ValueStack.Pop();
            context.ValueStack.Pop();
            context.ValueStack.Push(ExecutionContext.ToStackValue(equal));
        }

        public OpEqualAtom() : this(false) { }
        public OpEqualAtom(bool mustVerify) : base(mustVerify ? ScriptOpCode.OP_EQUALVERIFY : ScriptOpCode.OP_EQUAL) { }
        public OpEqualAtom(ScriptOpCode opcode)
            : base(opcode)
        {
            Contract.Requires(opcode == ScriptOpCode.OP_EQUAL || opcode == ScriptOpCode.OP_EQUALVERIFY);

            MustVerify = opcode == ScriptOpCode.OP_EQUALVERIFY;
        }

        [ContractInvariantMethod]
        private void __Invariant()
        {
            Contract.Invariant(MustVerify && OpCode == ScriptOpCode.OP_EQUALVERIFY || !MustVerify && OpCode == ScriptOpCode.OP_EQUAL);
        }
    }
}
