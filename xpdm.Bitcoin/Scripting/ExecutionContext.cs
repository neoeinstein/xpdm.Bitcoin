using System.Diagnostics.Contracts;
using C5;
using System.Numerics;

namespace xpdm.Bitcoin.Scripting
{
    public class ExecutionContext
    {
        public static readonly int MaximumCombinedStackSize = 1000;

        public IStack<BigInteger> ValueStack { get; private set; }
        public IStack<BigInteger> AltStack { get; private set; }
        public IStack<bool> ControlStack { get; private set; }

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

        public ExecutionContext()
        {
            ValueStack = new CircularQueue<BigInteger>();
            AltStack = new CircularQueue<BigInteger>();
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
                if (ValueStack.Count == 1 && AltStack.Count == 0 && ControlStack.Count == 0)
                {
                    return !ValueStack[0].IsZero;
                }
                return null;
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
                return ValueStack.Count + AltStack.Count <= MaximumCombinedStackSize;
            }
        }
    }
}
