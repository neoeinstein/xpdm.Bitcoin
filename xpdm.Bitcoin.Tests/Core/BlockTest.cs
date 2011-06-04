using System.Collections.Generic;
using Gallio.Framework;
using MbUnit.Framework;
using xpdm.Bitcoin.Core;
using xpdm.Bitcoin.Tests.Factories.Core;

namespace xpdm.Bitcoin.Tests.Core
{
    [TestFixture]
    [TestsOn(typeof(Block))]
    public class BlockTest
    {
        [Test]
        [Factory(typeof(BlockData), "BlocksForSerialization")]
        public void RoundTripBitcoinSerializedBlocks(
            byte[] serializedBlock,
            int offset,
            BlockHeader expectedHeader,
            Hash256 expectedHash,
            IEnumerable<Hash256> expectedMerkleTree)
        {
            var block = new Block(serializedBlock, offset);
            BlockTest.AssertThatMerkleTreeMatches(block.MerkleTree, expectedMerkleTree);
            BlockHeaderTest.AssertThatBlockHeaderMatches(block.Header, expectedHeader);
            BitcoinObjectTest.AssertThatHashMatches(block, expectedHash);
            BitcoinSerializableTest.AssertThatSerializedArrayMatches(serializedBlock, block);
        }

        [Test]
        [Factory(typeof(BlockData), "BlocksForSerialization")]
        public void SerializedBlockToBlockHeaderTest(
            byte[] serializedBlock,
            int offset,
            BlockHeader expectedHeader,
            Hash256 expectedHash,
            IEnumerable<Hash256> expectedMerkleTree)
        {
            var actual = new Block(serializedBlock, offset);
            TestLog.WriteLine(actual.Header);
            BlockHeaderTest.AssertThatBlockHeaderMatches(actual.Header, expectedHeader);
            BitcoinObjectTest.AssertThatHashMatches(actual, expectedHash);
        }

        [Test]
        [Factory(typeof(BlockData), "BlockTuples")]
        public void ToBlockHeaderTest(
            Block actual,
            BlockHeader expectedHeader,
            Hash256 expectedHash,
            IEnumerable<Hash256> expectedMerkleTree)
        {
            TestLog.WriteLine(actual.Header);
            BlockHeaderTest.AssertThatBlockHeaderMatches(actual.Header, expectedHeader);
            BitcoinObjectTest.AssertThatHashMatches(actual, expectedHash);
        }

        [Test]
        [Factory(typeof(BlockData), "BlockTuples")]
        public void ValidateBlockCalculations(
            Block actual,
            BlockHeader expectedHeader,
            Hash256 expectedHash,
            Hash256[] expectedMerkleTree)
        {
            BlockTest.AssertThatMerkleTreeMatches(actual.MerkleTree, expectedMerkleTree);
            BlockHeaderTest.AssertThatBlockHeaderMatches(actual.Header, expectedHeader);
            BitcoinObjectTest.AssertThatHashMatches(actual, expectedHash);
        }

        #region Utility Asserts

        public static void AssertThatMerkleTreeMatches(IEnumerable<Hash> actuals, IEnumerable<Hash> expected)
        {
            Assert.AreElementsEqual(expected, actuals);
        }

        public static void AssertThatTransactionsMatch(IEnumerable<Transaction> actuals, IEnumerable<Transaction> expected)
        {
            Assert.AreElementsEqual(expected, actuals);
        }


        #endregion
    }
}
