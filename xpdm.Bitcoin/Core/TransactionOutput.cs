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


        public TransactionOutput(BitcoinValue value, byte[] scriptBytes)
        {
            Value = value;
            _scriptBytes = (byte[])scriptBytes.Clone();
        }

        public TransactionOutput(Stream stream) : base(stream) { }
        public TransactionOutput(byte[] buffer, int offset) : base(buffer, offset) { }

        protected override void Deserialize(System.IO.Stream stream)
        {
            Value = ReadUInt64(stream);
            var byteCount = ReadVarInt(stream);
            _scriptBytes = ReadBytes(stream, (int)byteCount);
        }

        public override void Serialize(System.IO.Stream stream)
        {
            Write(stream, (ulong)Value);
            WriteVarInt(stream, ScriptBytes.Length);
            WriteBytes(stream, ScriptBytes);
        }

        public override int SerializedByteSize
        {
            get { return BufferOperations.UINT64_SIZE + VarIntByteSize(_scriptBytes.Length) + _scriptBytes.Length; }
        }
    }
}
