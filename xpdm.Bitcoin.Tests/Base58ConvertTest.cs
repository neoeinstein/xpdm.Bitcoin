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

        public static IEnumerable<IDataItem> ConversionTuples
        {
            get
            {
                yield return new DataRow("93VYUMzRG9DdbRP72uQXjaWibbQwygnvaCu9DumcqDjGybD864T", "", "0xeffb309e964684b54e6069f146e2cd6dae936b711a7a98df4097156b9fc9b344eb", "0x4f9a4b14", "0xeffb309e964684b54e6069f146e2cd6dae936b711a7a98df4097156b9fc9b344eb4f9a4b14");
                yield return new DataRow("5JLoTseHAG7neUS8UrkRdDCefvVb3BaawsckyroWERqeEPWQ14N", "", "0x804549dd72054c536dd34adbb4ddf6907f2c600fb83dcbbc95798c19917ad22317", "0x67e4c2ab", "0x804549dd72054c536dd34adbb4ddf6907f2c600fb83dcbbc95798c19917ad2231767e4c2ab");
                yield return new DataRow("PCsELyrLCZLZNNbhBdtqo61hwPrKmE7Lu", "", "0xf39145d4e53ffa996b70c51c34a71dab5037946d", "0xa14cf2a2", "0xf39145d4e53ffa996b70c51c34a71dab5037946da14cf2a2");
            }
        }
    }
}
