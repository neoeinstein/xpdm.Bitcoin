using System.Diagnostics.Contracts;
using System.Numerics;
using C5;
using System;

namespace xpdm.Bitcoin.Scripting.Atoms
{
    public class ValueAtom : ScriptAtom, IScriptValueAtom
    {
        public BigInteger Value { get; private set; }

        public ValueAtom(BigInteger value)
        {
            Value = value;
        }

        public ValueAtom(long value)
        {
            Value = value;
        }

        public ValueAtom(ulong value)
        {
            Value = value;
        }

        protected override void ExecuteImpl(ExecutionContext context)
        {
            context.ValueStack.Push(this);
        }

        public override byte[] ToByteCode()
        {
            Contract.Ensures(Contract.Result<byte[]>().Length >= 1);

            if (Value == 0)
            {
                return new byte[] { 0 };
            }
            if (-1 <= Value && Value <= 16)
            {
                return new byte[] { (byte)(Value + (int)ScriptOpCode.OP_RESERVED) };
            }
            var val = Value.ToByteArray();
            byte[] retVal;
            if (val.LongLength > uint.MaxValue)
            {
                throw new InvalidOperationException("Unable to write value with size greater than " + uint.MaxValue);
            }
            if (val.LongLength > ushort.MaxValue)
            {
                retVal = new byte[val.LongLength + 1 + 4];
                retVal[0] = (byte)ScriptOpCode.OP_PUSHDATA4;
                xpdm.Bitcoin.Protocol.BitcoinBufferOperations.WriteBytes((uint)retVal.LongLength, retVal, 1);
                val.CopyTo(retVal, 5);
            }
            else if (val.LongLength > byte.MaxValue)
            {
                retVal = new byte[val.LongLength + 1 + 2];
                retVal[0] = (byte)ScriptOpCode.OP_PUSHDATA2;
                xpdm.Bitcoin.Protocol.BitcoinBufferOperations.WriteBytes((ushort)retVal.LongLength, retVal, 1);
                val.CopyTo(retVal, 3);
            }
            else if (val.LongLength >= (int)ScriptOpCode.OP_PUSHDATA1)
            {
                retVal = new byte[val.LongLength + 1 + 1];
                retVal[0] = (byte)ScriptOpCode.OP_PUSHDATA1;
                retVal[1] = (byte)val.LongLength;
                val.CopyTo(retVal, 2);
            }
            else
            {
                retVal = new byte[val.LongLength + 1];
                retVal[0] = (byte)val.LongLength;
                val.CopyTo(retVal, 1);
            }

            return retVal;
        }

        public override string ToString()
        {
            return Value.ToString("x");
        }

        public string ToString(string format)
        {
            return Value.ToString(format);
        }

        public static implicit operator ValueAtom(BigInteger value)
        {
            return new ValueAtom(value);
        }

        public static implicit operator ValueAtom(long value)
        {
            return new ValueAtom(value);
        }

        public static implicit operator ValueAtom(ulong value)
        {
            return new ValueAtom(value);
        }

        public static implicit operator BigInteger(ValueAtom value)
        {
            return value.Value;
        }

        public static explicit operator long(ValueAtom value)
        {
            return (long)value.Value;
        }

        public static explicit operator ulong(ValueAtom value)
        {
            return (ulong)value.Value;
        }
    }
}
