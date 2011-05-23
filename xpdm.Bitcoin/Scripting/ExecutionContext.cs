using System.Diagnostics.Contracts;
using C5;

namespace xpdm.Bitcoin.Scripting
{
    public class ExecutionContext
    {
        public static readonly int MaximumCombinedStackSize = 1000;
        public static readonly int MaximumOpAtomsPerScript = 200;

        public IStack<byte[]> ValueStack { get; private set; }
        public IStack<byte[]> AltStack { get; private set; }
        public IStack<bool> ControlStack { get; private set; }

        public int OpAtomsExecuted { get; private set; }

        public bool Execute(Core.Script script, Core.Transaction transaction, int transactionInputIndex)
        {
            try
            {
                CurrentScript = script;
                CurrentTransaction = transaction;
                CurrentTransactionInputIndex = transactionInputIndex;

                foreach (var atom in script.Atoms)
                {
                    if (this.ExecutionResult.HasValue)
                    {
                        break;
                    }
                    if (!atom.CanExecute(this))
                    {
                        this.HardFailure = true;
                    }
                    atom.Execute(this);

                    if (atom is Atoms.OpAtom)
                    {
                        ++OpAtomsExecuted;
                    }
                    ++CurrentAtomIndex;
                }
            }
            finally
            {
                CurrentScript = null;
                CurrentTransaction = null;
                CurrentTransactionInputIndex = 0;
                CurrentAtomIndex = 0;
                LastSeparatorAtomIndex = 0;
            }

            return this.ExecutionResult != false;
        }

        public bool ExecuteFinal(Core.Script script, Core.Transaction transaction, int transactionInputIndex)
        {
            Contract.Ensures(InFinalState);
            Contract.Ensures(ExecutionResult != null);

            Execute(script, transaction, transactionInputIndex);

            Finish();

            return this.ExecutionResult == true;
        }

        public void Finish()
        {
            Contract.Ensures(InFinalState);
            Contract.Ensures(ExecutionResult != null);

            this.InFinalState = true;
        }

        private bool _hardFailure = false;
        public bool HardFailure
        {
            get { return _hardFailure; }
            set
            {
                Contract.Ensures(Contract.OldValue<bool>(HardFailure) == true || HardFailure == value);

                _hardFailure |= value;
            }
        }

        private bool _inFinalState = false;
        public bool InFinalState
        {
            get { return _inFinalState; }
            set
            {
                Contract.Ensures(Contract.OldValue<bool>(InFinalState) == true || InFinalState == value);
                Contract.Ensures(!value || ExecutionResult != null);

                _inFinalState |= value;
            }
        }

        public ExecutionContext()
        {
            ValueStack = new CircularQueue<byte[]>();
            AltStack = new CircularQueue<byte[]>();
            ControlStack = new CircularQueue<bool>();
        }

        public bool? ExecutionResult
        {
            get
            {
                Contract.Ensures(!InFinalState || Contract.Result<bool?>() != null);

                if (HardFailure || !IsValid)
                {
                    return false;
                }
                if (!InFinalState)
                {
                    return null;
                }
                if (ValueStack.Count == 1 && AltStack.Count == 0 && ControlStack.Count == 0)
                {
                    return !HardFailure && IsValid && ExecutionContext.ToBool(ValueStack.Peek());
                }
                return false;
            }
        }

        public bool ExecuteBlock
        {
            get
            {
                return !ControlStack.All(b => b);
            }
        }

        public bool IsValid
        {
            get
            {
                return ValueStack.Count + AltStack.Count <= MaximumCombinedStackSize
                    && OpAtomsExecuted <= MaximumOpAtomsPerScript;
            }
        }

        public Core.Script CurrentScript { get; private set; }
        public Core.Transaction CurrentTransaction { get; private set; }
        public int CurrentTransactionInputIndex { get; private set; }
        public int CurrentAtomIndex { get; set; }
        public int LastSeparatorAtomIndex { get; set; }

        public static bool ToBool(byte[] val)
        {
            ContractsCommon.NotNull(val, "val");

            for (int i = 0; i < val.Length; ++i)
            {
                if (val[i] != 0)
                {
                    // Can be negative zero
                    if (i == val.Length - 1 && val[i] == 0x80)
                        return false;
                    return true;
                }
            }
            return false;
        }

        public static byte[] ToStackValue(bool val)
        {
            return val ? ExecutionContext.True : ExecutionContext.False;
        }

        public static byte[] True
        {
            get
            {
                return new byte[] { 0x01 };
            }
        }

        public static byte[] False
        {
            get
            {
                return new byte[] { 0x00 };
            }
        }

        [ContractInvariantMethod]
        private void __Invariant()
        {
            Contract.Invariant(ValueStack != null);
            Contract.Invariant(ControlStack != null);
            Contract.Invariant(AltStack != null);
            Contract.Invariant(CurrentScript == null);
            Contract.Invariant(CurrentTransaction == null);
            Contract.Invariant(CurrentTransactionInputIndex == 0);
            Contract.Invariant(CurrentAtomIndex == 0);
            Contract.Invariant(LastSeparatorAtomIndex == 0);
            Contract.Invariant(!InFinalState || ExecutionResult.HasValue);
        }
    }
}
