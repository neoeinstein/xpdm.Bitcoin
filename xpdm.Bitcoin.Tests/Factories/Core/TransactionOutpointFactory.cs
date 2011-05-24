using System.Collections.Generic;
using System.Linq;
using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Tests.Factories.Core
{
    /// <summary>A factory for xpdm.Bitcoin.Core.TransactionOutpoint instances</summary>
    public static class TransactionOutpointFactory
    {
        /// <summary>A factory for xpdm.Bitcoin.Core.TransactionOutpoint instances</summary>
        public static TransactionOutpoint Create(Hash256 sourceHash, int outpointSequenceNumber, bool freeze)
        {
            TransactionOutpoint transactionOutpoint = new TransactionOutpoint
            {
                SourceTransactionHash = sourceHash,
                OutputSequenceNumber = outpointSequenceNumber,
            };
            if (freeze)
            {
                transactionOutpoint.Freeze();
            }
            return transactionOutpoint;
        }

        public static IEnumerable<TransactionOutpoint> TransactionOutpoints()
        {
            yield return TransactionOutpoint.Empty;
            foreach (var outpoint in FrozenOutpoints())
                yield return outpoint;
            foreach (var outpoint in ThawedOutpoints())
                yield return outpoint;
        }

        public static IEnumerable<TransactionOutpoint> FrozenOutpoints()
        {
            return
                from source in Hash256Factory.RandomValidHashSet()
                from sequence in TransactionOutpointFactory.SequenceNumbers()
                select Create(source, sequence, true);
        }

        public static IEnumerable<TransactionOutpoint> ThawedOutpoints()
        {
            return
                from source in Hash256Factory.RandomValidHashSet()
                from sequence in TransactionOutpointFactory.SequenceNumbers()
                select Create(source, sequence, false);
        }

        public static IEnumerable<int> SequenceNumbers()
        {
            yield return 0;
            yield return 1;
            yield return -1;
            yield return int.MaxValue;
            yield return int.MinValue;
        }
    }
}
