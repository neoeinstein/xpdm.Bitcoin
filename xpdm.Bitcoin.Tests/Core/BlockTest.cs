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
            Block expectedHeader,
            Hash256 expectedHash,
            IEnumerable<Hash256> expectedMerkleTree)
        {
            var block = new Block(serializedBlock, offset);
            BlockTest.AssertThatMerkleTreeMatches(block.MerkleTree, expectedMerkleTree);
            BlockTest.AssertThatBlockHeaderMatches(block, expectedHeader);
            BitcoinObjectTest.AssertThatHashMatches(block, expectedHash);
            BitcoinSerializableTest.AssertThatSerializedArrayMatches(serializedBlock, block);
        }

        [Test]
        [Factory(typeof(BlockData), "BlocksForSerialization")]
        public void SerializedBlockToBlockHeaderTest(
            byte[] serializedBlock,
            int offset,
            Block expectedHeader,
            Hash256 expectedHash,
            IEnumerable<Hash256> expectedMerkleTree)
        {
            var actual = new Block(serializedBlock, offset);
            var blockHeader = actual.ToBlockHeader();
            TestLog.WriteLine(blockHeader);
            BlockTest.AssertThatBlockHeaderMatches(blockHeader, expectedHeader);
            BitcoinObjectTest.AssertThatHashMatches(blockHeader, expectedHash);
        }

        [Test]
        [Factory(typeof(BlockData), "BlockTuples")]
        public void ToBlockHeaderTest(
            Block actual,
            Block expectedHeader,
            Hash256 expectedHash,
            IEnumerable<Hash256> expectedMerkleTree)
        {
            var blockHeader = actual.ToBlockHeader();
            TestLog.WriteLine(blockHeader);
            BlockTest.AssertThatBlockHeaderMatches(blockHeader, expectedHeader);
            BitcoinObjectTest.AssertThatHashMatches(blockHeader, expectedHash);
        }

        [Test]
        [Factory(typeof(BlockData), "BlockTuples")]
        public void ValidateBlockCalculations(
            Block actual,
            Block expectedHeader,
            Hash256 expectedHash,
            Hash256[] expectedMerkleTree)
        {
            BlockTest.AssertThatMerkleTreeMatches(actual.MerkleTree, expectedMerkleTree);
            BlockTest.AssertThatBlockHeaderMatches(actual, expectedHeader);
            BitcoinObjectTest.AssertThatHashMatches(actual, expectedHash);
        }

        #region Utility Asserts

        public static void AssertThatMerkleTreeMatches(IEnumerable<Hash> actuals, IEnumerable<Hash> expected)
        {
            Assert.AreElementsEqual(expected, actuals);
        }

        public static void AssertThatBlockHeaderMatches(
            Block actualBlock,
            Block expectedBlock)
        {
            BlockTest.AssertThatBlockHeaderMatches(
                actualBlock,
                expectedBlock.Version,
                expectedBlock.PreviousBlockHash,
                expectedBlock.MerkleRoot,
                expectedBlock.Timestamp,
                expectedBlock.DifficultyBits,
                expectedBlock.Nonce);
        }

        public static void AssertThatBlockHeaderMatches(
            Block actualBlock,
            uint expectedVersion,
            Hash256 expectedPreviousBlockHash,
            Hash256 expectedMerkleRoot,
            Timestamp expectedTimestamp,
            uint expectedDifficultyBits,
            uint expectedNonce)
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedVersion, actualBlock.Version);
                Assert.AreEqual(expectedPreviousBlockHash, actualBlock.PreviousBlockHash);
                Assert.AreEqual(expectedMerkleRoot, actualBlock.MerkleRoot);
                Assert.AreEqual(expectedTimestamp, actualBlock.Timestamp);
                Assert.AreEqual(expectedDifficultyBits, actualBlock.DifficultyBits);
                Assert.AreEqual(expectedNonce, actualBlock.Nonce);
            });
        }

        #endregion
    }
}
