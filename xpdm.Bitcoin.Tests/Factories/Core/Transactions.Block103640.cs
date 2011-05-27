using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Tests.Factories.Core
{
    public static partial class Transactions
    {
        public static class Block103640
        {
            #region Coinbase

            public static readonly Hash256 Tx0_Hash = Hash256.Parse("cdd3cf10fe83c28d1ac80714a48440e47f1c6b6950fec54f6b8ea04241004cb5");

            public static readonly Transaction Tx0 = new Transaction
            {
                Version = 1,
                LockTime = 0,
                TransactionInputs =
                    {
                        new TransactionInput
                            {
                                Source = TransactionOutpoint.Coinbase,
                                Script = Script.Parse("ee8d031b 6031010001"),
                            },
                    },
                TransactionOutputs =
                    {
                        new TransactionOutput
                            {
                                Value = BitcoinValue.FromWholeCoins(50.0m),
                                Script = Script.Parse("04b6d446f48f4c9f1426ac051ade817afe4c9c261b5061c398adf5d59c8b9b588677837c4f2eab1d4ff22203abdaf5457040241c88187d168feb9c422dd7c9e789 OP_CHECKSIG"),
                            },
                    }
            };

            #endregion
            #region Tx 1

            public static readonly Hash256 Tx1_Hash = Hash256.Parse("945691940e0ccd9f526ee1edd57a77ce170804915749702f5564c49b1f70f330");

            public static readonly Transaction Tx1 = new Transaction
            {
                Version = 1,
                LockTime = 0,
                TransactionInputs =
                    {
                        new TransactionInput
                            {
                                Source = new TransactionOutpoint
                                {
                                    SourceTransactionHash = Hash256.Parse("916eb104bdc5ab68742a9bd16e7379df3f1807b0d3418e0546f64c6db87f7b0f"),
                                    OutputSequenceNumber = 0,
                                },
                                Script = Script.Parse("3046022100b2ee39d2fcc2e5544a57c30f7b4e49cfb82222666d034fb90e22348e17e28e0f022100db91c3199cc7b41d4d7afce0ccb4ceb424b9476d51c06142583daf53ce0a9b6601 04c32215a9093011bd3c41283ace3d002c666077b24a605b3cfc8f71019a0f43df66f389f3d9a62188a494b869dc7e5f9dffc98a76d3088a21e9b738ec9eba98cb"),
                            },
                        new TransactionInput
                            {
                                Source = new TransactionOutpoint
                                {
                                    SourceTransactionHash = Hash256.Parse("4949c13fa4bca0d5417670a3e6f315b8c021e0aaca6534d35e7b8f5225410097"),
                                    OutputSequenceNumber = 1,
                                },
                                Script = Script.Parse("3044022033d02c2e896f1a1252488d534cfb08abf3e7ea90aba7ba6f57abf189cef1d837022005668d755013b0e59a2af5145f10efe62ea716d333268b0b5a3efbd82d1439be01 04c32215a9093011bd3c41283ace3d002c666077b24a605b3cfc8f71019a0f43df66f389f3d9a62188a494b869dc7e5f9dffc98a76d3088a21e9b738ec9eba98cb"),
                            },
                    },
                TransactionOutputs =
                    {
                        new TransactionOutput
                            {
                                Value = BitcoinValue.FromWholeCoins(2.0m),
                                Script = Script.Parse("OP_DUP OP_HASH160 02bf4b2889c6ada8190c252e70bde1a1909f9617 OP_EQUALVERIFY OP_CHECKSIG"),
                            },
                    }
            };

            public static readonly byte[] Block103640_Tx1_Out0_MinimalForSignatureVerificationSource = new byte[] {
                0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0xc2, 0xeb, 0x0b, 0x00, 0x00, 0x00, 0x00, 0x19, 0x76,
                0xa9, 0x14, 0x02, 0xbf, 0x4b, 0x28, 0x89, 0xc6, 0xad, 0xa8, 0x19, 0x0c, 0x25, 0x2e, 0x70, 0xbd,
                0xe1, 0xa1, 0x90, 0x9f, 0x96, 0x17, 0x88, 0xac, 0x00, 0x00, 0x00, 0x00 };

            #endregion

            static Block103640()
            {
                Tx0.Freeze();
                Tx1.Freeze();
            }
        }
    }
}
