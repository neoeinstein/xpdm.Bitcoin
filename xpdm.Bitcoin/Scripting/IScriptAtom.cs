using System;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin.Scripting
{
    [ContractClass(typeof(Contracts.IScriptAtomContract))]
    public interface IScriptAtom : Core.IBitcoinSerializable, IEquatable<IScriptAtom>
    {
        [Pure]
        int OperandCount(IExecutionContext context);
        [Pure]
        int ResultCount(IExecutionContext context);
        [Pure]
        int AltStackChange(IExecutionContext context);
        [Pure]
        bool CanExecute(IExecutionContext context);
        [Pure]
        void Execute(IExecutionContext context);
    }

    namespace Contracts
    {
        [ContractClassFor(typeof(IScriptAtom))]
        abstract class IScriptAtomContract : IScriptAtom
        {
            public int OperandCount(IExecutionContext context)
            {
                ContractsCommon.NotNull(context, "context");
                Contract.Ensures(Contract.Result<int>() >= 0);

                return default(int);
            }

            public int ResultCount(IExecutionContext context)
            {
                ContractsCommon.NotNull(context, "context");
                Contract.Ensures(Contract.Result<int>() >= 0);

                return default(int);
            }

            public int AltStackChange(IExecutionContext context)
            {
                ContractsCommon.NotNull(context, "context");

                return default(int);
            }

            [Pure]
            public bool CanExecute(IExecutionContext context)
            {
                ContractsCommon.NotNull(context, "context");

                return default(bool);
            }

            [Pure]
            public void Execute(IExecutionContext context)
            {
                ContractsCommon.NotNull(context, "context");
                Contract.Requires(this.CanExecute(context));
                Contract.Ensures(context.HardFailure || Contract.OldValue(context.ValueStack.Count) - Contract.OldValue(this.OperandCount(context)) + Contract.OldValue(this.ResultCount(context)) == context.ValueStack.Count);
                Contract.Ensures(!context.HardFailure || Contract.OldValue(context.ValueStack.Count) == context.ValueStack.Count);
                Contract.Ensures(context.HardFailure || Contract.OldValue(context.AltStack.Count) + Contract.OldValue(this.AltStackChange(context)) == context.AltStack.Count);
                Contract.Ensures(!context.HardFailure || Contract.OldValue(context.AltStack.Count) == context.AltStack.Count);
                Contract.EnsuresOnThrow<Exception>(Contract.OldValue(context.ValueStack.Count) == context.ValueStack.Count);
                Contract.EnsuresOnThrow<Exception>(Contract.OldValue(context.AltStack.Count) == context.AltStack.Count);
                Contract.EnsuresOnThrow<Exception>(context.HardFailure == true);
            }

            public abstract void Serialize(System.IO.Stream stream);
            public abstract int SerializedByteSize { get; }
            public abstract bool Equals(IScriptAtom other);
        }
    }
}