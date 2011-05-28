using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Tests.Factories.Core.Transactions
{
    class B000170
    {
        #region Coinbase

        public static TestTx Coinbase = new TestTx
        {
            ExpectedHash = Hash256.Parse("b1fea52486ce0c62bb442b530a3f0132b826c74e473d1f2c220bfa78111c5082"),
            ExpectedCoinbase = true,
            Transaction = new Transaction
                {
                    Version = 1,
                    LockTime = 0,
                    TransactionInputs =
                        {
                            new TransactionInput
                                {
                                    Source = TransactionOutpoint.Coinbase,
                                    Script = Script.Parse("ffff001d 02"),
                                },
                        },
                    TransactionOutputs =
                        {
                            new TransactionOutput
                                {
                                    Value = BitcoinValue.FromWholeCoins(50.0m),
                                    Script = Script.Parse("04d46c4968bde02899d2aa0963367c7a6ce34eec332b32e42e5f3407e052d64ac625da6f0718e7b302140434bd725706957c092db53805b821a85b23a7ac61725b OP_CHECKSIG"),
                                },
                        }
                },
        };

        #endregion
        #region Tx 1

        public static TestTx Tx1 = new TestTx
        {
            ExpectedHash = Hash256.Parse("f4184fc596403b9d638783cf57adfe4c75c605f6356fbc91338530e9831e9e16"),
            Transaction = new Transaction
            {
                Version = 1,
                LockTime = 0,
                TransactionInputs =
                        {
                            new TransactionInput
                                {
                                    Source = new TransactionOutpoint
                                    {
                                        SourceTransactionHash = Hash256.Parse("0437cd7f8525ceed2324359c2d0ba26006d92d856a9c20fa0241106ee5a597c9"),
                                        OutputSequenceNumber = 0,
                                    },
                                    Script = Script.Parse("304402204e45e16932b8af514961a1d3a1a25fdf3f4f7732e9d624c6c61548ab5fb8cd410220181522ec8eca07de4860a4acdd12909d831cc56cbbac4622082221a8768d1d0901"),
                                },
                        },
                TransactionOutputs =
                        {
                            new TransactionOutput
                                {
                                    Value = BitcoinValue.FromWholeCoins(10.0m),
                                    Script = Script.Parse("04ae1a62fe09c5f51b13905f07f06b99a2f7159b2225f374cd378d71302fa28414e7aab37397f554a7df5f142c21c1b7303b8a0626f1baded5c72a704f7e6cd84c OP_CHECKSIG"),
                                },
                            new TransactionOutput
                                {
                                    Value = BitcoinValue.FromWholeCoins(40.0m),
                                    Script = Script.Parse("0411db93e1dcdb8a016b49840f8c53bc1eb68a382e97b1482ecad7b148a6909a5cb2e0eaddfb84ccf9744464f82e160bfa9b8b64f9d4c03f999b8643f656b412a3 OP_CHECKSIG"),
                                },
                        }
            },
        };

        #endregion

        static B000170()
        {
            Coinbase.Transaction.Freeze();
        }

    }
}
