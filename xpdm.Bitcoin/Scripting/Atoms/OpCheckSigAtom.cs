using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Scripting.Atoms
{
    public class OpCheckSigAtom : OpVerifyAtom
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
            var key = context.ValueStack.Peek(0);
            var sig = context.ValueStack.Peek(1);

            var subscript = context.CurrentScript.Subscript(context.LastSeparatorAtomIndex,
                                                            context.CurrentAtomIndex - context.LastSeparatorAtomIndex + 1);

            bool isValidSignature = TransactionSignatureOperations.VerifySignature(
                key, sig, subscript, context.CurrentTransaction, context.CurrentTransactionInputIndex, 0);
            
            ExecuteVerify(context, isValidSignature);
        }

        protected override void PopArguments(ExecutionContext context)
        {
            context.ValueStack.Pop();
            context.ValueStack.Pop();
        }

        public OpCheckSigAtom() : this(false) { }
        public OpCheckSigAtom(bool verifyOrFail) : base(verifyOrFail ? ScriptOpCode.OP_CHECKSIGVERIFY : ScriptOpCode.OP_CHECKSIG, verifyOrFail) { }
        public OpCheckSigAtom(ScriptOpCode opcode) : base(opcode, opcode == ScriptOpCode.OP_CHECKSIGVERIFY)
        {
            Contract.Requires(opcode == ScriptOpCode.OP_CHECKSIG || opcode == ScriptOpCode.OP_CHECKSIGVERIFY);
        }

    }
}
