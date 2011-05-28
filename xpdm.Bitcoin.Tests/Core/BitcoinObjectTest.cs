using MbUnit.Framework;
using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Tests.Core
{
    [TestFixture]
    public class BitcoinObjectTest
    {
        #region Utility Asserts

        public static void AssertThatHashMatches(BitcoinObject bitObj, Hash256 expectedHash)
        {
            Assert.AreEqual(expectedHash, bitObj.Hash256);
        }

        public static void AssertThatHashMatches(BitcoinObject bitObj, Hash160 expectedHash)
        {
            Assert.AreEqual(expectedHash, bitObj.Hash160);
        }

        #endregion
    }
}
