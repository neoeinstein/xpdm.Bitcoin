using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin.Scripting.Atoms
{
    public class OpCheckSigAtom : OpAtom, IVerifyAtom
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
            var key = context.ValueStack.Peek(0);
            var sig = context.ValueStack.Peek(1);

            var subscript = context.CurrentScript.Subscript(context.LastSeparatorAtomIndex,
                                                            context.CurrentAtomIndex - context.LastSeparatorAtomIndex + 1);

            subscript.Atoms.RemoveAllCopies(new ValueAtom(sig));

            bool isValidSignature = TransactionSignatureOperations.VerifySignature(
                key, sig, subscript, context.CurrentTransaction, context.CurrentTransactionInputIndex, 0);

            context.ValueStack.Pop();
            context.ValueStack.Pop();
            context.ValueStack.Push(ExecutionContext.ToStackValue(isValidSignature));
        }

        public OpCheckSigAtom() : this(false) { }
        public OpCheckSigAtom(bool mustVerify) : base(mustVerify ? ScriptOpCode.OP_CHECKSIGVERIFY : ScriptOpCode.OP_CHECKSIG) { }
        public OpCheckSigAtom(ScriptOpCode opcode)
            : base(opcode)
        {
            Contract.Requires(opcode == ScriptOpCode.OP_CHECKSIG || opcode == ScriptOpCode.OP_CHECKSIGVERIFY);

            MustVerify = opcode == ScriptOpCode.OP_CHECKSIGVERIFY;
        }

        [ContractInvariantMethod]
        private void __Invariant()
        {
            Contract.Invariant(MustVerify && OpCode == ScriptOpCode.OP_CHECKSIGVERIFY || !MustVerify && OpCode == ScriptOpCode.OP_CHECKSIG);
        }
    }
}
