using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Tests.Factories.Core.Transactions
{
    class TestTx : ITestTx
    {
        #region ITestTx Members

        public Hash256 ExpectedHash { get; set; }
        public Transaction Transaction { get; set; }
        public bool ExpectedCoinbase { get; set; }
        public byte[] SerializedTransactionData { get; set; }

        #endregion
    }
}
