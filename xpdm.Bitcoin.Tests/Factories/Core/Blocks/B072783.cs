using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Tests.Factories.Core.Blocks
{
    class B072783 : TestBlock
    {
        private static B072783 _instance = new B072783();
        public static B072783 Instance { get { return _instance; } }

        private B072783()
        {
            base.ExpectedHash = Hash256.Parse("000000000074672e28f2049c94c5d7fe3b20753f0ed6a8aa168e1c103cc48388");
            base.Header = new Block
            {
                Version = 1,
                PreviousBlockHash = Hash256.Parse("0000000000b94dd60e9a8fabb3d6b3a820486a566ed260855583a17f1ba18a29"),
                MerkleRoot = Hash256.Parse("bdc105236f133fb9fd8ef55fc0247bf46bb400e9d7b8b2e925b16dea931bce4f"),
                Timestamp = 1281155353,
                DifficultyBits = 469809688, //0x1c00ba18
                Nonce = 1000411397,
            };
            base.Header.Freeze();
            base.Block = new Block
            {
                Version = 1,
                PreviousBlockHash = Hash256.Parse("0000000000b94dd60e9a8fabb3d6b3a820486a566ed260855583a17f1ba18a29"),
                Timestamp = 1281155353,
                DifficultyBits = 469809688, //0x1c00ba18
                Nonce = 1000411397,
                Transactions =
                {
                    B072783.Coinbase,
                    B072783.Tx1,
                },
            };
            base.Block.Freeze();
            base.ExpectedMerkleTree = new[]
            {
                Hash256.Parse("9f92327b5a71d5acf608974fd8bbf16abc948ae21f934a2a5a7c841c499b5f92"),
                Hash256.Parse("b77e0fc6d275c951342a33473015937e62b25a68538d78a260f1225b2835a283"),
                Hash256.Parse("bdc105236f133fb9fd8ef55fc0247bf46bb400e9d7b8b2e925b16dea931bce4f"),
            };
        }
        public static Transaction Coinbase
        {
            get { return TransactionData.Instance["9f92327b5a71d5acf608974fd8bbf16abc948ae21f934a2a5a7c841c499b5f92"].Transaction; }
        }

        public static Transaction Tx1
        {
            get { return TransactionData.Instance["b77e0fc6d275c951342a33473015937e62b25a68538d78a260f1225b2835a283"].Transaction; }
        }
    }
}
