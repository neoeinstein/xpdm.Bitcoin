using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Tests.Factories.Core.Blocks
{
    class B103640 : TestBlock
    {
        private static B103640 _instance = new B103640();
        public static B103640 Instance { get { return _instance; } }

        private B103640()
        {
            base.ExpectedHash = Hash256.Parse("0000000000003702793ba6bf5a085eccee1ec9b249f6ff42063b34980e7cf028");
            base.Header = new Block
            {
                Version = 1,
                PreviousBlockHash = Hash256.Parse("000000000000111a11fcee10f2b46210aacc2e2e4c7dd1d1924b2f6f03e8316f"),
                MerkleRoot = Hash256.Parse("63a37fe141ab610d471067f75591f291c4d6d4333e3213363f06f187fce04e82"),
                Timestamp = 1295526788,
                DifficultyBits = 453217774, //0x1b038dee
                Nonce = 3592101678,
            };
            base.Header.Freeze();
            base.Block = new Block
            {
                Version = 1,
                PreviousBlockHash = Hash256.Parse("000000000000111a11fcee10f2b46210aacc2e2e4c7dd1d1924b2f6f03e8316f"),
                Timestamp = 1295526788,
                DifficultyBits = 453217774, //0x1b038dee
                Nonce = 3592101678,
                Transactions =
                    {
                        B103640.Coinbase,
                        B103640.Tx1,
                    },
            };
            base.Block.Freeze();
            ExpectedMerkleTree = new[]
            {
                Hash256.Parse("cdd3cf10fe83c28d1ac80714a48440e47f1c6b6950fec54f6b8ea04241004cb5"),
                Hash256.Parse("945691940e0ccd9f526ee1edd57a77ce170804915749702f5564c49b1f70f330"),
                Hash256.Parse("63a37fe141ab610d471067f75591f291c4d6d4333e3213363f06f187fce04e82"),
            };
        }

        public static Transaction Coinbase
        {
            get { return TransactionData.Instance["cdd3cf10fe83c28d1ac80714a48440e47f1c6b6950fec54f6b8ea04241004cb5"].Transaction; }
        }

        public static Transaction Tx1
        {
            get { return TransactionData.Instance["945691940e0ccd9f526ee1edd57a77ce170804915749702f5564c49b1f70f330"].Transaction; }
        }
    }
}
