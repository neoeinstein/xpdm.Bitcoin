using System;
using MbUnit.Framework;

namespace xpdm.Bitcoin.Tests
{
    [TestFixture]
    public class BufferOperationsTest
    {
        [Test]
        public void StringEvenLengthRoundTripTest(
            [RandomStrings(Count = 10, Pattern = "([0-9a-fA-F][0-9a-fA-F]){1,16}")] string byteString,
            [EnumData(typeof(Endianness))] Endianness endianness)
        {
            var bytes = BufferOperations.FromByteString(byteString, endianness);
            var roundTrip = BufferOperations.ToByteString(bytes, endianness);
            Assert.AreEqual(byteString, roundTrip, StringComparison.OrdinalIgnoreCase);
        }

        [Test]
        public void StringOddLengthRoundTripTest(
            [RandomStrings(Count = 10, Pattern = "[0-9a-fA-F]([0-9a-fA-F][0-9a-fA-F]){1,15}")] string byteString,
            [EnumData(typeof(Endianness))] Endianness endianness)
        {
            var bytes = BufferOperations.FromByteString(byteString, endianness);
            var roundTrip = BufferOperations.ToByteString(bytes, endianness);
            Assert.AreEqual("0" + byteString, roundTrip, StringComparison.OrdinalIgnoreCase);
        }
    }
}
