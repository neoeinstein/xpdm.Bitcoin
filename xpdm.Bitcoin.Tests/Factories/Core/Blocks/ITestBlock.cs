using System.Collections.Generic;
using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Tests.Factories.Core.Blocks
{
    interface ITestBlock
    {
        Hash256 ExpectedHash { get; }
        Block Header { get; }
        Block Block { get; }
        IEnumerable<Hash256> ExpectedMerkleTree { get; }
        byte[] SerializedBlockData { get; }
    }
}
