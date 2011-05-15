using System.Diagnostics.Contracts;
using System.IO;

namespace xpdm.Bitcoin.Core
{
    public sealed class TransactionInput : BitcoinObject
    {
        public TransactionOutpoint Source { get; private set; }
        public Script Script { get; private set; }
        public uint SequenceNumber { get; private set; }

        private const uint DefaultSequenceNumber = 0xFFFFFFFFU;

        public TransactionInput(TransactionOutpoint source, Script script, uint sequenceNumber)
        {
            Source = source;
            Script = script;
            SequenceNumber = sequenceNumber;
        }
        public TransactionInput(TransactionOutpoint source, Script script) : this(source, script, DefaultSequenceNumber) { }

        public TransactionInput() { }
        public TransactionInput(Stream stream) : base(stream) { }
        public TransactionInput(byte[] buffer, int offset) : base(buffer, offset) { }

        protected override void Deserialize(Stream stream)
        {
            Source = new TransactionOutpoint(stream);
            Script = new Script(stream);
            SequenceNumber = ReadUInt32(stream);
        }

        public override void Serialize(Stream stream)
        {
            Source.Serialize(stream);
            Script.Serialize(stream);
            Write(stream, SequenceNumber);
        }

        public override int SerializedByteSize
        {
            get { return Source.SerializedByteSize + Script.SerializedByteSize + BufferOperations.UINT32_SIZE; }
        }

        public override string ToString()
        {
            return string.Format("{0} [ {1} ] {2}", Source, Script, SequenceNumber);
        }
    }
}
