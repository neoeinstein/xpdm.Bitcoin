using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Tests.Factories.Core
{
    public static partial class Transactions
    {
        public static class Block000000
        {
            #region Coinbase

            public static readonly Hash256 Tx0_Hash = Hash256.Parse("4a5e1e4baab89f3a32518a88c31bc87f618f76673e2cc77ab2127b7afdeda33b");

            public static readonly Transaction Tx0 = new Transaction
            {
                Version = 1,
                LockTime = 0,
                TransactionInputs =
                    {
                        new TransactionInput
                            {
                                Source = TransactionOutpoint.Coinbase,
                                Script = new Script(BufferOperations.FromByteString("4d04ffff001d0104455468652054696d65732030332f4a616e2f32303039204368616e63656c6c6f72206f6e206272696e6b206f66207365636f6e64206261696c6f757420666f722062616e6b73", Endianness.BigEndian), 0),
                            },
                    },
                TransactionOutputs =
                    {
                        new TransactionOutput
                            {
                                Value = BitcoinValue.FromWholeCoins(50.0m),
                                Script = Script.Parse("04678afdb0fe5548271967f1a67130b7105cd6a828e03909a67962e0ea1f61deb649f6bc3f4cef38c4f35504e51ec112de5c384df7ba0b8d578a4c702b6bf11d5f OP_CHECKSIG"),
                            },
                    }
            };

            #endregion

            static Block000000()
            {
                Tx0.Freeze();
            }
        }
    }
}
