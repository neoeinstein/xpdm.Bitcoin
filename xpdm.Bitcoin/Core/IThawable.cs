using System;
using SCG = System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace xpdm.Bitcoin.Core
{
    [ContractClass(typeof(Contracts.IThawableContract<>))]
    public interface IThawable<T> : IFreezable where T : IThawable<T>
    {
        T Thaw();
        T ThawTree();
    }

    public static class FreezableExtensions
    {
        public static SCG.IEnumerable<T> ThawChildren<T>(SCG.IEnumerable<T> children, bool thawChildren) where T : IThawable<T>
        {
            return thawChildren ? from c in children select c.ThawTree() : children;
        }

        public static T ThawChild<T>(T child, bool thawChildren) where T : IThawable<T>
        {
            return thawChildren ? child.ThawTree() : child;
        }
    }

    namespace Contracts
    {
        [ContractClassFor(typeof(IThawable<>))]
        public abstract class IThawableContract<T> : IThawable<T> where T : IThawable<T>
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
