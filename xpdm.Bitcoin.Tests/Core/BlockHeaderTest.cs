using System.Collections.Generic;
using Gallio.Framework;
using MbUnit.Framework;
using xpdm.Bitcoin.Core;
using xpdm.Bitcoin.Tests.Factories.Core;

namespace xpdm.Bitcoin.Tests.Core
{
    [TestFixture]
    [TestsOn(typeof(BlockHeader))]
    public class BlockHeaderTest
    {
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

        #region Utility Asserts

        public static void AssertThatMerkleTreeMatches(IEnumerable<Hash> actuals, IEnumerable<Hash> expected)
        {
            Assert.AreElementsEqual(expected, actuals);
        }

        public static void AssertThatBlockHeaderMatches(
            BlockHeader actualHeader,
            BlockHeader expectedHeader)
        {
            BlockHeaderTest.AssertThatBlockHeaderMatches(
                actualHeader,
                expectedHeader.Version,
                expectedHeader.PreviousBlockHash,
                expectedHeader.MerkleRoot,
                expectedHeader.Timestamp,
                expectedHeader.DifficultyBits,
                expectedHeader.Nonce);
        }

        public static void AssertThatBlockHeaderMatches(
            BlockHeader actualHeader,
            uint expectedVersion,
            Hash256 expectedPreviousBlockHash,
            Hash256 expectedMerkleRoot,
            Timestamp expectedTimestamp,
            uint expectedDifficultyBits,
            uint expectedNonce)
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedVersion, actualHeader.Version);
                Assert.AreEqual(expectedPreviousBlockHash, actualHeader.PreviousBlockHash);
                Assert.AreEqual(expectedMerkleRoot, actualHeader.MerkleRoot);
                Assert.AreEqual(expectedTimestamp, actualHeader.Timestamp);
                Assert.AreEqual(expectedDifficultyBits, actualHeader.DifficultyBits);
                Assert.AreEqual(expectedNonce, actualHeader.Nonce);
            });
        }

        #endregion
    }
}
