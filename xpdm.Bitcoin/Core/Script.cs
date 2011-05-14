using System;
using C5;
using SCG = System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace xpdm.Bitcoin.Core
{
    public sealed class Script : BitcoinObject
    {
        public IList<Scripting.IScriptAtom> Atoms { get; private set; }

        public Script()
        {
            Atoms = new WrappedArray<Scripting.IScriptAtom>(new Scripting.IScriptAtom[0]);
        }
        public Script(Stream stream) : base(stream) { }
        public Script(byte[] buffer, int offset) : base(buffer, offset) { }

        protected override void Deserialize(Stream stream)
        {
            var size = (int)ReadVarInt(stream);
            var atoms = new ArrayList<Scripting.IScriptAtom>();
            int read = 0;
            while (read < size)
            {
                var atom = Scripting.ScriptAtomFactory.GetAtom(stream);
                read += atom.SerializedByteSize;
                atoms.Add(atom);
            }
            if (read > size)
            {
                stream.Position -= read - size;
                throw new SerializationException("Script longer than expected.");
            }
            Atoms = new GuardedList<Scripting.IScriptAtom>(atoms);
        }

        public override void Serialize(Stream stream)
        {
            WriteVarInt(stream, Atoms.Sum(a => a.SerializedByteSize));
            foreach (var atom in Atoms)
            {
                atom.Serialize(stream);
            }
        }

        public override int SerializedByteSize
        {
            get
            {
                return Atoms.Sum(a => a.SerializedByteSize);
            }
        }
    }
}
