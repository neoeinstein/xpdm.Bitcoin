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
        public static readonly int MaximumScriptByteSize = 10000;

        public IList<Scripting.IScriptAtom> Atoms { get; private set; }

        public Script()
        {
            Atoms = new WrappedArray<Scripting.IScriptAtom>(new Scripting.IScriptAtom[0]);
        }

        public Script(SCG.IEnumerable<Scripting.IScriptAtom> atoms)
        {
            var atomsList = new ArrayList<Scripting.IScriptAtom>();
            atomsList.AddAll(atoms);
            Atoms = new GuardedList<Scripting.IScriptAtom>(atomsList);
        }

        public Script Subscript(int index, int length)
        {
            var subscript = new Script();
            subscript.Atoms = Atoms.View(index, length);
            return subscript;
        }

        public ScriptBuilder ToScriptBuilder()
        {
            var sb = new ScriptBuilder();
            sb.Atoms.AddAll(Atoms);
            return sb;
        }

        public byte[] SerializeToByteArrayWithoutSize()
        {
            var arr = new byte[Atoms.Sum(a => a.SerializedByteSize)];
            Array.Copy(SerializeToByteArray(), VarIntByteSize(arr.Length), arr, 0, arr.Length);
            return arr;
        }

        public Script(Stream stream) : base(stream) { }
        public Script(byte[] buffer, int offset) : base(buffer, offset) { }

        protected override void Deserialize(Stream stream)
        {
            var scriptSize = (int)ReadVarInt(stream);
            if (scriptSize > Script.MaximumScriptByteSize)
            {
                throw new SerializationException("Unable to deserialize: Script length greater than maximum allowable script size: " + scriptSize);
            }

            var atoms = new ArrayList<Scripting.IScriptAtom>();
            int read = 0;

            while (read < scriptSize)
            {
                var atom = Scripting.ScriptAtomFactory.GetAtom(stream);
                read += atom.SerializedByteSize;
                atoms.Add(atom);
            }
            if (read > scriptSize)
            {
                stream.Position -= read - scriptSize;
                throw new SerializationException("Unable to deserialize: Script longer than expected.");
            }
            Atoms = new GuardedList<Scripting.IScriptAtom>(atoms);
        }

        public override void Serialize(Stream stream)
        {
            var scriptSize = Atoms.Sum(a => a.SerializedByteSize);
            if (scriptSize > Script.MaximumScriptByteSize)
            {
                throw new SerializationException("Unable to serialize: Script length greater than maximum allowable script size: " + scriptSize);
            }

            WriteVarInt(stream, scriptSize);
            foreach (var atom in Atoms)
            {
                atom.Serialize(stream);
            }
        }

        public override int SerializedByteSize
        {
            get
            {
                var scriptSize = Atoms.Sum(a => a.SerializedByteSize);
                return VarIntByteSize(scriptSize) + scriptSize;
            }
        }

        public override string ToString()
        {
            return string.Join(" ", Atoms);
        }
    }
}
