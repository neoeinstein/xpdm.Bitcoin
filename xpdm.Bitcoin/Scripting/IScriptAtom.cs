using System.Diagnostics.Contracts;
using C5;
using System.Numerics;
using System;

namespace xpdm.Bitcoin.Scripting
{
    [ContractClass(typeof(Contracts.IScriptAtomContract))]
    public interface IScriptAtom : Core.IBitcoinSerializable, IEquatable<IScriptAtom>
    {
        int OperandCount { get; }
        int ResultCount { get; }
        int AltStackChange { get; }
        [Pure]
        bool CanExecute(ExecutionContext context);
        [Pure]
        void Execute(ExecutionContext context);
    }

    namespace Contracts
    {
        [ContractClassFor(typeof(IScriptAtom))]
        abstract class IScriptAtomContract : IScriptAtom
        {
            public int OperandCount
            {
                get
                {
                    Contract.Ensures(Contract.Result<int>() >= 0);

                    return default(int);
                }
            }

            public int ResultCount
            {
                get
                {
                    Contract.Ensures(Contract.Result<int>() >= 0);

                    return default(int);
                }
            }

            public int AltStackChange
            {
                get
                {
                    return default(int);
                }
            }

            [Pure]
            public bool CanExecute(ExecutionContext context)
            {
                ContractsCommon.NotNull(context, "context");

                return default(bool);
            }

            [Pure]
            public void Execute(ExecutionContext context)
            {
                ContractsCommon.NotNull(context, "context");
                Contract.Requires(this.CanExecute(context));
                Contract.Ensures(context.HardFailure || Contract.OldValue(context.ValueStack.Count) - this.OperandCount + this.ResultCount == context.ValueStack.Count);
                Contract.Ensures(!context.HardFailure || Contract.OldValue(context.ValueStack.Count) == context.ValueStack.Count);
                Contract.Ensures(context.HardFailure || Contract.OldValue(context.AltStack.Count) + this.AltStackChange == context.AltStack.Count);
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