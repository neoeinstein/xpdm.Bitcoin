using System.Diagnostics.Contracts;
using System.Numerics;
using C5;
using System;

namespace xpdm.Bitcoin.Scripting.Atoms
{
    public class ValueAtom : ScriptAtom, IScriptValueAtom
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

        public override byte[] ToByteCode()
        {
            Contract.Ensures(Contract.Result<byte[]>() != null);
            Contract.Ensures(Value.LongLength <= ushort.MaxValue || Contract.Result<byte[]>().LongLength == Value.LongLength + 5);
            Contract.Ensures(Value.LongLength <= byte.MaxValue || Contract.Result<byte[]>().LongLength == Value.LongLength + 3);
            Contract.Ensures(Value.LongLength < (int)ScriptOpCode.OP_PUSHDATA1 || Contract.Result<byte[]>().LongLength == Value.LongLength + 2);
            Contract.Ensures(Value.LongLength >= (int)ScriptOpCode.OP_PUSHDATA1 || Contract.Result<byte[]>().LongLength == Value.LongLength + 1);

            byte[] retVal;
            if (Value.LongLength > uint.MaxValue)
            {
                throw new InvalidOperationException("Unable to write value with size greater than " + uint.MaxValue);
            }
            if (Value.LongLength > ushort.MaxValue)
            {
                retVal = new byte[Value.LongLength + 1 + 4];
                retVal[0] = (byte)ScriptOpCode.OP_PUSHDATA4;
                ((uint)retVal.LongLength).WriteBytes(retVal, 1);
                Value.CopyTo(retVal, 5);
            }
            else if (Value.LongLength > byte.MaxValue)
            {
                retVal = new byte[Value.LongLength + 1 + 2];
                retVal[0] = (byte)ScriptOpCode.OP_PUSHDATA2;
                ((ushort)retVal.LongLength).WriteBytes(retVal, 1);
                Value.CopyTo(retVal, 3);
            }
            else if (Value.LongLength >= (int)ScriptOpCode.OP_PUSHDATA1)
            {
                retVal = new byte[Value.LongLength + 1 + 1];
                retVal[0] = (byte)ScriptOpCode.OP_PUSHDATA1;
                retVal[1] = (byte)Value.LongLength;
                Value.CopyTo(retVal, 2);
            }
            else
            {
                retVal = new byte[Value.LongLength + 1];
                retVal[0] = (byte)Value.LongLength;
                Value.CopyTo(retVal, 1);
            }

            return retVal;
        }
    }
}
