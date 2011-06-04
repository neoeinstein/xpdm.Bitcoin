using System;
using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Tests.Factories.Core.Blocks
{
    class B124010 : TestBlock
    {
        private static B124010 _instance = new B124010();
        public static B124010 Instance { get { return _instance; } }

        private B124010()
        {
            base.ExpectedHash = Hash256.Parse("0000000000003a8b34e22c6e7b4934993d91261fe0f7e0f2940c6abd8bfae156");
            base.Header = new BlockHeader
            {
                Version = 1,
                PreviousBlockHash = Hash256.Parse("0000000000003a2bf9728419cd10d1c68493c99f275242db8e7df2ee9079e0ed"),
                MerkleRoot = Hash256.Parse("b8694ee1b40d0802df4755f66f662dee380f4ad5a093d48e0e12ab730af92751"),
                Timestamp = 1305422821,
                DifficultyBits = 443192243, //0x1a6a93b3
                Nonce = 3984824426,
            };
            base.Header.Freeze();
            base.ExpectedMerkleTree = new[]
            {
                Hash256.Parse("f6b344e9d4a7541a40ac6967ed7258542283e21b111322c7c6db5c1f5ed3826f"),
                Hash256.Parse("c860b61c73a23f4b0777b222edc3da4a7dbc71f74a8450cb8822d6fc40fde18b"),
                Hash256.Parse("cfb583b55ab800621dfaad5e19216520318bd980dfe23f6e2e93a027b12201d6"),
                Hash256.Parse("be076c6f4668f24c4427306e83f10f0a5dbd801a5fa1542716fbc82b26d60e36"),
                Hash256.Parse("029ee8bef83db3188bd9b7aeb3ee8cd507e1ad57d7341fdfce088fb6885867d5"),
                Hash256.Parse("247b55531c72a13048f3e1bde3bd435ec779c42d8cc33aa45f1ac1e8a9ff28f8"),
                Hash256.Parse("ace9eeed06bbc2efd574db233d7156802f24a909b9a6d32306a9b63a4d5d916f"),
                Hash256.Parse("852b7700c26499029a0f902ecdf280c471547123220e7d8c6bdb06919294fa51"),
                Hash256.Parse("13942c810a7a820fff3a380e2e1561edf225cce3d26a44a2be3e4b107839915f"),
                Hash256.Parse("84f8b9bbada011cdd870d1ecdbbf652fe3bb5a41c0e980dca450dcb9aaeeab8b"),
                Hash256.Parse("f28761813116cb7c388b386ac4880af146be9edb68664a0ad8e6ab1dc7295199"),
                Hash256.Parse("a25b72a25c6f2d52bebc4670a9832f118db1d0760698229f39d24528a0c880d9"),
                Hash256.Parse("0b05dc4b5b9e4cb37ef67d91982c068d1c646ef56de357f71a24e9ce8735fdec"),
                Hash256.Parse("e3e6c054908f591151c8b18cb88cf2878e9cd294734190ecc04289470c37327d"),
                Hash256.Parse("d6ca67466f0c58bdc5d0c2e140295d79fdea27c1bae8985292828f762ecfbfba"),
                Hash256.Parse("a111aaa5de9344f3fd33f056e3eab691179b2973ab1c196914af9045710887c5"),
                Hash256.Parse("18f385c2b3fa101b3e2fbd106a0fdbfe7c6240168d3d7f3b3ff8a10d7faa78b4"),
                Hash256.Parse("e71f71ee557a9157d8d13b3a6bf1752add91a0bc44b3ddf077c5d20232938378"),
                Hash256.Parse("d8d059aeadc543817c6133f7a9d0fc688c3de813df8d91f3c053f8eb8e5200fb"),
                Hash256.Parse("5861ee7d4d1b795c98b039c1af55a9c2149569b77391036767b20aa5263cafa9"),
                Hash256.Parse("923e6ac1d117d5e4de3005ebe21b7aa89de4bad9679251b62e6b9deb4445e767"),
                Hash256.Parse("e60aa79c2dfe4815489d5533b93b0df112efc3bf47c80c8c07783e92159392d8"),
                Hash256.Parse("9f89e501c9005f5bb67f16d33d2a8dfe5d71eb8fe59bf0f3764994b7bb5f6218"),
                Hash256.Parse("da2d633976b7f1fc012ca0a96457907852069c2441cce19b75a4c020f3260dd5"),
                Hash256.Parse("1cf8960c96a5e9a370d0c7841391019968dc3439d022a7647d77caff30db2e49"),
                Hash256.Parse("52bdd96630258c1aebcfdccd2b8971ff32cae3783958ce0c500fdff79943ec34"),
                Hash256.Parse("ee873fc0b60bc12ab154f6222d6868245d467b4f3798b47f2439a52fed222c16"),
                Hash256.Parse("b8694ee1b40d0802df4755f66f662dee380f4ad5a093d48e0e12ab730af92751"),
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
                return BlockData.GetSerializedData("MsgBlock124010.bin");
            }
        }
    }
}
