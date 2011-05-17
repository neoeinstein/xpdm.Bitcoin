using System;
using SCG = System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace xpdm.Bitcoin.Core
{
    [ContractClass(typeof(Contracts.IFreezableContract))]
    public interface IFreezable
    {
        bool IsFrozen { get; }
        void Freeze();
    }

    [ContractClass(typeof(Contracts.IFreezableContract<>))]
    public interface IFreezable<T> : IFreezable where T : IFreezable<T>
    {
        T Thaw();
        T ThawTree();
    }

    public static class FreezableExtensions
    {
        public static SCG.IEnumerable<T> ThawChildren<T>(SCG.IEnumerable<T> children, bool thawChildren) where T : IFreezable<T>
        {
            return thawChildren ? from c in children select c.ThawTree() : children;
        }

        public static T ThawChild<T>(T child, bool thawChildren) where T : IFreezable<T>
        {
            return thawChildren ? child.ThawTree() : child;
        }
    }

    namespace Contracts
    {
        [ContractClassFor(typeof(IFreezable))]
        public abstract class IFreezableContract : IFreezable
        {
            public bool IsFrozen
            {
                get
                {
                    return default(bool);
                }
            }

            public void Freeze()
            {
                Contract.Ensures(IsFrozen == true);
                Contract.EnsuresOnThrow<Exception>(IsFrozen == true);
            }

            [ContractInvariantMethod]
            private void __Invariant()
            {
                Contract.Invariant(!Contract.OldValue<bool>(IsFrozen) || Contract.OldValue<bool>(IsFrozen) == IsFrozen);
            }
        }

        [ContractClassFor(typeof(IFreezable<>))]
        public abstract class IFreezableContract<T> : IFreezable<T> where T : IFreezable<T>
        {
            public abstract bool IsFrozen { get; }
            public abstract void Freeze();

            public T Thaw()
            {
                ContractsCommon.ResultIsNonNull<T>();
                ContractsCommon.IsThawed(Contract.Result<T>());
                Contract.Ensures(IsFrozen == Contract.OldValue(IsFrozen));

                return default(T);
            }

            public T ThawTree()
            {
                ContractsCommon.ResultIsNonNull<T>();
                ContractsCommon.IsThawed(Contract.Result<T>());
                Contract.Ensures(IsFrozen == Contract.OldValue(IsFrozen));

                return default(T);
            }

            [ContractInvariantMethod]
            private void __Invariant()
            {
                Contract.Invariant(!Contract.OldValue<bool>(IsFrozen) || Contract.OldValue<bool>(IsFrozen) == IsFrozen);
            }
        }
    }
}
