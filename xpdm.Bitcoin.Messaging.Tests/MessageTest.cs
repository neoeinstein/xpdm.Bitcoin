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
        public void RoundTripDeserialize(byte[] message)
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

                var inv = new Payloads.InvPayload(new[]
                {
                    new InventoryVector(InventoryObjectType.Msg_Block, Core.Hash256.Empty),
                    new InventoryVector(InventoryObjectType.Msg_Tx, Core.Hash256.Parse("a1bb3b094946d7fe01776a87334ec8b9099e1853c25ba15c5be1eabf4070274d")),
                    new InventoryVector(InventoryObjectType.Msg_Block, Core.Hash256.Parse("000000000000b2ecc741843890bd619af991fa1e4c555636867cc759e1265d95")),
                    new InventoryVector(InventoryObjectType.Msg_Tx, Core.Hash256.Parse("60742d2df2de74fe4d2a2cf08c62e3e61665aa68834ab3a094e591ed1b5b98a8")),
                    new InventoryVector(InventoryObjectType.Msg_Block, Core.Hash256.Parse("000000000000834dc3f04e4f8ba6433f5a9dd1bffd231e14c9947b36243053ef")),
                });
                msg = new Message(Network.Main, inv);
                yield return msg.SerializeToByteArray();
            }
        }
    }
}
