﻿using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Tests.Factories.Core
{
    public static partial class Transactions
    {
        public static class Block103958
        {
            #region Coinbase

            public static readonly Hash256 Tx0_Hash = Hash256.Parse("f2dedefe5222786abd6cb2223c89e50d590f234dbba69002d5ad5cd6e438abfd");

            public static readonly Transaction Tx0 = new Transaction
            {
                Version = 1,
                LockTime = 0,
                TransactionInputs =
                    {
                        new TransactionInput
                            {
                                Source = TransactionOutpoint.Coinbase,
                                Script = Script.Parse("1b038dee 39"),
                            },
                    },
                TransactionOutputs =
                    {
                        new TransactionOutput
                            {
                                Value = 5000000000,
                                Script = Script.Parse("044e65470b43854073a647bb88b337e1c4f0ea7553147805715674888a0030fe4c6dd5afec5317919fbab57cc3abfd11168ebdcce469acb5c1db345e8d79269302 OP_CHECKSIG"),
                            },
                    }
            };

            #endregion
            #region Tx 1

            public static readonly Hash256 Tx1_Hash = Hash256.Parse("ff954e099764d192c5bb531c9c14c18c230b0c0a63f02cd168a4ea94548c890f");

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
                                        SourceTransactionHash = Hash256.Parse("945691940e0ccd9f526ee1edd57a77ce170804915749702f5564c49b1f70f330"),
                                        OutputSequenceNumber = 0,
                                    },
                                Script = Script.Parse("3046022100f5746b0b254f5a37e75251459c7a23b6dfcb868ac7467edd9a6fdd1d969871be02210088948aea29b69161ca341c49c02686a81d8cbb73940f917fa0ed7154686d3e5b01 0447d490561f396c8a9efc14486bc198884ba18379bcac2e0be2d8525134ab742f301a9aca36606e5d29aa238a9e2993003150423df6924563642d4afe9bf4fe28"),
                            },
                        new TransactionInput
                            {
                                Source = new TransactionOutpoint
                                    {
                                        SourceTransactionHash = Hash256.Parse("89a68eb0a3e6c158592d98802c0cb69bd5b9bf6533b7e56d2ce96c68f72b1472"),
                                        OutputSequenceNumber = 0,
                                    },
                                Script = Script.Parse("3046022100bce43ad3acbc79b0247e54c8c91eac1cf9037505000e01d1fd811854d85bc21a022100992a6f6f2feb6f62d3706f3b9aaab88d9f1132956a1dffa926cd556ed55360df01"),
                            },
                        new TransactionInput
                            {
                                Source = new TransactionOutpoint
                                    {
                                        SourceTransactionHash = Hash256.Parse("2c631bfa782cc627b1daa119ac56ea7b7edc19c60c630a3d1c7c20b6bb2881d2"),
                                        OutputSequenceNumber = 0,
                                    },
                                Script = Script.Parse("30450220209757368161537708fd29d89bb1e9d648007949ecfded789b51a96324cb6518022100cd0f7c30213916482b6e166d8a4f2b981f777eb184cd8a495f1b3d3690fbbf2d01"),
                            },
                    },
                    TransactionOutputs =
                    {
                        new TransactionOutput
                            {
                                Value = 10200000000,
                                Script = Script.Parse("OP_DUP OP_HASH160 9e35d93c7792bdcaad5697ddebf04353d9a5e196 OP_EQUALVERIFY OP_CHECKSIG"),
                            },
                    }
                };

            public static readonly byte[] Tx1_Serialized = new byte[] {
            0x01, 0x00, 0x00, 0x00, 0x03, 0x30, 0xf3, 0x70, 0x1f, 0x9b, 0xc4, 0x64, 0x55, 0x2f, 0x70, 0x49, 
            0x57, 0x91, 0x04, 0x08, 0x17, 0xce, 0x77, 0x7a, 0xd5, 0xed, 0xe1, 0x6e, 0x52, 0x9f, 0xcd, 0x0c, 
            0x0e, 0x94, 0x91, 0x56, 0x94, 0x00, 0x00, 0x00, 0x00, 0x8c, 0x49, 0x30, 0x46, 0x02, 0x21, 0x00, 
            0xf5, 0x74, 0x6b, 0x0b, 0x25, 0x4f, 0x5a, 0x37, 0xe7, 0x52, 0x51, 0x45, 0x9c, 0x7a, 0x23, 0xb6, 
            0xdf, 0xcb, 0x86, 0x8a, 0xc7, 0x46, 0x7e, 0xdd, 0x9a, 0x6f, 0xdd, 0x1d, 0x96, 0x98, 0x71, 0xbe, 
            0x02, 0x21, 0x00, 0x88, 0x94, 0x8a, 0xea, 0x29, 0xb6, 0x91, 0x61, 0xca, 0x34, 0x1c, 0x49, 0xc0, 
            0x26, 0x86, 0xa8, 0x1d, 0x8c, 0xbb, 0x73, 0x94, 0x0f, 0x91, 0x7f, 0xa0, 0xed, 0x71, 0x54, 0x68, 
            0x6d, 0x3e, 0x5b, 0x01, 0x41, 0x04, 0x47, 0xd4, 0x90, 0x56, 0x1f, 0x39, 0x6c, 0x8a, 0x9e, 0xfc, 
            0x14, 0x48, 0x6b, 0xc1, 0x98, 0x88, 0x4b, 0xa1, 0x83, 0x79, 0xbc, 0xac, 0x2e, 0x0b, 0xe2, 0xd8, 
            0x52, 0x51, 0x34, 0xab, 0x74, 0x2f, 0x30, 0x1a, 0x9a, 0xca, 0x36, 0x60, 0x6e, 0x5d, 0x29, 0xaa, 
            0x23, 0x8a, 0x9e, 0x29, 0x93, 0x00, 0x31, 0x50, 0x42, 0x3d, 0xf6, 0x92, 0x45, 0x63, 0x64, 0x2d, 
            0x4a, 0xfe, 0x9b, 0xf4, 0xfe, 0x28, 0xff, 0xff, 0xff, 0xff, 0x72, 0x14, 0x2b, 0xf7, 0x68, 0x6c, 
            0xe9, 0x2c, 0x6d, 0xe5, 0xb7, 0x33, 0x65, 0xbf, 0xb9, 0xd5, 0x9b, 0xb6, 0x0c, 0x2c, 0x80, 0x98, 
            0x2d, 0x59, 0x58, 0xc1, 0xe6, 0xa3, 0xb0, 0x8e, 0xa6, 0x89, 0x00, 0x00, 0x00, 0x00, 0x4a, 0x49, 
            0x30, 0x46, 0x02, 0x21, 0x00, 0xbc, 0xe4, 0x3a, 0xd3, 0xac, 0xbc, 0x79, 0xb0, 0x24, 0x7e, 0x54, 
            0xc8, 0xc9, 0x1e, 0xac, 0x1c, 0xf9, 0x03, 0x75, 0x05, 0x00, 0x0e, 0x01, 0xd1, 0xfd, 0x81, 0x18, 
            0x54, 0xd8, 0x5b, 0xc2, 0x1a, 0x02, 0x21, 0x00, 0x99, 0x2a, 0x6f, 0x6f, 0x2f, 0xeb, 0x6f, 0x62, 
            0xd3, 0x70, 0x6f, 0x3b, 0x9a, 0xaa, 0xb8, 0x8d, 0x9f, 0x11, 0x32, 0x95, 0x6a, 0x1d, 0xff, 0xa9, 
            0x26, 0xcd, 0x55, 0x6e, 0xd5, 0x53, 0x60, 0xdf, 0x01, 0xff, 0xff, 0xff, 0xff, 0xd2, 0x81, 0x28, 
            0xbb, 0xb6, 0x20, 0x7c, 0x1c, 0x3d, 0x0a, 0x63, 0x0c, 0xc6, 0x19, 0xdc, 0x7e, 0x7b, 0xea, 0x56, 
            0xac, 0x19, 0xa1, 0xda, 0xb1, 0x27, 0xc6, 0x2c, 0x78, 0xfa, 0x1b, 0x63, 0x2c, 0x00, 0x00, 0x00, 
            0x00, 0x49, 0x48, 0x30, 0x45, 0x02, 0x20, 0x20, 0x97, 0x57, 0x36, 0x81, 0x61, 0x53, 0x77, 0x08, 
            0xfd, 0x29, 0xd8, 0x9b, 0xb1, 0xe9, 0xd6, 0x48, 0x00, 0x79, 0x49, 0xec, 0xfd, 0xed, 0x78, 0x9b, 
            0x51, 0xa9, 0x63, 0x24, 0xcb, 0x65, 0x18, 0x02, 0x21, 0x00, 0xcd, 0x0f, 0x7c, 0x30, 0x21, 0x39, 
            0x16, 0x48, 0x2b, 0x6e, 0x16, 0x6d, 0x8a, 0x4f, 0x2b, 0x98, 0x1f, 0x77, 0x7e, 0xb1, 0x84, 0xcd, 
            0x8a, 0x49, 0x5f, 0x1b, 0x3d, 0x36, 0x90, 0xfb, 0xbf, 0x2d, 0x01, 0xff, 0xff, 0xff, 0xff, 0x01, 
            0x00, 0xa6, 0xf7, 0x5f, 0x02, 0x00, 0x00, 0x00, 0x19, 0x76, 0xa9, 0x14, 0x9e, 0x35, 0xd9, 0x3c, 
            0x77, 0x92, 0xbd, 0xca, 0xad, 0x56, 0x97, 0xdd, 0xeb, 0xf0, 0x43, 0x53, 0xd9, 0xa5, 0xe1, 0x96, 
            0x88, 0xac, 0x00, 0x00, 0x00, 0x00 };

            #endregion
            #region Tx 2

            public static readonly Hash256 Tx2_Hash = Hash256.Parse("69ea9cf5e3e116cffe595d16b3258cd3508768fe3a1ce087ed8479f3d78ef91a");

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
                                        SourceTransactionHash = Hash256.Parse("0e3e4a3af34d6c6ee1966dab9d1eac8c67baadd8b9a466bda9b94cb7e8bf5a73"),
                                        OutputSequenceNumber = 0,
                                    },
                                Script = Script.Parse("304402201e4456d4b9ce0fc7828364fbd70fed283d8eb1361974a7cf058d9568efe7474202201caeeebf6631e9eb38ef33fbf50fe40146e2fa9b830b8b75fb6008dad8fbe97c01 04207efd639399acd3645e891c5a88f702677c08f52812d088108cf22a64ed17a0713589364f1c603d8c10f3c284eb4457e4550c4bf1f833c23abad73c107f7b70"),
                            },
                    },
                TransactionOutputs =
                    {
                        new TransactionOutput
                            {
                                Value = 5000000,
                                Script = Script.Parse("OP_DUP OP_HASH160 bc4f09003cd9ff3ba672c05a75fac4be38a3d57f OP_EQUALVERIFY OP_CHECKSIG"),
                            },
                        new TransactionOutput
                            {
                                Value = 110000000,
                                Script = Script.Parse("OP_DUP OP_HASH160 7fefa10049b41709fc930fb6582c8a1236ad15eb OP_EQUALVERIFY OP_CHECKSIG"),
                            },
                    }
            };

            #endregion

            static Block103958()
            {
                Tx0.Freeze();
                Tx1.Freeze();
                Tx2.Freeze();
            }
        }
    }
}
