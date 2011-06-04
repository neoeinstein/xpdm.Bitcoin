using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Tests.Factories.Core.Blocks
{
    class B072785 : TestBlock
    {
        private static B072785 _instance = new B072785();
        public static B072785 Instance { get { return _instance; } }

        private B072785()
        {
            base.ExpectedHash = Hash256.Parse("00000000009ffdadbb2a8bcf8e8b1d68e1696802856c6a1d61561b1f630e79e7");
            base.Header = new BlockHeader
            {
                Version = 1,
                PreviousBlockHash = Hash256.Parse("0000000000b079382c19436fe8e076e199f0faa9014a930caf89d15744d9aec7"),
                MerkleRoot = Hash256.Parse("e81287dc0c00422aaf0db3e4586c48b01acd82b3108da6956cbd6baf19cfaf9a"),
                Timestamp = 1281156783,
                DifficultyBits = 469809688, //0x1c00ba18
                Nonce = 2283211008,
            };
            base.Header.Freeze();
            base.Block = new Block
            {
                Header = new BlockHeader
                    {
                        Version = 1,
                        PreviousBlockHash = Hash256.Parse("0000000000b079382c19436fe8e076e199f0faa9014a930caf89d15744d9aec7"),
                        Timestamp = 1281156783,
                        DifficultyBits = 469809688, //0x1c00ba18
                        Nonce = 2283211008,
                    },
                Transactions =
                {
                    B072785.Coinbase,
                    B072785.Tx1,
                    B072785.Tx2,
                    B072785.Tx3,
                    B072785.Tx4,
                    B072785.Tx5,
                },
            };
            base.Block.Freeze();
            base.ExpectedMerkleTree = new[]
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
        }
        public static Transaction Coinbase
        {
            get { return TransactionData.Instance["2d7f4d1c25893dcaf538fdd1f34104687211ca7d8a1ba43c16b618d5fbc620c3"].Transaction; }
        }

        public static Transaction Tx1
        {
            get { return TransactionData.Instance["3407a84dce0fe04fdab91608d1974941af3683ea6e4d904a30469485c50d336a"].Transaction; }
        }

        public static Transaction Tx2
        {
            get { return TransactionData.Instance["5edf5acf8f517d965219a5495321e0bedd761daf45bcdc59a33b07b520968b8c"].Transaction; }
        }

        public static Transaction Tx3
        {
            get { return TransactionData.Instance["65c35615b476c86f28a4d3a8985ea161cc2e35e6574eacbd68942782ce29804c"].Transaction; }
        }

        public static Transaction Tx4
        {
            get { return TransactionData.Instance["89aa32f6e1b047e740401ce4fd43a865631de5a959fde7451936c28c52249b56"].Transaction; }
        }

        public static Transaction Tx5
        {
            get { return TransactionData.Instance["e3e69c802b7e36d220151e4ccdeace1d58ca2af97c5fd970314bbecd9767a514"].Transaction; }
        }
    }
}
