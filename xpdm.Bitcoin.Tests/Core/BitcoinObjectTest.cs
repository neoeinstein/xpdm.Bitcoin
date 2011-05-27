using MbUnit.Framework;
using NHamcrest.Core;
using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Tests.Core
{
    [TestFixture]
    public class BitcoinObjectTest
    {
        #region Utility Asserts

        public static void AssertThatHashMatches(BitcoinObject bitObj, Hash256 expectedHash)
        {
            Assert.That(bitObj.Hash256, Is.EqualTo(expectedHash));
        }

        public static void AssertThatHashMatches(BitcoinObject bitObj, Hash160 expectedHash)
        {
            Assert.That(bitObj.Hash160, Is.EqualTo(expectedHash));
        }

        #endregion
    }
}
