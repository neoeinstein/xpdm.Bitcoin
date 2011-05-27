using System;
using System.Collections.Generic;
using MbUnit.Framework;
using NHamcrest.Core;
using xpdm.Bitcoin.Core;
using xpdm.Bitcoin.Tests.Factories.Core;

namespace xpdm.Bitcoin.Tests.Core
{
    [TestFixture]
    public class BlockTest
    {
        [Test]
        [Factory(typeof(Blocks), "BlocksForSerialization")]
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
            var ser = new byte[serializedBlock.Length - 24];
            Array.Copy(serializedBlock, 24, ser, 0, ser.Length);
            BitcoinSerializableTest.AssertThatSerializedArrayMatches(block, ser);
        }

        [Test]
        [Factory(typeof(Blocks), "BlockTuples")]
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
            Assert.Over.Pairs(actuals, expected, (l, r) => Assert.That(l, Is.EqualTo(r)));
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
                Assert.That(actualBlock.Version, Is.EqualTo(expectedVersion));
                Assert.That(actualBlock.PreviousBlockHash, Is.EqualTo(expectedPreviousBlockHash));
                Assert.That(actualBlock.MerkleRoot, Is.EqualTo(expectedMerkleRoot));
                Assert.That(actualBlock.Timestamp, Is.EqualTo(expectedTimestamp));
                Assert.That(actualBlock.DifficultyBits, Is.EqualTo(expectedDifficultyBits));
                Assert.That(actualBlock.Nonce, Is.EqualTo(expectedNonce));
            });
        }

        #endregion
    }
}
