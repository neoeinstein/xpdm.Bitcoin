using System.Diagnostics.Contracts;
using System.Numerics;
using C5;
using System;
using System.IO;
using System.Runtime.Serialization;

namespace xpdm.Bitcoin.Scripting.Atoms
{
    public sealed class ValueAtom : ScriptAtom, IScriptValueAtom
    {
        public byte[] Value { get; private set; }

        public ValueAtom(byte[] bytes)
        {
            Value = bytes;
        }

        public ValueAtom(BigInteger value)
            : this(value.ToByteArray())
        {
        }

        public ValueAtom(long value)
            : this(BitConverter.GetBytes(value))
        {
        }

        public ValueAtom(ulong value)
            : this(BitConverter.GetBytes(value))
        {
        }

        protected override void ExecuteImpl(ExecutionContext context)
        {

            context.ValueStack.Push(this.Value);
        }

        public ValueAtom() { }
        public ValueAtom(Stream stream) : base(stream) { }
        public ValueAtom(byte[] buffer, int offset) : base(buffer, offset) { }

        public override void Serialize(Stream stream)
        {
            //Contract.Ensures(Contract.Result<byte[]>() != null);
            //Contract.Ensures(Value.LongLength <= ushort.MaxValue || Contract.Result<byte[]>().LongLength == Value.LongLength + 5);
            //Contract.Ensures(Value.LongLength <= byte.MaxValue || Contract.Result<byte[]>().LongLength == Value.LongLength + 3);
            //Contract.Ensures(Value.LongLength < (int)ScriptOpCode.OP_PUSHDATA1 || Contract.Result<byte[]>().LongLength == Value.LongLength + 2);
            //Contract.Ensures(Value.LongLength >= (int)ScriptOpCode.OP_PUSHDATA1 || Contract.Result<byte[]>().LongLength == Value.LongLength + 1);

            if (Value.LongLength > ushort.MaxValue)
            {
                if (Value.LongLength > MaximumAtomSize)
                {
                    throw new SerializationException("Unable to serialize atom with size greater than maximum");
                }
                Write(stream, (byte)ScriptOpCode.OP_PUSHDATA4);
                Write(stream, (uint)Value.LongLength);
            }
            else if (Value.LongLength > byte.MaxValue)
            {
                Write(stream, (byte)ScriptOpCode.OP_PUSHDATA2);
                Write(stream, (ushort)Value.LongLength);
            }
            else if (Value.LongLength >= (int)ScriptOpCode.OP_PUSHDATA1)
            {
                Write(stream, (byte)ScriptOpCode.OP_PUSHDATA1);
                Write(stream, (byte)Value.LongLength);
            }
            else
            {
                Write(stream, (byte)Value.LongLength);
            }

            WriteBytes(stream, Value);
        }

        protected override void Deserialize(Stream stream)
        {
            var opcode = (ScriptOpCode)ReadByte(stream);
            uint length;
            switch (opcode)
            {
                case ScriptOpCode.OP_PUSHDATA1:
                    length = ReadByte(stream);
                    break;
                case ScriptOpCode.OP_PUSHDATA2:
                    length = ReadUInt16(stream);
                    break;
                case ScriptOpCode.OP_PUSHDATA4:
                    length = ReadUInt32(stream);
                    break;
                default:
                    length = (byte)opcode;
                    break;
            }
            if (length > MaximumAtomSize)
            {
                throw new SerializationException("Unable to deserialize atom with size greater than maximum");
            }
            Value = ReadBytes(stream, (int)length);
        }

        public override int SerializedByteSize
        {
            get
            {
                if (Value.Length > ushort.MaxValue)
                {
                    return Value.Length + 5;
                }
                if (Value.Length > byte.MaxValue)
                {
                    return Value.Length + 3;
                }
                if (Value.Length >= (byte)ScriptOpCode.OP_PUSHDATA1)
                {
                    return Value.Length + 2;
                }
                return Value.Length + 1;
            }
        }

        public override string ToString()
        {
            return BufferOperations.ToByteString(Value);
        }
    }
}
