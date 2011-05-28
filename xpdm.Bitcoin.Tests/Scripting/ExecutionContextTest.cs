using System.Collections.Generic;
using Gallio.Framework.Data;
using MbUnit.Framework;
using xpdm.Bitcoin.Core;
using xpdm.Bitcoin.Scripting;
using xpdm.Bitcoin.Tests.Factories.Core.Transactions;

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
            Assert.IsTrue(inProgress);
            Assert.AreEqual(2, ec.ValueStack.Count);
            Assert.AreEqual(0, ec.ControlStack.Count);
            Assert.AreEqual(0, ec.AltStack.Count);
            Assert.IsFalse(ec.HardFailure);
            Assert.IsTrue(ec.IsValid);
            var finalResult = ec.ExecuteFinal(scriptPubKey, transaction, inputIndex);
            Assert.IsTrue(finalResult);
            Assert.AreEqual(1, ec.ValueStack.Count);
            Assert.AreEqual(0, ec.ControlStack.Count);
            Assert.AreEqual(0, ec.AltStack.Count);
            Assert.IsFalse(ec.HardFailure);
            Assert.IsTrue(ec.IsValid);
        }

        public static IEnumerable<IDataItem> ScriptTransactionTuples
        {
            get
            {
                yield return new DataRow(B103958.Tx1.Transaction.TransactionInputs[0].Script,
                                         B103640.Tx1.Transaction.TransactionOutputs[0].Script,
                                         B103958.Tx1.Transaction,
                                         0);
                yield return new DataRow(B072785.Tx2.Transaction.TransactionInputs[0].Script,
                                         B072783.Tx1.Transaction.TransactionOutputs[0].Script,
                                         B072785.Tx2.Transaction,
                                         0);
            }
        }
    }
}
