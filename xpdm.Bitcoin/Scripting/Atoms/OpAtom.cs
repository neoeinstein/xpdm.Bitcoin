using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace xpdm.Bitcoin.Scripting.Atoms
{
    public abstract class OpAtom : ScriptAtom, IEquatable<OpAtom>
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

        [Pure]
        public bool Equals(OpAtom other)
        {
            return other != null && OpCode.Equals(other.OpCode);
        }

        [Pure]
        public sealed override bool Equals(IScriptAtom other)
        {
            return this.Equals(other as OpAtom);
        }

        public sealed override bool Equals(object obj)
        {
            return this.Equals(obj as OpAtom);
        }

        public static bool operator ==(OpAtom atom, OpAtom other)
        {
            return object.ReferenceEquals(atom, other) || !object.ReferenceEquals(atom, null) && atom.Equals(other);
        }

        public static bool operator !=(OpAtom atom, OpAtom other)
        {
            return !(atom == other);
        }

        public override int GetHashCode()
        {
            return OpCode.GetHashCode();
        }
    }
}
