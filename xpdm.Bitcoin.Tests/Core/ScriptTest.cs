using System;
using System.Collections.Generic;
using Gallio.Framework.Data;
using MbUnit.Framework;
using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Tests.Core
{
    [TestFixture, TestsOn(typeof(Script))]
    public class ScriptTest
    {
        [Test]
        [Factory("ScriptStrings")]
        public void RoundTripParseTest(
            string scriptString,
            string expectedRoundTrip)
        {
            expectedRoundTrip = expectedRoundTrip ?? scriptString;
            var script = Script.Parse(scriptString);
            Assert.IsNotNull(script);
            var roundTrip = script.ToString();
            Assert.AreEqual(expectedRoundTrip, roundTrip, StringComparison.Ordinal);
        }

        public static IEnumerable<IDataItem> ScriptStrings
        {
            get
            {
                yield return new DataRow("", null);
                yield return new DataRow("OP_TRUE", "OP_1");
                yield return new DataRow("OP_FALSE OP_DROP OP_1", "OP_0 OP_DROP OP_1");
                yield return new DataRow("OP_2", null);
                yield return new DataRow("OP_3DUP 34de82cc920ddd", null);
                yield return new DataRow("0123456789abcdef", null);
                yield return new DataRow("'This is a test message'", null);
                yield return new DataRow("''", "");
                yield return new DataRow("8743261987 'afs8324  asdf 324   asdf' OP_ADD OP_DROP OP_1", null);
                yield return new DataRow("8743261987        'afs8324  asdf 324   asdf'  OP_ADD    OP_DROP           OP_TRUE", "8743261987 'afs8324  asdf 324   asdf' OP_ADD OP_DROP OP_1");
                yield return new DataRow(@"'\''", "27");
                yield return new DataRow(@"' \''", "2027");
                yield return new DataRow(@"'\' '", "2720");
                yield return new DataRow(@"'\\''", "5c27");
                yield return new DataRow(@"'\\\''", null);
                yield return new DataRow(@"'\\\\''", null);
                yield return new DataRow(@"'\'", null).WithMetadata("ExpectedException", "System.FormatException");
                yield return new DataRow(@"'\\'", null).WithMetadata("ExpectedException", "System.FormatException");
                yield return new DataRow("23498123s", null).WithMetadata("ExpectedException", "System.FormatException");
            }
        }
    }
}
