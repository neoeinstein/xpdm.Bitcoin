using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin.Scripting.Atoms
{
    class OpInvalidAtom : OpAtom
    {
        protected override void ExecuteImpl(IExecutionContext context)
        {
            Contract.Ensures(context.HardFailure == true);

            context.HardFailure = true;
        }

        public OpInvalidAtom() : this(ScriptOpCode.OP_INVALIDOPCODE) { }
        public OpInvalidAtom(ScriptOpCode opcode) : base(opcode) { }

    }
}
