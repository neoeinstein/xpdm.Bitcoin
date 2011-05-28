using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Tests.Factories.Core.Transactions
{
    interface ITestTx
    {
        Hash256 ExpectedHash { get; }
        Transaction Transaction { get; }
        bool ExpectedCoinbase { get; }
        byte[] SerializedTransactionData { get; }
    }
}
