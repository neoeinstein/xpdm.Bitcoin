using System.Collections.Generic;
using System.Reflection;
using Gallio.Framework;
using Gallio.Framework.Data;
using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Tests.Factories.Core
{
    public static class Blocks
    {
        #region Block Factories

        public static IEnumerable<IDataItem> BlocksForSerialization
        {
            get
            {
                yield return new DataRow(
                    Block124009_SerializedMessage,
                    Block124009_SerializedMessageOffset,
                    Block124009_Header,
                    Block124009_Hash,
                    Block124009_MerkleTree);
                yield return new DataRow(
                    Block124010_SerializedMessage,
                    Block124010_SerializedMessageOffset,
                    Block124010_Header,
                    Block124010_Hash,
                    Block124010_MerkleTree);
            }
        }

        public static IEnumerable<IDataItem> BlockTuples
        {
            get
            {
                yield return new DataRow(
                    Block000000,
                    Block000000_Header,
                    Block000000_Hash,
                    Block000000_MerkleTree);
                yield return new DataRow(
                    Block072783,
                    Block072783_Header,
                    Block072783_Hash,
                    Block072783_MerkleTree);
                yield return new DataRow(
                    Block072785,
                    Block072785_Header,
                    Block072785_Hash,
                    Block072785_MerkleTree);
                yield return new DataRow(
                    Block103640,
                    Block103640_Header,
                    Block103640_Hash,
                    Block103640_MerkleTree);
                yield return new DataRow(
                    Block103958,
                    Block103958_Header,
                    Block103958_Hash,
                    Block103958_MerkleTree);
            }
        }

        #endregion
        #region Block 0

        public static readonly Hash256 Block000000_Hash = Hash256.Parse("000000000019d6689c085ae165831e934ff763ae46a2a6c172b3f1b60a8ce26f");

        public static readonly Block Block000000_Header = new Block
        {
            Version = 1,
            PreviousBlockHash = Hash256.Empty,
            MerkleRoot = Hash256.Parse("4a5e1e4baab89f3a32518a88c31bc87f618f76673e2cc77ab2127b7afdeda33b"),
            Timestamp = 1231006505,
            DifficultyBits = 486604799, //0x1d00ffff
            Nonce = 2083236893,
        };

        public static readonly Block Block000000 = new Block
        {
            Version = 1,
            PreviousBlockHash = Hash256.Empty,
            Timestamp = 1231006505,
            DifficultyBits = 486604799, //0x1d00ffff
            Nonce = 2083236893,
            Transactions =
                {
                    Transactions.Block000000.Tx0,
                },
        };

        public static readonly Hash256[] Block000000_MerkleTree = new[]
        {
            Hash256.Parse("4a5e1e4baab89f3a32518a88c31bc87f618f76673e2cc77ab2127b7afdeda33b"),
        };

        #endregion
        #region Block 72783

        public static readonly Hash256 Block072783_Hash = Hash256.Parse("000000000074672e28f2049c94c5d7fe3b20753f0ed6a8aa168e1c103cc48388");

        public static readonly Block Block072783_Header = new Block
        {
            Version = 1,
            PreviousBlockHash = Hash256.Parse("0000000000b94dd60e9a8fabb3d6b3a820486a566ed260855583a17f1ba18a29"),
            MerkleRoot = Hash256.Parse("bdc105236f133fb9fd8ef55fc0247bf46bb400e9d7b8b2e925b16dea931bce4f"),
            Timestamp = 1281155353,
            DifficultyBits = 469809688, //0x1c00ba18
            Nonce = 1000411397,
        };

        public static readonly Block Block072783 = new Block
        {
            Version = 1,
            PreviousBlockHash = Hash256.Parse("0000000000b94dd60e9a8fabb3d6b3a820486a566ed260855583a17f1ba18a29"),
            Timestamp = 1281155353,
            DifficultyBits = 469809688, //0x1c00ba18
            Nonce = 1000411397,
            Transactions =
                {
                    Transactions.Block072783.Tx0,
                    Transactions.Block072783.Tx1,
                },
        };

        public static readonly Hash256[] Block072783_MerkleTree = new[]
        {
            Hash256.Parse("9f92327b5a71d5acf608974fd8bbf16abc948ae21f934a2a5a7c841c499b5f92"),
            Hash256.Parse("b77e0fc6d275c951342a33473015937e62b25a68538d78a260f1225b2835a283"),
            Hash256.Parse("bdc105236f133fb9fd8ef55fc0247bf46bb400e9d7b8b2e925b16dea931bce4f"),
        };

        #endregion
        #region Block 72785

        public static readonly Hash256 Block072785_Hash = Hash256.Parse("00000000009ffdadbb2a8bcf8e8b1d68e1696802856c6a1d61561b1f630e79e7");

        public static readonly Block Block072785_Header = new Block
        {
            Version = 1,
            PreviousBlockHash = Hash256.Parse("0000000000b079382c19436fe8e076e199f0faa9014a930caf89d15744d9aec7"),
            MerkleRoot = Hash256.Parse("e81287dc0c00422aaf0db3e4586c48b01acd82b3108da6956cbd6baf19cfaf9a"),
            Timestamp = 1281156783,
            DifficultyBits = 469809688, //0x1c00ba18
            Nonce = 2283211008,
        };

        public static readonly Block Block072785 = new Block
        {
            Version = 1,
            PreviousBlockHash = Hash256.Parse("0000000000b079382c19436fe8e076e199f0faa9014a930caf89d15744d9aec7"),
            Timestamp = 1281156783,
            DifficultyBits = 469809688, //0x1c00ba18
            Nonce = 2283211008,
            Transactions =
                {
                    Transactions.Block072785.Tx0,
                    Transactions.Block072785.Tx1,
                    Transactions.Block072785.Tx2,
                    Transactions.Block072785.Tx3,
                    Transactions.Block072785.Tx4,
                    Transactions.Block072785.Tx5,
                },
        };

        public static readonly Hash256[] Block072785_MerkleTree = new[]
        {
            Hash256.Parse("2d7f4d1c25893dcaf538fdd1f34104687211ca7d8a1ba43c16b618d5fbc620c3"),
            Hash256.Parse("3407a84dce0fe04fdab91608d1974941af3683ea6e4d904a30469485c50d336a"),
            Hash256.Parse("5edf5acf8f517d965219a5495321e0bedd761daf45bcdc59a33b07b520968b8c"),
            Hash256.Parse("65c35615b476c86f28a4d3a8985ea161cc2e35e6574eacbd68942782ce29804c"),
            Hash256.Parse("89aa32f6e1b047e740401ce4fd43a865631de5a959fde7451936c28c52249b56"),
            Hash256.Parse("e3e69c802b7e36d220151e4ccdeace1d58ca2af97c5fd970314bbecd9767a514"),
            Hash256.Parse("8ebc6ac1c5c656c19632f8b7efd130303a9710ed1c0ea12935255d6fefc5d3b4"),
            Hash256.Parse("d5e41432e73312b7c82fe57303ff5bd3d0f82cb933a89bd5d80a11556fb54e07"),
            Hash256.Parse("89b77c032617fb9c37f2f922264d87764f16307541880b569fc8ca52dcec074a"),
            Hash256.Parse("d1074c2765e46d3102c51ff38d2f9a0eff87d90e5e70d89ce03f508bf5a65874"),
            Hash256.Parse("70a4e6dfb21e1e341ba2893e87bcc5473d1884505d8556cd5866a6e369174786"),
            Hash256.Parse("e81287dc0c00422aaf0db3e4586c48b01acd82b3108da6956cbd6baf19cfaf9a"),
        };

        #endregion
        #region Block 103640

        public static readonly Hash256 Block103640_Hash = Hash256.Parse("0000000000003702793ba6bf5a085eccee1ec9b249f6ff42063b34980e7cf028");

        public static readonly Block Block103640_Header = new Block
        {
            Version = 1,
            PreviousBlockHash = Hash256.Parse("000000000000111a11fcee10f2b46210aacc2e2e4c7dd1d1924b2f6f03e8316f"),
            MerkleRoot = Hash256.Parse("63a37fe141ab610d471067f75591f291c4d6d4333e3213363f06f187fce04e82"),
            Timestamp = 1295526788,
            DifficultyBits = 453217774, //0x1b038dee
            Nonce = 3592101678,
        };

        public static readonly Block Block103640 = new Block
        {
            Version = 1,
            PreviousBlockHash = Hash256.Parse("000000000000111a11fcee10f2b46210aacc2e2e4c7dd1d1924b2f6f03e8316f"),
            Timestamp = 1295526788,
            DifficultyBits = 453217774, //0x1b038dee
            Nonce = 3592101678,
            Transactions =
                {
                    Transactions.Block103640.Tx0,
                    Transactions.Block103640.Tx1,
                },
        };

        public static readonly Hash256[] Block103640_MerkleTree = new[]
        {
            Hash256.Parse("cdd3cf10fe83c28d1ac80714a48440e47f1c6b6950fec54f6b8ea04241004cb5"),
            Hash256.Parse("945691940e0ccd9f526ee1edd57a77ce170804915749702f5564c49b1f70f330"),
            Hash256.Parse("63a37fe141ab610d471067f75591f291c4d6d4333e3213363f06f187fce04e82"),
        };

        #endregion
        #region Block 103958

        public static readonly Hash256 Block103958_Hash = Hash256.Parse("000000000002055a179aa185de281da76ce6de1d0df94a7bf4ea642dff0070ae");

        public static readonly Block Block103958_Header = new Block
        {
            Version = 1,
            PreviousBlockHash = Hash256.Parse("0000000000005639d7aa5a5c32c9c2e622040c5cb4ded25a9201254cd1c09468"),
            MerkleRoot = Hash256.Parse("54312d3d842bbe585c15cef58ae300b2cf105f59e5d27f5bcc24c61d52270f31"),
            Timestamp = 1295686236,
            DifficultyBits = 453217774, //0x1b038dee
            Nonce = 3826166152,
        };

        public static readonly Block Block103958 = new Block
        {
            Version = 1,
            PreviousBlockHash = Hash256.Parse("0000000000005639d7aa5a5c32c9c2e622040c5cb4ded25a9201254cd1c09468"),
            Timestamp = 1295686236,
            DifficultyBits = 453217774, //0x1b038dee
            Nonce = 3826166152,
            Transactions =
                {
                    Transactions.Block103958.Tx0,
                    Transactions.Block103958.Tx1,
                    Transactions.Block103958.Tx2,
                },
        };

        public static readonly Hash256[] Block103958_MerkleTree = new[]
        {
            Hash256.Parse("f2dedefe5222786abd6cb2223c89e50d590f234dbba69002d5ad5cd6e438abfd"),
            Hash256.Parse("ff954e099764d192c5bb531c9c14c18c230b0c0a63f02cd168a4ea94548c890f"),
            Hash256.Parse("69ea9cf5e3e116cffe595d16b3258cd3508768fe3a1ce087ed8479f3d78ef91a"),
            Hash256.Parse("cb4e6248ed1591012e5c3c865ad732f423b6d2ac2beaa5be0c576934e8a436bd"),
            Hash256.Parse("6f9be236a0af022d0344802b2d89b85ff6a5df46db309db47b191f9f67413527"),
            Hash256.Parse("54312d3d842bbe585c15cef58ae300b2cf105f59e5d27f5bcc24c61d52270f31"),
        };

        #endregion
        #region Block 124009

        public static readonly Hash256 Block124009_Hash = Hash256.Parse("0000000000003a2bf9728419cd10d1c68493c99f275242db8e7df2ee9079e0ed");

        public static readonly Block Block124009_Header = new Block
        {
            Version = 1,
            PreviousBlockHash = Hash256.Parse("00000000000048c38a17019b15672158434e5dc7ae89bb808109843e4c7af45f"),
            MerkleRoot = Hash256.Parse("a2198c4f6b03a76aaea0f8c174de28358eb6affd66471d91ad7d0e0115bb36c1"),
            Timestamp = 1305421686,
            DifficultyBits = 443192243, //0x1a6a93b3
            Nonce = 3195439750,
        };

        public static readonly Hash256[] Block124009_MerkleTree = new[]
        {
            Hash256.Parse("b3fb95ee04a3d3159ad7d1773d6fe6a1397f37fa99c138a23ef9f645b7a4a6b7"),
            Hash256.Parse("ad5512b67fee69c0e3293290c3d7ff88d6576fb8b2c55a037e0f003cf88a0151"),
            Hash256.Parse("a2198c4f6b03a76aaea0f8c174de28358eb6affd66471d91ad7d0e0115bb36c1"),
        };

        public static readonly int Block124009_SerializedMessageOffset = 24;

        public static byte[] Block124009_SerializedMessage
        {
            get
            {
                return GetSerializedData("MsgBlock124009.bin");
            }
        }

        #endregion
        #region Block 124010

        public static readonly Hash256 Block124010_Hash = Hash256.Parse("0000000000003a8b34e22c6e7b4934993d91261fe0f7e0f2940c6abd8bfae156");

        public static readonly Block Block124010_Header = new Block
        {
            Version = 1,
            PreviousBlockHash = Hash256.Parse("0000000000003a2bf9728419cd10d1c68493c99f275242db8e7df2ee9079e0ed"),
            MerkleRoot = Hash256.Parse("b8694ee1b40d0802df4755f66f662dee380f4ad5a093d48e0e12ab730af92751"),
            Timestamp = 1305422821,
            DifficultyBits = 443192243, //0x1a6a93b3
            Nonce = 3984824426,
        };

        public static readonly Hash256[] Block124010_MerkleTree = new[]
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

        public static readonly int Block124010_SerializedMessageOffset = 24;

        public static byte[] Block124010_SerializedMessage
        {
            get
            {
                return GetSerializedData("MsgBlock124010.bin");
            }
        }

        #endregion

        private static byte[] GetSerializedData(string dataResourceName)
        {
            dataResourceName = Assembly.GetExecutingAssembly().GetName().Name + ".SerializedData." + dataResourceName;
            using (var resStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(dataResourceName))
            {
                if (resStream == null)
                {
                    DiagnosticLog.WriteLine("Unable to find for: {0} among:", dataResourceName);
                    DiagnosticLog.WriteLine(string.Join("\n", System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames()));
                    return null;
                }
                using (var reader = new System.IO.BinaryReader(resStream))
                {
                    return reader.ReadBytes((int)reader.BaseStream.Length);
                }
            }
        }

        static Blocks()
        {
            Block000000.Freeze();
            Block000000_Header.Freeze();
            Block072783.Freeze();
            Block072783_Header.Freeze();
            Block072785.Freeze();
            Block072785_Header.Freeze();
            Block103640.Freeze();
            Block103640_Header.Freeze();
            Block103958.Freeze();
            Block103958_Header.Freeze();
            Block124009_Header.Freeze();
            Block124010_Header.Freeze();
        }
    }
}
