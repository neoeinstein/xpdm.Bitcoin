
using System;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin.Scripting.Atoms
{
    public sealed class OpVerifyAtom : OpAtom, IVerifyAtom
    {
        public bool MustVerify
        {
            get { return true; }
        }

        protected override void ExecuteImpl(ExecutionContext context)
        {
        }

        public OpVerifyAtom() : this(ScriptOpCode.OP_VERIFY) { }

        public OpVerifyAtom(ScriptOpCode opcode)
            : base(opcode)
        {
            Contract.Requires<ArgumentException>(opcode == ScriptOpCode.OP_VERIFY);
        }
    }
}
