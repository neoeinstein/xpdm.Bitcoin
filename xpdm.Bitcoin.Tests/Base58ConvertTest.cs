using System;
using System.Collections.Generic;
using Gallio.Framework.Data;
using MbUnit.Framework;

namespace xpdm.Bitcoin.Tests
{
    [TestFixture]
    class Base58ConvertTest
    {
        [Test]
        [Factory("ConversionTuples")]
        public void EncodeWithCheck([Bind(2)] byte[] plaintext, [Bind(0)] string expected)
        {
            var enc = Base58Convert.EncodeWithCheck(plaintext);
            Assert.AreEqual(expected, enc, StringComparison.Ordinal);
        }

        [Test]
        [Factory("ConversionTuples")]
        public void DecodeWithCheck([Bind(2)] byte[] expected, [Bind(0)] string encoded)
        {
            var plaintext = Base58Convert.DecodeWithCheck(encoded);
            Assert.AreElementsEqual(expected, plaintext);
        }

        [Test, MultipleAsserts]
        [Factory("ConversionTuples")]
        public void TryDecodeWithCheck([Bind(2)] byte[] expected, [Bind(0)] string encoded)
        {
            byte[] plaintext;
            var success = Base58Convert.DecodeWithCheck(encoded, out plaintext);
            Assert.IsTrue(success);
            Assert.AreElementsEqual(expected, plaintext);
        }

        [Test]
        [Factory("ConversionTuples")]
        public void Encode([Bind(4)] byte[] plaintext, [Bind(0)] string expected)
        {
            var enc = Base58Convert.Encode(plaintext);
            Assert.AreEqual(expected, enc, StringComparison.Ordinal);
        }

        [Test]
        [Factory("ConversionTuples")]
        public void Decode([Bind(4)] byte[] expected, [Bind(0)] string encoded)
        {
            var plaintext = Base58Convert.Decode(encoded);
            Assert.AreElementsEqual(expected, plaintext);
        }

        [Test]
        [Factory("AddressChecks")]
        public void EncodeWithCheck(string expectedEncode, byte[] prefix, byte[] plaintext)
        {
            var enc = Base58Convert.EncodeWithCheck(prefix, plaintext);
            Assert.AreEqual(expectedEncode, enc, StringComparison.Ordinal);
        }

        [Test]
        [Factory("AddressChecks")]
        public void DecodeWithCheck(string enc, byte[] expectedPlaintext, byte[] expectedPrefix)
        {
            byte[] prefix, plaintext;
            var success = Base58Convert.DecodeWithCheck(enc, expectedPrefix.Length, out prefix, out plaintext);
            Assert.AreElementsEqual(expectedPlaintext, plaintext);
            Assert.AreElementsEqual(expectedPrefix, prefix);
            Assert.IsTrue(success);
        }

        public static IEnumerable<IDataItem> ConversionTuples
        {
            get
            {
                yield return new DataRow("93VYUMzRG9DdbRP72uQXjaWibbQwygnvaCu9DumcqDjGybD864T", "", "0xeffb309e964684b54e6069f146e2cd6dae936b711a7a98df4097156b9fc9b344eb", "1", "0xeffb309e964684b54e6069f146e2cd6dae936b711a7a98df4097156b9fc9b344eb4f9a4b14");
                yield return new DataRow("5JLoTseHAG7neUS8UrkRdDCefvVb3BaawsckyroWERqeEPWQ14N", "", "0x804549dd72054c536dd34adbb4ddf6907f2c600fb83dcbbc95798c19917ad22317", "1", "0x804549dd72054c536dd34adbb4ddf6907f2c600fb83dcbbc95798c19917ad2231767e4c2ab");
                yield return new DataRow("PCsELyrLCZLZNNbhBdtqo61hwPrKmE7Lu", "", "0xf39145d4e53ffa996b70c51c34a71dab5037946d", "1", "0xf39145d4e53ffa996b70c51c34a71dab5037946da14cf2a2");
            }
        }

        public static IEnumerable<IDataItem> AddressChecks
        {
            get
            {
                yield return new DataRow("1PCsELyrLCZLZNNbhBdtqo61hwPrKyxYvs", "0xf39145d4e53ffa996b70c51c34a71dab5037946d", new byte[] { 0 });
                yield return new DataRow("147pkTEvgKwrhw9eUAcWN4FGNaUhTSq9Bq", "0x2232bb6cdd5ba5ba3a29b25ebf934edb4d83e26c", new byte[] { 0 });
                yield return new DataRow("mzF1TAPvt3DyyMsJYRqbN7DVEaWXMWJrMQ", "0xcd665eb011aeff4ee150e74a7686308efe7ce925", new byte[] { 111 });
                yield return new DataRow("n19Q6YTwzFoq3g2JRCsZW2PKbAbUcHU6Z5", "0xd74ee70df4807e7a5fe18c435da4363f9c5ec8fb", new byte[] { 111 });
                yield return new DataRow("momxmA3hAnFYdyP17QKa4t9SEsuafyCiBn", "0x5a97a2c1d8c20f0e4619eee47f3385e05d2cda37", new byte[] { 111 });
            }
        }
    }
}
