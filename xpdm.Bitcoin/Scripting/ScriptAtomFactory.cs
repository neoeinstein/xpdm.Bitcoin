using System;
using System.Numerics;
using System.Diagnostics.Contracts;
using System.IO;

namespace xpdm.Bitcoin.Scripting
{
    public static class ScriptAtomFactory
    {
        public static IScriptAtom GetAtom(Stream stream)
        {
            Contract.Ensures(Contract.Result<IScriptAtom>() != null);

            var opcode = (ScriptOpCode)stream.ReadByte();

            if (opcode <= ScriptOpCode.OP_PUSHDATA4)
            {
                stream.Position--;
                return new Atoms.ValueAtom(stream);
            }

            return GetOpAtom(opcode);
        }

        public static IScriptAtom GetAtom(byte[] buffer, int offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Ensures(Contract.Result<IScriptAtom>() != null);
            Contract.Ensures(Contract.ValueAtReturn(out offset) <= buffer.Length);

            var ms = new MemoryStream(buffer, offset, buffer.Length - offset);
            return GetAtom(ms);
        }

        public static IScriptAtom GetOpAtom(ScriptOpCode opcode)
        {
            Contract.Requires<ArgumentOutOfRangeException>(opcode > ScriptOpCode.OP_PUSHDATA4, "opcode");

            switch (opcode)
            {
                case ScriptOpCode.OP_DUP:
                case ScriptOpCode.OP_2DUP:
                case ScriptOpCode.OP_3DUP:
                    return new Atoms.OpDupAtom(opcode);
                case ScriptOpCode.OP_RIPEMD160:
                case ScriptOpCode.OP_SHA1:
                case ScriptOpCode.OP_SHA256:
                case ScriptOpCode.OP_HASH160:
                case ScriptOpCode.OP_HASH256:
                    return new Atoms.OpHashAtom(opcode);
                case ScriptOpCode.OP_EQUAL:
                case ScriptOpCode.OP_EQUALVERIFY:
                    return new Atoms.OpEqualAtom(opcode);
            }
            return null;
        }
    }
}
