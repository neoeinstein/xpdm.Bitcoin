namespace xpdm.Bitcoin.Scripting.Atoms
{
    public class OpDupAtom : OpAtom
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
                return 2;
            }
        }

        protected override void ExecuteImpl(ExecutionContext context)
        {
            context.ValueStack.Push(context.ValueStack[0]);
        }

        public OpDupAtom() : base(ScriptOpCode.OP_DUP) { }
    }
}
