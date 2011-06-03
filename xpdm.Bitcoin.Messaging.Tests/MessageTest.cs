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
                var ver = new Payloads.VersionPayload(30000, Services.Node_Network, (Core.Timestamp)System.DateTime.Now, new NetworkAddress(Services.Node_Network, new System.Net.IPEndPoint(System.Net.IPAddress.Parse("127.0.0.1"), 6321)), new NetworkAddress(Services.Node_Network, new System.Net.IPEndPoint(System.Net.IPAddress.Parse("10.0.0.1"), 5)), 12345678, System.Text.Encoding.ASCII.GetBytes("Test"), 127001);
                var msg = new Message(Network.Main, ver);
                yield return msg.SerializeToByteArray();
            }
        }
    }
}
