
using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Tests.Factories.Core
{
    public static class Blocks
    {
        public static readonly Hash256 Block103958_Hash = Hash256.Parse("000000000002055a179aa185de281da76ce6de1d0df94a7bf4ea642dff0070ae");

        public static readonly Block Block103958_Header = new Block
            {
                Version = 1,
                PreviousBlockHash = Hash256.Parse("0000000000005639d7aa5a5c32c9c2e622040c5cb4ded25a9201254cd1c09468"),
                MerkleRoot = Hash256.Parse("54312d3d842bbe585c15cef58ae300b2cf105f59e5d27f5bcc24c61d52270f31"),
                Timestamp = 1295686236,
                DifficultyBits = 453217774, //0x1b038dee
                Nonce = 3826166152,
            };

        public static readonly Block Block103958 = new Block
        {
            Version = 1,
            PreviousBlockHash = Hash256.Parse("0000000000005639d7aa5a5c32c9c2e622040c5cb4ded25a9201254cd1c09468"),
            Timestamp = 1295686236,
            DifficultyBits = 453217774, //0x1b038dee
            Nonce = 3826166152,
            Transactions =
                {
                    Transactions.Block103958_Tx0,
                    Transactions.Block103958_Tx1,
                    Transactions.Block103958_Tx2,
                },
        };

        public static readonly Hash256[] Block103958_MerkleTree = new[]
            {
                Hash256.Parse("f2dedefe5222786abd6cb2223c89e50d590f234dbba69002d5ad5cd6e438abfd"),
                Hash256.Parse("ff954e099764d192c5bb531c9c14c18c230b0c0a63f02cd168a4ea94548c890f"),
                Hash256.Parse("69ea9cf5e3e116cffe595d16b3258cd3508768fe3a1ce087ed8479f3d78ef91a"),
                Hash256.Parse("cb4e6248ed1591012e5c3c865ad732f423b6d2ac2beaa5be0c576934e8a436bd"),
                Hash256.Parse("6f9be236a0af022d0344802b2d89b85ff6a5df46db309db47b191f9f67413527"),
                Hash256.Parse("54312d3d842bbe585c15cef58ae300b2cf105f59e5d27f5bcc24c61d52270f31"),
            };
    }
}
