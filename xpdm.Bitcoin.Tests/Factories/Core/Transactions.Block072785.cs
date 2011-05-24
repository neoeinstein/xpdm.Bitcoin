using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Tests.Factories.Core
{
    public static partial class Transactions
    {
        public static class Block072785
        {
            #region Coinbase

            public static readonly Hash256 Tx0_Hash = Hash256.Parse("2d7f4d1c25893dcaf538fdd1f34104687211ca7d8a1ba43c16b618d5fbc620c3");

            public static readonly Transaction Tx0 = new Transaction
            {
                Version = 1,
                LockTime = 0,
                TransactionInputs =
                    {
                        new TransactionInput
                            {
                                Source = TransactionOutpoint.Coinbase,
                                Script = Script.Parse("1c00ba18 03ce"),
                            },
                    },
                TransactionOutputs =
                    {
                        new TransactionOutput
                            {
                                Value = 5000000000,
                                Script = Script.Parse("04ba124cb5a347e8f73593a6331651b788113c5cb80e72b965223e134c4e80a825081b374d58507822fc46b8825f63f76fd0df42451e26f011a6ae88f9721de14f OP_CHECKSIG"),
                            },
                    }
            };

            #endregion
            #region Tx 1

            public static readonly Hash256 Tx1_Hash = Hash256.Parse("3407a84dce0fe04fdab91608d1974941af3683ea6e4d904a30469485c50d336a");

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
                                    SourceTransactionHash = Hash256.Parse("df39bffe852f56b35f068287fa7e615777c52aac267f35e161235858dce62d37"),
                                    OutputSequenceNumber = 0,
                                },
                                Script = Script.Parse("3045022067fdfc854e8d1e35c678ce1cd20ce7c684a4c45f2b0525f7c0870840c0161127022100e6696d20ce506775abb262187ff4e332e8eedf68908483e9705be6efe9d81add01 04ec17406f1c2a39644c90d0c8142e80f022d10b7271403279f9e7c76917be14cae260d506f2185651a4e00d51315fce80b3e9ad3cab4e11fa22900c54f3d66f3e"),
                            },
                        new TransactionInput
                            {
                                Source = new TransactionOutpoint
                                {
                                    SourceTransactionHash = Hash256.Parse("64ebeacd2fc7e71f46d7066482377bf974c2c5c9fd0ebbaf5afaad701f4d5edc"),
                                    OutputSequenceNumber = 1,
                                },
                                Script = Script.Parse("3045022100b63c97455d05ce1642c0117e204d5dc6a7d764a880cade6432f4c1b11fe87d010220290d1b9eed0faff8d2af5c6a3dd25c5b8e69c6e15476c4d96562a13b87f9a16501 0435bcbce319ac7205a6240ce8fc3cdccd07316f992e9e93f9a85d3c193b9142ccba82d25c3aa1b8c543078bff6330df75523198cd8ce362c862e589a9eabc9000"),
                            },
                    },
                TransactionOutputs =
                    {
                        new TransactionOutput
                            {
                                Value = BitcoinValue.FromWholeCoins(0.06m),
                                Script = Script.Parse("OP_DUP OP_HASH160 726b4be78fc438098c7123e08e9d3903e43eefed OP_EQUALVERIFY OP_CHECKSIG"),
                            },
                    }
            };

            #endregion
            #region Tx 2

            public static readonly Hash256 Tx2_Hash = Hash256.Parse("5edf5acf8f517d965219a5495321e0bedd761daf45bcdc59a33b07b520968b8c");

            public static readonly Transaction Tx2 = new Transaction
            {
                Version = 1,
                LockTime = 0,
                TransactionInputs =
                    {
                        new TransactionInput
                            {
                                Source = new TransactionOutpoint
                                {
                                    SourceTransactionHash = Hash256.Parse("b77e0fc6d275c951342a33473015937e62b25a68538d78a260f1225b2835a283"),
                                    OutputSequenceNumber = 0,
                                },
                                Script = Script.Parse("304502206ed3eaf7318179866b857b2d9f84dca62d3ad65a50db34efc2bb56db0c56554002210084d619f0226f4afe6ae88f72e4cf7830cd497d3b7f1f3ae502beb9c1af539ce301 04fb1b0fbd4fbaee7cd2293ed005b91538aa8e34b6404851c0c119276b518d1c3454e698c1545591a9daebf0ae663c804c29b79c8ddc9a76d05832990968fcb564"),
                            },
                    },
                TransactionOutputs =
                    {
                        new TransactionOutput
                            {
                                Value = BitcoinValue.FromWholeCoins(350m),
                                Script = Script.Parse("OP_DUP OP_HASH160 a3aef8b533b30b7c22d7ef527c65f5fe41e3efd7 OP_EQUALVERIFY OP_CHECKSIG"),
                            },
                    }
            };

            #endregion
            #region Tx 3

            public static readonly Hash256 Tx3_Hash = Hash256.Parse("65c35615b476c86f28a4d3a8985ea161cc2e35e6574eacbd68942782ce29804c");

            public static readonly Transaction Tx3 = new Transaction
            {
                Version = 1,
                LockTime = 0,
                TransactionInputs =
                    {
                        new TransactionInput
                            {
                                Source = new TransactionOutpoint
                                {
                                    SourceTransactionHash = Hash256.Parse("893335cda097cc79f9b7273dba1199c514dd73d0fe5be02fce9588cc71089761"),
                                    OutputSequenceNumber = 0,
                                },
                                Script = Script.Parse("3045022100dfaef4f49c13e37ccdc8eda697adcd94ee989342d75a6cf4c60ba3033ba642e20220140612f6fbfcc737d9cb899c664ef73e025d152b5aa1a2dea0dff058fd15c3b801 04c3792f44adfabaad7334a65d914356739dfece4412f95abc44a2f9c5d4740a2e1f1896661df9929bd8d6c2c682deb05133a2517e41a7740d8d927e9eb9d8d1d1"),
                            },
                    },
                TransactionOutputs =
                    {
                        new TransactionOutput
                            {
                                Value = BitcoinValue.FromWholeCoins(1.8585m),
                                Script = Script.Parse("OP_DUP OP_HASH160 2cff865721ada119912e5b759708421a5be08131 OP_EQUALVERIFY OP_CHECKSIG"),
                            },
                        new TransactionOutput
                            {
                                Value = BitcoinValue.FromWholeCoins(3.1415m),
                                Script = Script.Parse("OP_DUP OP_HASH160 f2adab04bce76bcd2143888c6533285538ce9ef9 OP_EQUALVERIFY OP_CHECKSIG"),
                            },
                    }
            };

            #endregion
            #region Tx 4

            public static readonly Hash256 Tx4_Hash = Hash256.Parse("89aa32f6e1b047e740401ce4fd43a865631de5a959fde7451936c28c52249b56");

            public static readonly Transaction Tx4 = new Transaction
            {
                Version = 1,
                LockTime = 0,
                TransactionInputs =
                    {
                        new TransactionInput
                            {
                                Source = new TransactionOutpoint
                                {
                                    SourceTransactionHash = Hash256.Parse("4a7469b5e41059ec68c99bc84e6e49f55fc6ddc0a05ed08056131850d4437c07"),
                                    OutputSequenceNumber = 0,
                                },
                                Script = Script.Parse("3046022100cfa125788f1a8ca336d74de7f793f8a6bcd43b027cecb534a430b10ad33df5b2022100ef4a589737b9f28f9d9c04806addb72d984344e19ff8cf91d3114bce9d19150d01 043fd703e89a72c0bf8caf24fd03f0772580cf48107349fd1f2d28ef74998a009d79bb8ee8bc526ce94ae0058f7a3ea98595808c4a2b0c1d28944d7b364a9cfb5c"),
                            },
                    },
                TransactionOutputs =
                    {
                        new TransactionOutput
                            {
                                Value = BitcoinValue.FromWholeCoins(0.05m),
                                Script = Script.Parse("OP_DUP OP_HASH160 97cecf4269b89b9c75601dff474350e7e1830356 OP_EQUALVERIFY OP_CHECKSIG"),
                            },
                        new TransactionOutput
                            {
                                Value = BitcoinValue.FromWholeCoins(0.20m),
                                Script = Script.Parse("OP_DUP OP_HASH160 19fbaa87a9df466ee8f37a05c42413866e337460 OP_EQUALVERIFY OP_CHECKSIG"),
                            },
                    }
            };

            #endregion
            #region Tx 5

            public static readonly Hash256 Tx5_Hash = Hash256.Parse("e3e69c802b7e36d220151e4ccdeace1d58ca2af97c5fd970314bbecd9767a514");

            public static readonly Transaction Tx5 = new Transaction
            {
                Version = 1,
                LockTime = 0,
                TransactionInputs =
                    {
                        new TransactionInput
                            {
                                Source = new TransactionOutpoint
                                {
                                    SourceTransactionHash = Hash256.Parse("b77e0fc6d275c951342a33473015937e62b25a68538d78a260f1225b2835a283"),
                                    OutputSequenceNumber = 0,
                                },
                                Script = Script.Parse("3045022100ade09f7f9c884906899d359dc66407723390382bc5582f70e78e4df8faea7edf02206528f1e2de03733d397beaf8dab9fc81df09400f09a5c3e05502186412af182e01 0499c0b4ce36a491bba7afa0b25408ca3814480feffa3266c9de323fa8f9d3eacd47f534ef2d61e482392825d9932d2d9f976933742cd9f52bc922c21e1d6d5a08"),
                            },
                    },
                TransactionOutputs =
                    {
                        new TransactionOutput
                            {
                                Value = BitcoinValue.FromWholeCoins(50.0m),
                                Script = Script.Parse("OP_DUP OP_HASH160 7a8073c0e7a50c615c16b2237768012c3c0b8e45 OP_EQUALVERIFY OP_CHECKSIG"),
                            },
                        new TransactionOutput
                            {
                                Value = BitcoinValue.FromWholeCoins(100.0m),
                                Script = Script.Parse("OP_DUP OP_HASH160 1ea7068f4c04814ffddc27730df2224eb9fdef9e OP_EQUALVERIFY OP_CHECKSIG"),
                            },
                    }
            };

            #endregion

            static Block072785()
            {
                Tx0.Freeze();
                Tx1.Freeze();
                Tx2.Freeze();
                Tx3.Freeze();
                Tx4.Freeze();
                Tx5.Freeze();
            }
        }
    }
}
