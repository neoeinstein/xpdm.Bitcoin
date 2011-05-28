using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Tests.Factories.Core.Transactions
{
    static class B072783
    {
        #region Coinbase

        public static TestTx Coinbase = new TestTx
        {
            ExpectedHash = Hash256.Parse("9f92327b5a71d5acf608974fd8bbf16abc948ae21f934a2a5a7c841c499b5f92"),
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
                                    Script = Script.Parse("18ba001c f304"),
                                },
                        },
                    TransactionOutputs =
                        {
                            new TransactionOutput
                                {
                                    Value = BitcoinValue.FromWholeCoins(50.0m),
                                    Script = Script.Parse("04e94c7cf0b210d00cdaaf14670f806bf675e0d55dbf6d0f4faab7106ac59ea3326d0d6fa3b91ed2b03cd79534db1dd348ed301944e5b03149e13206d9bb240447 OP_CHECKSIG"),
                                },
                        }
                },
        };

        #endregion
        #region Tx 1

        public static TestTx Tx1 = new TestTx
        {
            ExpectedHash = Hash256.Parse("b77e0fc6d275c951342a33473015937e62b25a68538d78a260f1225b2835a283"),
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
                                        SourceTransactionHash = Hash256.Parse("eee857c8143bed326d0ac47ff9782d8d4aac4a931a346b5561cc7c324d34760b"),
                                        OutputSequenceNumber = 1,
                                    },
                                    Script = Script.Parse("304502206a84fa7c46d35733d2bcd25b372eaac9a42b99355b218e29959d73d4bb4f67ce022100915992d617dcdf0efd477df24043b483f510d21106522e450cace41ed55ee80901 046f54677bcf8d96d41c9c957ba4388113838c9b21be9d04d199fec82ec4b3466f1e44649a85f744e2f4adda4a9bd74bca8104cd02b64eec4e682b489271983729"),
                                },
                        },
                    TransactionOutputs =
                        {
                            new TransactionOutput
                                {
                                    Value = BitcoinValue.FromWholeCoins(350.0m),
                                    Script = Script.Parse("OP_DUP OP_HASH160 fb66e6a692e08ce1fb6a4edfc017dcdec3a28a3e OP_EQUALVERIFY OP_CHECKSIG"),
                                },
                            new TransactionOutput
                                {
                                    Value = BitcoinValue.FromWholeCoins(150.0m),
                                    Script = Script.Parse("OP_DUP OP_HASH160 d29c76892d626b8b9a0080b0f63e913a42a9dd0e OP_EQUALVERIFY OP_CHECKSIG"),
                                },
                        }
                },
        };

        #endregion

        static B072783()
        {
            Coinbase.Transaction.Freeze();
            Tx1.Transaction.Freeze();
        }
    }
}
