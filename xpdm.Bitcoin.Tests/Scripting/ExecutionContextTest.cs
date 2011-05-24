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
                yield return new DataRow(Transactions.Block103958.Tx1.TransactionInputs[0].Script,
                                         Transactions.Block103640.Tx1.TransactionOutputs[0].Script,
                                         Transactions.Block103958.Tx1,
                                         0);
                yield return new DataRow(Transactions.Block072785.Tx2.TransactionInputs[0].Script,
                                         Transactions.Block072783.Tx1.TransactionOutputs[0].Script,
                                         Transactions.Block072785.Tx2,
                                         0);
            }
        }
    }
}
