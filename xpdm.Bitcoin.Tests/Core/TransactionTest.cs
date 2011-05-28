using MbUnit.Framework;
using xpdm.Bitcoin.Core;
using xpdm.Bitcoin.Tests.Factories.Core;

namespace xpdm.Bitcoin.Tests.Core
{
    [TestFixture]
    public class TransactionTest
    {
        [Test]
        [Factory(typeof(TransactionData), "TransactionsWithHash")]
        public void VerifyTransactionHash(
            Hash256 txHash,
            Transaction tx)
        {
            BitcoinObjectTest.AssertThatHashMatches(tx, txHash);
        }

        [Test]
        [Factory(typeof(TransactionData), "TransactionsWithCoinbase")]
        public void VerifyIsCoinbase(
            bool expectedCoinbase,
            Transaction tx)
        {
            bool isCoinbase = tx.TransactionInputs.Count == 1 && tx.TransactionInputs[0].IsCoinbase;
            Assert.AreEqual(expectedCoinbase, isCoinbase);
        }

        [Test]
        [Factory(typeof(TransactionData), "SerializedTransactionsForRoundTripping")]
        public void RoundTripBitcoinSerializedTransactions(
            Transaction expectedTx,
            byte[] expectedSerializedTx)
        {
            var tx = new Transaction(expectedSerializedTx, 0);
            Assert.AreEqual(expectedTx, tx);
            BitcoinSerializableTest.AssertThatSerializedArrayMatches(expectedSerializedTx, expectedTx);
        }
    }
}
