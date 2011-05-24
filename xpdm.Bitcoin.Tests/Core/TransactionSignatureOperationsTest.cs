using System.Collections.Generic;
using Gallio.Framework;
using Gallio.Framework.Data;
using MbUnit.Framework;
using xpdm.Bitcoin.Core;
using xpdm.Bitcoin.Tests.Factories.Core;

namespace xpdm.Bitcoin.Tests.Core
{
    [TestFixture]
    public class TransactionSignatureOperationsTest
    {
        [Test]
        [Factory("SignatureVerifyFactory")]
        public void VerifySignature(byte[] publicKey, byte[] sigHash, Script script, Transaction transaction, int index)
        {
            TestLog.WriteLine(transaction.ToString());
            var verify = TransactionSignatureOperations.VerifySignature(
                publicKey, sigHash, script, transaction, index, SignatureHashType.Unknown);
            Assert.IsTrue(verify);
        }

        public IEnumerable<IDataItem> SignatureVerifyFactory
        {
            get
            {
                var key = "0447d490561f396c8a9efc14486bc198884ba18379bcac2e0be2d8525134ab742f301a9aca36606e5d29aa238a9e2993003150423df6924563642d4afe9bf4fe28";
                var sig = "3046022100f5746b0b254f5a37e75251459c7a23b6dfcb868ac7467edd9a6fdd1d969871be02210088948aea29b69161ca341c49c02686a81d8cbb73940f917fa0ed7154686d3e5b01";
                var s = "OP_DUP OP_HASH160 02bf4b2889c6ada8190c252e70bde1a1909f9617 OP_EQUALVERIFY OP_CHECKSIG";

                yield return new DataRow(key, sig, s, new Transaction(Transactions.Block103958.Tx1_Serialized, 0), 0);
                yield return new DataRow(key, sig, s, Transactions.Block103958.Tx1, 0);
            }
        }
    }
}
