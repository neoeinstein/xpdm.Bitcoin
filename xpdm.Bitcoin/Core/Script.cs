using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using C5;
using SCG = System.Collections.Generic;
using xpdm.Bitcoin.Scripting;

namespace xpdm.Bitcoin.Core
{
    public sealed class Script : BitcoinObject, IFreezable, IThawable<Script>
    {
        public static readonly int MaximumScriptByteSize = 10000;

        public IList<IScriptAtom> Atoms { get; private set; }

        public Scripting.IScriptAtom this[int index]
        {
            get
            {
                Contract.Requires<IndexOutOfRangeException>(0 <= index && index < Atoms.Count);

                return Atoms[index];
            }
        }

        public Script()
        {
            var newAtoms = new ArrayList<IScriptAtom>();
            newAtoms.CollectionChanged += AtomsChanged;
            Atoms = newAtoms;
        }
        public Script(SCG.IEnumerable<IScriptAtom> atoms)
        {
            ContractsCommon.NotNull(atoms, "atoms");

            var newAtoms = new ArrayList<IScriptAtom>();
            newAtoms.AddAll(atoms);
            newAtoms.CollectionChanged += AtomsChanged;
            Atoms = newAtoms;
        }

        public Script(Script script)
        {
            ContractsCommon.NotNull(script, "script");

            var atoms = new ArrayList<IScriptAtom>(script.Atoms.Count);
            atoms.AddAll(script.Atoms);
            atoms.CollectionChanged += AtomsChanged;
            Atoms = atoms;
        }
        public Script(Stream stream) : base(stream) { }
        public Script(byte[] buffer, int offset) : base(buffer, offset) { }

        [Pure]
        public Script Subscript(int offset, int length)
        {
            ContractsCommon.ValidOffsetLength(0, Atoms.Count, offset, length);

            using (var view = Atoms.View(offset, length))
            {
                var script = new Script();
                script.Atoms.AddAll(view);
                return script;
            }
        }

        private void AtomsChanged(object sender)
        {
            ContractsCommon.NotFrozen(this);

            InvalidateBitcoinHashes(sender, EventArgs.Empty);
        }

        protected sealed override void Deserialize(Stream stream)
        {
            var scriptSize = (int)ReadVarInt(stream);
            if (scriptSize > Script.MaximumScriptByteSize)
            {
                throw new SerializationException("Unable to deserialize: Script length greater than maximum allowable script size: " + scriptSize);
            }

            var atoms = new ArrayList<IScriptAtom>();
            int read = 0;

            while (read < scriptSize)
            {
                var atom = ScriptAtomFactory.GetAtom(stream);
                read += atom.SerializedByteSize;
                atoms.Add(atom);
            }
            if (read > scriptSize)
            {
                stream.Position -= read - scriptSize;
                throw new SerializationException("Unable to deserialize: Script longer than expected.");
            }
            atoms.CollectionChanged += AtomsChanged;
            Atoms = atoms;

            Freeze();
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

        public bool IsFrozen { get; private set; }

        public void Freeze()
        {
            if (!Atoms.IsReadOnly)
            {
                Atoms = new GuardedList<IScriptAtom>(Atoms);
            }

            IsFrozen = true;
        }

        public Script Thaw()
        {
            return new Script(this);
        }

        public Script ThawTree()
        {
            return Thaw();
        }

        static Script()
        {
            var empty = new Script();
            empty.Freeze();
            _empty = empty;
        }

        private static readonly Script _empty;
        public static Script Empty { get { return _empty; } }

        [ContractInvariantMethod]
        private void __Invariant()
        {
            Contract.Invariant(Atoms != null);
            Contract.Invariant(!IsFrozen || Atoms.IsReadOnly);
        }
    }
}
