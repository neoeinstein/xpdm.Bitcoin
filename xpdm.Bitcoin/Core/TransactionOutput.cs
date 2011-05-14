using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;

namespace xpdm.Bitcoin.Core
{
    public sealed class TransactionOutput : BitcoinObject
    {
        public BitcoinValue Value { get; private set; }
        public Script Script { get; private set; }

        public TransactionOutput(BitcoinValue value, Script script)
        {
            Value = value;
            Script = script;
        }

        public TransactionOutput() { }
        public TransactionOutput(Stream stream) : base(stream) { }
        public TransactionOutput(byte[] buffer, int offset) : base(buffer, offset) { }

        protected override void Deserialize(System.IO.Stream stream)
        {
            Value = ReadUInt64(stream);
            Script = new Script(stream);
        }

        public override void Serialize(System.IO.Stream stream)
        {
            Write(stream, (ulong)Value);
            Script.Serialize(stream);
        }

        public override int SerializedByteSize
        {
            get { return BufferOperations.UINT64_SIZE + Script.SerializedByteSize; }
        }
    }
}
