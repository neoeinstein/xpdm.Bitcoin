using System;
using System.Diagnostics.Contracts;
using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Scripting
{
    [ContractClass(typeof(Contracts.IExecutionContextContract))]
    public interface IExecutionContext
    {
        bool Execute(Script script, Transaction transaction, int transactionInputIndex);
        bool ExecuteFinal(Script script, Transaction transaction, int transactionInputIndex);
        void Finish();

        bool HardFailure { get; set; }
        bool IsInFinalState { get; }
        bool IsValid { get; }

        C5.IStack<byte[]> ValueStack { get; }
        C5.IStack<byte[]> AltStack { get; }
        C5.IStack<bool> ControlStack { get; }
        bool ExecuteBlock { get; }

        int OpAtomsExecuted { get; }

        bool? ExecutionResult { get; }

        Script CurrentScript { get; }
        Transaction CurrentTransaction { get; }
        int CurrentTransactionInputIndex { get; }
        int CurrentAtomIndex { get; }
        int LastSeparatorAtomIndex { get; set; }
    }

    namespace Contracts
    {
        [ContractClassFor(typeof(IExecutionContext))]
        abstract class IExecutionContextContract : IExecutionContext
        {
            public bool Execute(Script script, Transaction transaction, int transactionInputIndex)
            {
                ContractsCommon.NotNull(script, "script");
                ContractsCommon.NotNull(transaction, "transaction");
                ContractsCommon.ValidIndex(0, transaction.TransactionInputs.Count, transactionInputIndex, "transactionInputIndex");
                Contract.Requires<InvalidOperationException>(!IsInFinalState);
                Contract.Ensures(ExecutionResult == false || Contract.Result<bool>() == true);
                Contract.EnsuresOnThrow<Exception>(CurrentScript == null);
                Contract.EnsuresOnThrow<Exception>(CurrentTransaction == null);
                Contract.EnsuresOnThrow<Exception>(CurrentTransactionInputIndex == 0);
                Contract.EnsuresOnThrow<Exception>(CurrentAtomIndex == 0);
                Contract.EnsuresOnThrow<Exception>(LastSeparatorAtomIndex == 0);

                return default(bool);
            }

            public bool ExecuteFinal(Script script, Transaction transaction, int transactionInputIndex)
            {
                ContractsCommon.NotNull(script, "script");
                ContractsCommon.NotNull(transaction, "transaction");
                ContractsCommon.ValidIndex(0, transaction.TransactionInputs.Count, transactionInputIndex, "transactionInputIndex");
                Contract.Requires<InvalidOperationException>(!IsInFinalState);
                Contract.Ensures(IsInFinalState);
                Contract.Ensures(ExecutionResult != null);
                Contract.Ensures(ExecutionResult == true || Contract.Result<bool>() == false);
                Contract.EnsuresOnThrow<Exception>(CurrentScript == null);
                Contract.EnsuresOnThrow<Exception>(CurrentTransaction == null);
                Contract.EnsuresOnThrow<Exception>(CurrentTransactionInputIndex == 0);
                Contract.EnsuresOnThrow<Exception>(CurrentAtomIndex == 0);
                Contract.EnsuresOnThrow<Exception>(LastSeparatorAtomIndex == 0);

                return default(bool);
            }

            public void Finish()
            {
                Contract.Ensures(IsInFinalState);
                Contract.Ensures(ExecutionResult != null);
            }


            public bool HardFailure
            {
                get;
                set;
            }

            public bool IsInFinalState
            {
                get
                {
                    return default(bool);
                }
            }

            public bool IsValid
            {
                get
                {
                    return default(bool);
                }
            }

            public C5.IStack<byte[]> ValueStack
            {
                get
                {
                    ContractsCommon.ResultIsNonNull<C5.IStack<byte[]>>();

                    return default(C5.IStack<byte[]>);
                }
            }

            public C5.IStack<byte[]> AltStack
            {
                get
                {
                    ContractsCommon.ResultIsNonNull<C5.IStack<byte[]>>();

                    return default(C5.IStack<byte[]>);
                }
            }

            public C5.IStack<bool> ControlStack
            {
                get
                {
                    ContractsCommon.ResultIsNonNull<C5.IStack<bool>>();

                    return default(C5.IStack<bool>);
                }
            }

            public bool ExecuteBlock
            {
                get
                {
                    return default(bool);
                }
            }

            public int OpAtomsExecuted
            {
                get
                {
                    Contract.Ensures(Contract.Result<int>() >= 0);

                    return default(int);
                }
            }

            public bool? ExecutionResult
            {
                get
                {
                    Contract.Ensures(!(IsInFinalState || HardFailure) || Contract.Result<bool?>() != null);

                    return default(bool?);
                }
            }

            public Script CurrentScript
            {
                get
                {
                    return default(Script);
                }
            }

            public Transaction CurrentTransaction
            {
                get
                {
                    return default(Transaction);
                }
            }

            public int CurrentTransactionInputIndex
            {
                get
                {
                    Contract.Ensures(Contract.Result<int>() >= 0);

                    return default(int);
                }
            }

            public int CurrentAtomIndex
            {
                get
                {
                    Contract.Ensures(Contract.Result<int>() >= 0);

                    return default(int);
                }
            }

            public int LastSeparatorAtomIndex
            {
                get
                {
                    Contract.Ensures(Contract.Result<int>() >= 0);

                    return default(int);
                }
                set
                {
                    Contract.Requires(value >= 0);
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
                Contract.Invariant(!IsInFinalState || ExecutionResult.HasValue);
                Contract.Invariant(IsInFinalState == true || Contract.OldValue<bool>(IsInFinalState) == false);
                Contract.Invariant(!HardFailure || ExecutionResult.HasValue && ExecutionResult == false);
                Contract.Invariant(HardFailure == true || Contract.OldValue<bool>(HardFailure) == false);
            }
        }
    }
}
