using System.Collections.Generic;
using Gallio.Framework.Data;
using MbUnit.Framework;
using NHamcrest.Core;
using xpdm.Bitcoin.Core;
using xpdm.Bitcoin.Scripting;
using xpdm.Bitcoin.Tests.Factories.Core;

namespace xpdm.Bitcoin.Tests.Scripting
{
    [TestFixture]
    public class ExecutionContextTest
    {
        [Test]
        [Factory("ScriptTransactionTuples")]
        public void ScriptingTest(Script scriptSig, Script scriptPubKey, Transaction transaction, int inputIndex)
        {
            var ec = new ExecutionContext();
            var inProgress = ec.Execute(scriptSig, transaction, inputIndex);
            Assert.That(inProgress, Is.True());
            Assert.That(ec.ValueStack.Count, Is.EqualTo(2));
            Assert.That(ec.HardFailure, Is.False());
            Assert.That(ec.IsValid, Is.True());
            var finalResult = ec.ExecuteFinal(scriptPubKey, transaction, inputIndex);
            Assert.That(finalResult, Is.True());
            Assert.That(ec.ValueStack.Count, Is.EqualTo(1));
            Assert.That(ec.ControlStack.Count, Is.EqualTo(0));
            Assert.That(ec.AltStack.Count, Is.EqualTo(0));
            Assert.That(ec.HardFailure, Is.False());
            Assert.That(ec.IsValid, Is.True());
        }

        public static IEnumerable<IDataItem> ScriptTransactionTuples
        {
            get
            {
                yield return
                    new DataRow(
                        Transactions.Block103958.Tx1.TransactionInputs[0].Script,
                        "OP_DUP OP_HASH160 02bf4b2889c6ada8190c252e70bde1a1909f9617 OP_EQUALVERIFY OP_CHECKSIG",
                        Transactions.Block103958.Tx1,
                        0);
            }
        }
    }
}
