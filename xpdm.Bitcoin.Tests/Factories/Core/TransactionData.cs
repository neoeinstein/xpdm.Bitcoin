using System.Collections.Generic;
using Gallio.Framework.Data;
using xpdm.Bitcoin.Core;
using xpdm.Bitcoin.Tests.Factories.Core.Transactions;

namespace xpdm.Bitcoin.Tests.Factories.Core
{
    class TransactionData
    {
        public static IEnumerable<IDataItem> TransactionsWithHash
        {
            get
            {
                foreach (var testTx in Instance._txHash.Values)
                {
                    if (testTx.Transaction != null)
                    {
                        yield return new DataRow(testTx.ExpectedHash, testTx.Transaction);
                    }
                }
            }
        }

        public static IEnumerable<IDataItem> TransactionsWithCoinbase
        {
            get
            {
                foreach (var testTx in Instance._txHash.Values)
                {
                    if (testTx.Transaction != null)
                    {
                        yield return new DataRow(testTx.ExpectedCoinbase, testTx.Transaction);
                    }
                }
            }
        }

        public static IEnumerable<IDataItem> SerializedTransactionsForRoundTripping
        {
            get
            {
                foreach (var testTx in Instance._txHash.Values)
                {
                    if (testTx.Transaction != null && testTx.SerializedTransactionData != null)
                    {
                        yield return new DataRow(testTx.Transaction, testTx.SerializedTransactionData);
                    }
                }
            }
        }
        private C5.IDictionary<BitcoinObject, ITestTx> _txHash = new C5.HashDictionary<BitcoinObject, ITestTx>
        {
            { B000000.Coinbase.ExpectedHash, B000000.Coinbase },
            { B000170.Coinbase.ExpectedHash, B000170.Coinbase },
            { B000170.Tx1.ExpectedHash, B000170.Tx1 },
            { B072783.Coinbase.ExpectedHash, B072783.Coinbase },
            { B072783.Tx1.ExpectedHash, B072783.Tx1 },
            { B072785.Coinbase.ExpectedHash, B072785.Coinbase },
            { B072785.Tx1.ExpectedHash, B072785.Tx1 },
            { B072785.Tx2.ExpectedHash, B072785.Tx2 },
            { B072785.Tx3.ExpectedHash, B072785.Tx3 },
            { B072785.Tx4.ExpectedHash, B072785.Tx4 },
            { B072785.Tx5.ExpectedHash, B072785.Tx5 },
            { B103640.Coinbase.ExpectedHash, B103640.Coinbase },
            { B103640.Tx1.ExpectedHash, B103640.Tx1 },
            { B103958.Coinbase.ExpectedHash, B103958.Coinbase },
            { B103958.Tx1.ExpectedHash, B103958.Tx1 },
            { B103958.Tx2.ExpectedHash, B103958.Tx2 },
        };

        public ITestTx this[string txHash]
        {
            get { return _txHash[Hash256.Parse(txHash)]; }
        }

        public ITestTx this[BitcoinObject bitObj]
        {
            get { return _txHash[bitObj]; }
        }

        private static TransactionData _instance = new TransactionData();
        public static TransactionData Instance
        {
            get { return _instance; }
        }

        private TransactionData()
        {
        }

    }
}
