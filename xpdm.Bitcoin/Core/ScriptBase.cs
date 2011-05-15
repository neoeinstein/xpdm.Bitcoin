using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using C5;
using SCG = System.Collections.Generic;

namespace xpdm.Bitcoin.Core
{
    public abstract class ScriptBase : BitcoinObject
    {
        public static readonly int MaximumScriptByteSize = 10000;

        private IList<Scripting.IScriptAtom> _atoms;
        public virtual IList<Scripting.IScriptAtom> Atoms
        {
            get
            {
                Contract.Ensures(Contract.Result<IList<Scripting.IScriptAtom>>() != null);

                return _atoms;
            }
            protected set
            {
                Contract.Requires<ArgumentNullException>(value != null, "value");
                Contract.Ensures(Atoms != null);

                _atoms = value;
            }
        }

        protected ScriptBase(SCG.IEnumerable<Scripting.IScriptAtom> atoms)
        {
            Contract.Requires<ArgumentNullException>(atoms != null);

            var atomsList = new ArrayList<Scripting.IScriptAtom>();
            atomsList.AddAll(atoms);
            Atoms = atomsList;
        }

        protected ScriptBase() { }
        protected ScriptBase(Stream stream) : base(stream) { }
        protected ScriptBase(byte[] buffer, int offset) : base(buffer, offset) { }

        [Pure]
        protected T Subscript<T>(int index, int length) where T : ScriptBase, new()
        {
            Contract.Requires<ArgumentOutOfRangeException>(0 <= index && index < Atoms.Count, "index");
            Contract.Requires<ArgumentOutOfRangeException>(0 <= length && length <= Atoms.Count, "length");
            Contract.Requires<ArgumentOutOfRangeException>(index + length <= Atoms.Count, "length");

            using (var view = Atoms.View(index, length))
            {
                var script = new T();
                script._atoms.AddAll(view);
                return script;
            }
        }

        protected sealed override void Deserialize(Stream stream)
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
            Atoms = atoms;
        }

        public sealed override void Serialize(Stream stream)
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

        public sealed override int SerializedByteSize
        {
            get
            {
                var scriptSize = Atoms.Sum(a => a.SerializedByteSize);
                return VarIntByteSize(scriptSize) + scriptSize;
            }
        }

        public sealed override string ToString()
        {
            return string.Join(" ", Atoms);
        }
    }
}
