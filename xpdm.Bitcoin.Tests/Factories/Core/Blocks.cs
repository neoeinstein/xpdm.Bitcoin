
using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Tests.Factories.Core
{
    public static class Blocks
    {
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
            DifficultyBits = 469809688, //0x1b038dee
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
    }
}
