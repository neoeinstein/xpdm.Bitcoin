using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin.Scripting.Atoms
{
    public abstract class OpAtom : ScriptAtom
    {
        public ScriptOpCode OpCode { get; private set; }

        protected OpAtom(ScriptOpCode opcode)
        {
            OpCode = opcode;
        }

        public override byte[] ToByteCode()
        {
            Contract.Ensures(Contract.Result<byte[]>().Length == 1);
            Contract.Ensures((ScriptOpCode)Contract.Result<byte[]>()[0] == OpCode);

            return new byte[] { (byte)OpCode };
        }

        public override string ToString()
        {
            return OpCode.ToString();
        }
    }
}
