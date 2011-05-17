using System;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin.Core
{
    [ContractClass(typeof(Contracts.IFreezableContract))]
    public interface IFreezable
    {
        bool IsFrozen { get; }
        void Freeze();
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
    }
}
