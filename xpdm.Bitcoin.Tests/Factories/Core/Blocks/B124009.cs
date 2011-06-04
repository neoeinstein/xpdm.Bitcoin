using System;
using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Tests.Factories.Core.Blocks
{
    class B124009 : TestBlock
    {
        private static B124009 _instance = new B124009();
        public static B124009 Instance { get { return _instance; } }

        private B124009()
        {
            base.ExpectedHash = Hash256.Parse("0000000000003a2bf9728419cd10d1c68493c99f275242db8e7df2ee9079e0ed");
            base.Header = new BlockHeader
            {
                Version = 1,
                PreviousBlockHash = Hash256.Parse("00000000000048c38a17019b15672158434e5dc7ae89bb808109843e4c7af45f"),
                MerkleRoot = Hash256.Parse("a2198c4f6b03a76aaea0f8c174de28358eb6affd66471d91ad7d0e0115bb36c1"),
                Timestamp = 1305421686,
                DifficultyBits = 443192243, //0x1a6a93b3
                Nonce = 3195439750,
            };
            base.Header.Freeze();
            base.ExpectedMerkleTree = new[]
            {
                Hash256.Parse("b3fb95ee04a3d3159ad7d1773d6fe6a1397f37fa99c138a23ef9f645b7a4a6b7"),
                Hash256.Parse("ad5512b67fee69c0e3293290c3d7ff88d6576fb8b2c55a037e0f003cf88a0151"),
                Hash256.Parse("a2198c4f6b03a76aaea0f8c174de28358eb6affd66471d91ad7d0e0115bb36c1"),
            };
            var serMsg = SerializedBlockMessage;
            var serData = new byte[serMsg.Length - 24];
            Array.Copy(serMsg, 24, serData, 0, serData.Length);
            base.SerializedBlockData = serData;
        }
        public static readonly int SerializedMessageBlockOffset = 24;

        public static byte[] SerializedBlockMessage
        {
            get
            {
                return BlockData.GetSerializedData("MsgBlock124009.bin");
            }
        }
    }
}
