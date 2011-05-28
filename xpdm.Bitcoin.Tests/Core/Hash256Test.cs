using System;
using System.Collections.Generic;
using System.Numerics;
using MbUnit.Framework;
using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Tests.Core
{
    [TestFixture]
    public class Hash256Test
    {
        [Test]
        [Factory("HashStrings")]
        public void StringParseRoundTrip(string hashString)
        {
            var hash = Hash256.Parse(hashString);
            var roundTrip = hash.ToString();
            Assert.AreEqual(hashString, roundTrip, StringComparison.OrdinalIgnoreCase);
        }

        [Test]
        public void NullStringParseThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Hash256.Parse(null));
        }

        [Test]
        [Factory("InvalidHashStrings")]
        public void BadStringParseThrowsFormatException(string hashString)
        {
            Assert.Throws<FormatException>(() => Hash256.Parse(hashString));
        }

        [Test]
        public void NullStringTryParseReturnsFalse()
        {
            Hash256 dummy;
            Assert.IsFalse(Hash256.TryParse(null, out dummy));
        }

        [Test]
        [Factory("InvalidHashStrings")]
        public void ParseAndTryParseAreEquivalent(
            [RandomStrings(Count = 10, Pattern = "[0-9a-fA-F]{64}")] string hashString)
        {
            Hash256 tryHash;
            var good = Hash256.TryParse(hashString, out tryHash);
            if (good)
            {
                Hash256 hash = null;
                Assert.DoesNotThrow(() => hash = Hash256.Parse(hashString));
                Assert.AreEqual(tryHash, hash);
            }
            else
            {
                Assert.Throws<FormatException>(() => Hash256.Parse(hashString));
            }
        }

        [Test]
        public void DefaultConstruction()
        {
            var hash = new Hash256();
            Assert.AreEqual(Hash256.Empty, hash);
            Assert.AreElementsEqual(Hash256.Empty.Bytes, hash.Bytes);
        }

        [Test]
        [Factory("HashBytes")]
        public void ByteArrayContruction(byte[] hashBytes)
        {
            var hash = new Hash256(hashBytes);
            var roundTrip = hash.Bytes;
            Assert.AreElementsEqual(hashBytes, roundTrip);
        }

        [Test]
        [Factory("HashBigIntegers")]
        public void BigIntegerConstruction(BigInteger bi)
        {
            var biBytes = new byte[32];
            bi.ToByteArray().CopyTo(biBytes, 0);
            var hash = new Hash256(bi);
            Assert.AreElementsEqual(biBytes, hash.Bytes);
        }

        [Test]
        public void ExpectedHashSize()
        {
            var hash = new Hash256();
            Assert.AreEqual(32, hash.HashByteSize);
            Assert.AreEqual(hash.HashByteSize, hash.SerializedByteSize);
        }

        public static IEnumerable<string> HashStrings
        {
            get
            {
                yield return "0000000000000000000000000000000000000000000000000000000000000000";
                yield return "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF";
                yield return "00000000000000000000000000000000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF";
                yield return "8000000000000000000000000000000000000000000000000000000000000000";
                yield return "7FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF";
                yield return "acef65780a6cf890cba6df4564376978d89ca675126308741732641875dceabd";
                yield return "b51b2A55b1F0A956d6DDdc9bafCEDDAbc33286be5a72baD77fC50ebEBb19E77e";
                yield return "0C61abAddbF8A3eAaf5034Cd72CDAcF2B32febD7CDfB98Fccb923c5CFeA7fDD1";
                yield return "4E3ADFbeBef5c13b68A5Afe6aDbE696C7c2deFd1c6D33E267b57082a5C509d77";
                yield return "99d62E7B4d3560FBbd30abEd56C42fE2eB10604BbaB07eaa1d9A3dCEbF1C12ea";
                yield return "Cdc0F2EBD36a04F36742CE4E6fFBceF0e5cA8bc9b758F7FaA49cfEeD8eA62255";
            }
        }

        public static IEnumerable<string> InvalidHashStrings
        {
            get
            {
                yield return "";
                yield return "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF";
                yield return "00000000000000000000000000000000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFq";
                yield return "8000000000000000000000000000000000000000000000000000000000000000q";
                yield return "7";
                yield return "126308741732641875dceabd";
                yield return "b51b2A55b1F0A956d6DDdc9bafCED\aAbc33286be5a72baD77fC50ebEBb19E77e";
                yield return "0C61abAddbF8A3eAaf5034Cd72CDAcF2B32febD7CDfB98Fccb923c5CFeA7fDD10C61abAddbF8A3eAaf5034Cd72CDAcF2B32febD7CDfB98Fccb923c5CFeA7fDD1";
                yield return "00";
                yield return "-2dc0F2EBD36a04F36742CE4E6fFBceF0e5cA8bc9b758F7FaA49cfEeD8eA62255";
                yield return "0";
            }
        }


        public static IEnumerable<byte[]> HashBytes
        {
            get
            {
                yield return new byte[]
                    {
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
                    };
                yield return new byte[]
                    {
                        0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                        0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF
                    };
                yield return new byte[]
                    {
                        0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
                    };
                yield return new byte[]
                    {
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80
                    };
                yield return new byte[]
                    {
                        0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                        0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F
                    };
                yield return new byte[]
                    {
                        0x87, 0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80, 0x03, 0x06, 0x0C, 0x18, 0x30, 0x60, 0xC0,
                        0x81, 0x07, 0x0E, 0x1C, 0x38, 0x70, 0xE0, 0xC1, 0x83, 0x0F, 0x1E, 0x3C, 0x78, 0xF0, 0xE1, 0xC3
                    };
                yield return new byte[]
                    {
                        0x94, 0x32, 0x93, 0x43, 0x93, 0x24, 0x93, 0x18, 0x91, 0x32, 0x6A, 0xBD, 0xFF, 0xE6, 0xBD, 0x4E,
                        0xDE, 0xAD, 0xBE, 0xEF, 0xFE, 0xED, 0xFA, 0xCE, 0x3D, 0x29, 0x13, 0x06, 0x59, 0x99, 0xAA, 0x00
                    };
            }
        }

        public static IEnumerable<BigInteger> HashBigIntegers
        {
            get
            {
                foreach (var arr in HashBytes)
                    yield return new BigInteger(arr);
            }
        }
    }
}
