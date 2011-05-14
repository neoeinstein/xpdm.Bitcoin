using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.IO;

namespace xpdm.Bitcoin.Scripting.Atoms
{
    public abstract class OpAtom : ScriptAtom
    {
        public ScriptOpCode OpCode { get; private set; }

        protected OpAtom(ScriptOpCode opcode)
        {
            OpCode = opcode;
        }

        protected OpAtom() { }
        protected OpAtom(Stream stream) : base(stream) { }
        protected OpAtom(byte[] buffer, int offset) : base(buffer, offset) { }

        public override string ToString()
        {
            return OpCode.ToString();
        }

        protected sealed override void Deserialize(Stream stream)
        {
            var opcode = (ScriptOpCode)ReadByte(stream);
            OpCode = opcode;
        }

        public sealed override void Serialize(Stream stream)
        {
            Write(stream, (byte)OpCode);
        }

        public sealed override int SerializedByteSize
        {
            get { return 1; }
        }
    }
}
