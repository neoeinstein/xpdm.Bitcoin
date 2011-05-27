using MbUnit.Framework;
using NHamcrest.Core;
using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Tests.Core
{
    [TestFixture]
    public class BitcoinSerializableTest
    {
        #region Utility Asserts

        public static void AssertThatSerializedArrayMatches(BitcoinSerializable serializable, byte[] expectedSerialized)
        {
            var serialized = serializable.SerializeToByteArray();
            Assert.Over.Pairs(serialized, expectedSerialized, (l, r) => Assert.That(l, Is.EqualTo(r)));
        }

        #endregion
    }
}
