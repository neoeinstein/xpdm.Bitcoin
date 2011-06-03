using System.Diagnostics.Contracts;
using C5;
using xpdm.Bitcoin.Core;
using xpdm.Bitcoin.Scripting.Atoms;

namespace xpdm.Bitcoin.Scripting
{
    public class ExecutionContext : IExecutionContext
    {
        public static readonly int MaximumCombinedStackSize = 1000;
        public static readonly int MaximumOpAtomsPerScript = 200;

        public IStack<byte[]> ValueStack { get; private set; }
        public IStack<byte[]> AltStack { get; private set; }
        public IStack<bool> ControlStack { get; private set; }

        public int OpAtomsExecuted { get; private set; }

        public bool Execute(Script script, Transaction transaction, int transactionInputIndex)
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

                    if (atom is IVerifyAtom && (atom as IVerifyAtom).MustVerify)
                    {
                        ExecuteVerify();
                    }

                    if (atom is OpAtom)
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

        private void ExecuteVerify()
        {
            Contract.Ensures(Contract.OldValue(ToBool(ValueStack.Peek()))
                                && ValueStack.Count == Contract.OldValue(ValueStack.Count) - 1
                                && Contract.OldValue(HardFailure) == HardFailure
                          || !Contract.OldValue(ToBool(ValueStack.Peek()))
                                && ValueStack.Count == Contract.OldValue(ValueStack.Count)
                                && HardFailure == true
                                && ByteArrayComparer.Instance.Compare(ValueStack.Peek(), Contract.OldValue(ValueStack.Peek())) == 0);
            var valid = ToBool(ValueStack.Peek());
            if (valid)
            {
                ValueStack.Pop();
            }
            else
            {
                HardFailure = true;
            }
        }

        public bool ExecuteFinal(Core.Script script, Core.Transaction transaction, int transactionInputIndex)
        {
            Execute(script, transaction, transactionInputIndex);

            Finish();

            return this.ExecutionResult == true;
        }

        public void Finish()
        {
            this.IsInFinalState = true;
        }

        private bool _hardFailure = false;
        public bool HardFailure
        {
            get { return _hardFailure; }
            set
            {
                _hardFailure |= value;
            }
        }

        private bool _isInFinalState = false;
        public bool IsInFinalState
        {
            get { return _isInFinalState; }
            private set
            {
                _isInFinalState |= value;
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
                if (HardFailure || !IsValid)
                {
                    return false;
                }
                if (!IsInFinalState)
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

        public Script CurrentScript { get; private set; }
        public Transaction CurrentTransaction { get; private set; }
        public int CurrentTransactionInputIndex { get; private set; }
        public int CurrentAtomIndex { get; private set; }
        public int LastSeparatorAtomIndex { get; set; }

        [Pure]
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

        [Pure]
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
    }
}
