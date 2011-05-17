using System.Diagnostics.Contracts;
using System.Linq;
using System.Numerics;
using C5;
using System;
using System.IO;
using System.Runtime.Serialization;

namespace xpdm.Bitcoin.Scripting.Atoms
{
    public sealed class ValueAtom : ScriptAtom, IScriptValueAtom, IEquatable<ValueAtom>
    {
        private byte[] _value;
        public byte[] Value
        {
            get
            {
                ContractsCommon.ResultIsNonNull<byte[]>();

                return (byte[])_value.Clone();
            }
        }

        public ValueAtom(byte[] bytes)
        {
            ContractsCommon.NotNull(bytes, "bytes");

            _value = bytes;
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

            if (_value.LongLength > ushort.MaxValue)
            {
                if (_value.LongLength > MaximumAtomSize)
                {
                    throw new SerializationException("Unable to serialize atom with size greater than maximum");
                }
                Write(stream, (byte)ScriptOpCode.OP_PUSHDATA4);
                Write(stream, (uint)_value.LongLength);
            }
            else if (_value.LongLength > byte.MaxValue)
            {
                Write(stream, (byte)ScriptOpCode.OP_PUSHDATA2);
                Write(stream, (ushort)_value.LongLength);
            }
            else if (_value.LongLength >= (int)ScriptOpCode.OP_PUSHDATA1)
            {
                Write(stream, (byte)ScriptOpCode.OP_PUSHDATA1);
                Write(stream, (byte)_value.LongLength);
            }
            else
            {
                Write(stream, (byte)_value.LongLength);
            }

            WriteBytes(stream, _value);
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
            _value = ReadBytes(stream, (int)length);
        }

        public override int SerializedByteSize
        {
            get
            {
                if (_value.Length > ushort.MaxValue)
                {
                    return _value.Length + 5;
                }
                if (_value.Length > byte.MaxValue)
                {
                    return _value.Length + 3;
                }
                if (_value.Length >= (byte)ScriptOpCode.OP_PUSHDATA1)
                {
                    return _value.Length + 2;
                }
                return _value.Length + 1;
            }
        }

        public override string ToString()
        {
            return BufferOperations.ToByteString(_value, Endianness.BigEndian);
        }

        [Pure]
        public bool Equals(ValueAtom other)
        {
            return other != null && _value.Length == other._value.Length && _value.SequenceEqual(other._value);
        }

        [Pure]
        public override bool Equals(IScriptAtom other)
        {
            return this.Equals(other as ValueAtom);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as ValueAtom);
        }
    }
}
