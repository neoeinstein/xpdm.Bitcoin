using System;
using System.Numerics;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin.Scripting
{
    public static class ScriptAtomFactory
    {
        public static IScriptAtom GetAtom(byte[] buffer, ref int offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Ensures(Contract.Result<IScriptAtom>() != null);
            Contract.Ensures(Contract.ValueAtReturn(out offset) <= buffer.Length);

            var opcode = (ScriptOpCode)buffer[offset++];
            if (opcode <= ScriptOpCode.OP_PUSHDATA4)
            {
                uint size;
                switch(opcode)
                {
                    case ScriptOpCode.OP_PUSHDATA1:
                        if (buffer.Length <= offset)
                        {
                            return null;
                        }
                        size = buffer[offset++];
                        break;
                    case ScriptOpCode.OP_PUSHDATA2:
                        if (buffer.Length <= offset + 1)
                        {
                            return null;
                        }
                        size = xpdm.Bitcoin.Protocol.BitcoinBufferOperations.ReadUInt16(buffer, offset);
                        offset += 2;
                        break;
                    case ScriptOpCode.OP_PUSHDATA4:
                        if (buffer.Length <= offset + 3)
                        {
                            return null;
                        }
                        size = xpdm.Bitcoin.Protocol.BitcoinBufferOperations.ReadUInt32(buffer, offset);
                        offset += 4;
                        break;
                    default:
                        size = (uint)opcode;
                        break;
                }

                if (buffer.Length <= offset + size)
                {
                    return null;
                }
                var valBytes = new byte[size];
                Array.Copy(buffer, offset, valBytes, 0, size);
                offset += (int)size;
                return new Atoms.ValueAtom(new BigInteger(valBytes));
            }

            return GetOpAtom(opcode);
        }

        private static IScriptAtom GetOpAtom(ScriptOpCode opcode)
        {
            Contract.Requires<ArgumentOutOfRangeException>(opcode >= ScriptOpCode.OP_PUSHDATA1, "opcode");

            switch (opcode)
            {
                case ScriptOpCode.OP_DUP:
                    return new Atoms.OpDupAtom();
            }
            return null;
        }
    }
}
