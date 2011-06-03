using System.Collections.Generic;
using Gallio.Framework;
using MbUnit.Framework;
using xpdm.Bitcoin.Messaging.Tests.SerializedData;

namespace xpdm.Bitcoin.Messaging.Tests
{
    [TestFixture]
    public class MessageTest
    {
        [Test]
        [Factory("SerializedMessages")]
        public void Test(byte[] message)
        {
            var msg = new Message(message, 0);
            Assert.IsNotNull(msg);
            TestLog.WriteLine("Network: {0}", msg.Network);
            TestLog.WriteLine("Command: {0}", msg.Command);
            TestLog.WriteLine("PayloadLength: {0}", msg.PayloadLength);
            TestLog.WriteLine("PayloadPayload: {0}", msg.Payload);
            Assert.AreElementsEqual(message, msg.SerializeToByteArray());
        }

        public static IEnumerable<byte[]> SerializedMessages
        {
            get
            {
                yield return Access.GetSerializedData("MsgBlock124009.bin");
                yield return Access.GetSerializedData("MsgBlock124010.bin");
            }
        }
    }
}
