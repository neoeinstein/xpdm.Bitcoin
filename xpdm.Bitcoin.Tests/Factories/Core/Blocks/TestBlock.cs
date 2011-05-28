using System.Collections.Generic;
using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Tests.Factories.Core.Blocks
{
    abstract class TestBlock : ITestBlock
    {
        #region ITestBlock Members

        public Hash256 ExpectedHash { get; protected set; }
        public Block Header { get; protected set; }
        public Block Block { get; protected set; }
        public IEnumerable<Hash256> ExpectedMerkleTree { get; protected set; }
        public byte[] SerializedBlockData { get; protected set; }

        #endregion
    }
}
