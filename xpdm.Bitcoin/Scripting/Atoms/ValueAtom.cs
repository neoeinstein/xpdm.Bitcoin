using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization;

namespace xpdm.Bitcoin.Scripting.Atoms
{
    public sealed class ValueAtom : ScriptAtom, IScriptValueAtom, IEquatable<ValueAtom>
    {
        public override int ResultCount(IExecutionContext context)
        {
            return 1;
        }

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

        protected override void ExecuteImpl(IExecutionContext context)
        {
            context.ValueStack.Push(this.Value);
        }

        public ValueAtom() { }
        public ValueAtom(Stream stream) : base(stream) { }
        public ValueAtom(byte[] buffer, int offset) : base(buffer, offset) { }

        public bool IsLikelyString
        {
            get { return _value.Length > 2 && _value.All(b => 0x20 <= b && b < 0x7F); }
        }

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
            if (IsLikelyString)
            {
                var atomStr = System.Text.Encoding.ASCII.GetString(Value);
                atomStr = atomStr.Replace("' ", "\' ");
                if (atomStr.EndsWith("'"))
                {
                    atomStr = atomStr.Substring(0, atomStr.Length - 1) + @"\'";
                }
                return "'" + atomStr + "'";
            }
            return _value.ToByteString(Endianness.BigEndian);
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

        public static bool operator ==(ValueAtom atom, ValueAtom other)
        {
            return object.ReferenceEquals(atom, other) || !object.ReferenceEquals(atom, null) && atom.Equals(other);
        }

        public static bool operator !=(ValueAtom atom, ValueAtom other)
        {
            return !(atom == other);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }
}
