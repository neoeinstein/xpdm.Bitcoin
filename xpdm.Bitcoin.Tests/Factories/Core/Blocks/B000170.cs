using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Tests.Factories.Core.Blocks
{
    class B000170 : TestBlock
    {
        private static B000170 _instance = new B000170();
        public static B000170 Instance { get { return _instance; } }

        private B000170()
        {
            base.ExpectedHash = Hash256.Parse("00000000d1145790a8694403d4063f323d499e655c83426834d4ce2f8dd4a2ee");
            base.Header = new Block
            {
                Version = 1,
                PreviousBlockHash = Hash256.Parse("000000002a22cfee1f2c846adbd12b3e183d4f97683f85dad08a79780a84bd55"),
                MerkleRoot = Hash256.Parse("7dac2c5666815c17a3b36427de37bb9d2e2c5ccec3f8633eb91a4205cb4c10ff"),
                Timestamp = 1231731025,
                DifficultyBits = 486604799, //0x1d00ffff
                Nonce = 1889418792,
            };
            base.Header.Freeze();
            base.Block = new Block
            {
                Version = 1,
                PreviousBlockHash = Hash256.Parse("000000002a22cfee1f2c846adbd12b3e183d4f97683f85dad08a79780a84bd55"),
                Timestamp = 1231731025,
                DifficultyBits = 486604799, //0x1d00ffff
                Nonce = 1889418792,
                Transactions =
                {
                    B000170.Coinbase,
                    B000170.Tx1,
                },
            };
            base.Block.Freeze();
            base.ExpectedMerkleTree = new[]
            {
                Hash256.Parse("b1fea52486ce0c62bb442b530a3f0132b826c74e473d1f2c220bfa78111c5082"),
                Hash256.Parse("f4184fc596403b9d638783cf57adfe4c75c605f6356fbc91338530e9831e9e16"),
                Hash256.Parse("7dac2c5666815c17a3b36427de37bb9d2e2c5ccec3f8633eb91a4205cb4c10ff"),
            };
        }

        public static Transaction Coinbase
        {
            get { return TransactionData.Instance["b1fea52486ce0c62bb442b530a3f0132b826c74e473d1f2c220bfa78111c5082"].Transaction; }
        }

        public static Transaction Tx1
        {
            get { return TransactionData.Instance["f4184fc596403b9d638783cf57adfe4c75c605f6356fbc91338530e9831e9e16"].Transaction; }
        }
    }
}
