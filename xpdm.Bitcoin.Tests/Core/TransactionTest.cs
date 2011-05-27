using System.Collections.Generic;
using Gallio.Framework.Data;
using MbUnit.Framework;
using NHamcrest.Core;
using xpdm.Bitcoin.Core;
using xpdm.Bitcoin.Tests.Factories.Core;

namespace xpdm.Bitcoin.Tests.Core
{
    [TestFixture]
    public class TransactionTest
    {
        [Test]
        [Factory("TransactionData")]
        public void VerifyTransactionHash(
            [Bind(0)] Transaction tx,
            [Bind(1)] Hash256 txHash)
        {
            BitcoinObjectTest.AssertThatHashMatches(tx, txHash);
        }

        [Test]
        [Factory("TransactionData")]
        public void VerifyIsCoinbase(
            [Bind(0)] Transaction tx,
            [Bind(2)] bool expectedCoinbase)
        {
            Assert.That(tx.TransactionInputs.All(txIn => txIn.IsCoinbase), Is.EqualTo(expectedCoinbase));
        }

        public static IEnumerable<IDataItem> TransactionData
        {
            get
            {
                yield return new DataRow(Transactions.Block000000.Tx0, Transactions.Block000000.Tx0_Hash, true);
                yield return new DataRow(Transactions.Block072783.Tx0, Transactions.Block072783.Tx0_Hash, true);
                yield return new DataRow(Transactions.Block072783.Tx1, Transactions.Block072783.Tx1_Hash, false);
                yield return new DataRow(Transactions.Block072785.Tx0, Transactions.Block072785.Tx0_Hash, true);
                yield return new DataRow(Transactions.Block072785.Tx1, Transactions.Block072785.Tx1_Hash, false);
                yield return new DataRow(Transactions.Block072785.Tx2, Transactions.Block072785.Tx2_Hash, false);
                yield return new DataRow(Transactions.Block072785.Tx3, Transactions.Block072785.Tx3_Hash, false);
                yield return new DataRow(Transactions.Block072785.Tx4, Transactions.Block072785.Tx4_Hash, false);
                yield return new DataRow(Transactions.Block072785.Tx5, Transactions.Block072785.Tx5_Hash, false);
                yield return new DataRow(Transactions.Block103640.Tx0, Transactions.Block103640.Tx0_Hash, true);
                yield return new DataRow(Transactions.Block103640.Tx1, Transactions.Block103640.Tx1_Hash, false);
                yield return new DataRow(Transactions.Block103958.Tx0, Transactions.Block103958.Tx0_Hash, true);
                yield return new DataRow(Transactions.Block103958.Tx1, Transactions.Block103958.Tx1_Hash, false);
                yield return new DataRow(Transactions.Block103958.Tx2, Transactions.Block103958.Tx2_Hash, false);
            }
        }
    }
}
