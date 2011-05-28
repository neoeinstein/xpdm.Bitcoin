using MbUnit.Framework;
using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Tests.Core
{
    [TestFixture]
    public class BitcoinSerializableTest
    {
        #region Utility Asserts

        public static void AssertThatSerializedArrayMatches(byte[] expectedSerialized, BitcoinSerializable serializable)
        {
            var serialized = serializable.SerializeToByteArray();
            Assert.AreElementsEqual(expectedSerialized, serialized);
        }

        #endregion
    }
}
