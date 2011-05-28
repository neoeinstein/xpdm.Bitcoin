using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Tests.Factories.Core.Blocks
{
    sealed class B000000 : TestBlock
    {
        private static B000000 _instance = new B000000();
        public static B000000 Instance { get { return _instance; } }

        private B000000()
        {
            base.ExpectedHash = Hash256.Parse("000000000019d6689c085ae165831e934ff763ae46a2a6c172b3f1b60a8ce26f");
            base.Header = new Block
            {
                Version = 1,
                PreviousBlockHash = Hash256.Empty,
                MerkleRoot = Hash256.Parse("4a5e1e4baab89f3a32518a88c31bc87f618f76673e2cc77ab2127b7afdeda33b"),
                Timestamp = 1231006505,
                DifficultyBits = 486604799, //0x1d00ffff
                Nonce = 2083236893,
            };
            base.Header.Freeze();
            base.Block = new Block
            {
                Version = 1,
                PreviousBlockHash = Hash256.Empty,
                Timestamp = 1231006505,
                DifficultyBits = 486604799, //0x1d00ffff
                Nonce = 2083236893,
                Transactions =
                    {
                        B000000.Coinbase,
                    },
            };
            base.Block.Freeze();
            base.ExpectedMerkleTree = new[]
            {
                Hash256.Parse("4a5e1e4baab89f3a32518a88c31bc87f618f76673e2cc77ab2127b7afdeda33b"),
            };
        }

        public static Transaction Coinbase
        {
            get { return TransactionData.Instance["4a5e1e4baab89f3a32518a88c31bc87f618f76673e2cc77ab2127b7afdeda33b"].Transaction; }
        }
    }
}
