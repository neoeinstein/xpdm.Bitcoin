using System.Diagnostics.Contracts;
using System.IO;

namespace xpdm.Bitcoin.Core
{
    public sealed class TransactionInput : BitcoinObject
    {
        public TransactionOutpoint Source { get; private set; }
        private byte[] _scriptBytes;
        public byte[] ScriptBytes
        {
            get
            {
                Contract.Ensures(Contract.Result<byte[]>() != null);

                var retVal = (byte[])_scriptBytes.Clone();
                return retVal;
            }
        }
        public uint SequenceNumber { get; private set; }

        private const uint DefaultSequenceNumber = 0xFFFFFFFFU;

        public TransactionInput(TransactionOutpoint source, byte[] scriptBytes, uint sequenceNumber)
        {
            Source = source;
            _scriptBytes = (byte[])scriptBytes.Clone();
            SequenceNumber = sequenceNumber;
        }
        public TransactionInput(TransactionOutpoint source, byte[] scriptBytes) : this(source, scriptBytes, DefaultSequenceNumber) { }

        public TransactionInput() { }
        public TransactionInput(Stream stream) : base(stream) { }
        public TransactionInput(byte[] buffer, int offset) : base(buffer, offset) { }

        protected override void Deserialize(Stream stream)
        {
            Source = new TransactionOutpoint(stream);
            var byteCount = ReadVarInt(stream);
            _scriptBytes = ReadBytes(stream, (int)byteCount);
            SequenceNumber = ReadUInt32(stream);
        }

        public override void Serialize(Stream stream)
        {
            Source.Serialize(stream);
            WriteVarInt(stream, ScriptBytes.Length);
            WriteBytes(stream, ScriptBytes);
            Write(stream, SequenceNumber);
        }

        public override int SerializedByteSize
        {
            get { return Source.SerializedByteSize + VarIntByteSize(_scriptBytes.Length) + _scriptBytes.Length + BufferOperations.UINT32_SIZE; }
        }
    }
}
