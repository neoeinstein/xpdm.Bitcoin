using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using C5;
using xpdm.Bitcoin.Scripting;
using xpdm.Bitcoin.Scripting.Atoms;
using SCG = System.Collections.Generic;

namespace xpdm.Bitcoin.Core
{
    public sealed class Script : BitcoinObject, IFreezable, IThawable<Script>, IFormattable
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

        public static Script Parse(string scriptString)
        {
            var atoms = scriptString.Split(' ');
            var script = new Script();
            StringBuilder valueStr = null;
            foreach (var atom in atoms)
            {
                IScriptAtom newAtom = null;
                if (valueStr != null)
                {
                    valueStr.Append(" ");
                    if (atom.EndsWith("'"))
                    {
                        if (atom.EndsWith(@"\''"))
                        {
                            valueStr.Append(atom.Substring(0, atom.Length - 3));
                            valueStr.Append("'");
                            var valueBytes = Encoding.ASCII.GetBytes(valueStr.ToString());
                            newAtom = new ValueAtom(valueBytes);
                            script.Atoms.Add(newAtom);
                            valueStr = null;
                        }
                        else if (atom.EndsWith(@"\'"))
                        {
                            valueStr.Append(atom.Substring(0, atom.Length - 2));
                            valueStr.Append("'");
                        }
                        else
                        {
                            valueStr.Append(atom.Substring(0, atom.Length - 1));
                            var valueBytes = Encoding.ASCII.GetBytes(valueStr.ToString());
                            newAtom = new ValueAtom(valueBytes);
                            script.Atoms.Add(newAtom);
                            valueStr = null;
                        }
                    }
                    else
                    {
                        valueStr.Append(atom);
                    }
                    continue;
                }
                if (atom.StartsWith("OP_"))
                {
                    var opcode = (ScriptOpCode)Enum.Parse(typeof(ScriptOpCode), atom, false);
                    newAtom = ScriptAtomFactory.GetOpAtom(opcode);
                }
                else if (atom.StartsWith("'"))
                {
                    if (atom.EndsWith("'"))
                    {
                        if (atom.EndsWith(@"\''"))
                        {
                            var valueBytes = Encoding.ASCII.GetBytes(atom.Substring(1, atom.Length - 4) + "'");
                            newAtom = new ValueAtom(valueBytes);
                        }
                        else if (atom.EndsWith(@"\'"))
                        {
                            valueStr = new StringBuilder();
                            valueStr.Append(atom.Substring(1, atom.Length - 3));
                            valueStr.Append("'");
                        }
                        else if (atom.Length > 1)
                        {
                            if (atom.Length > 2)
                            {
                                var valueBytes = Encoding.ASCII.GetBytes(atom.Substring(1, atom.Length - 2));
                                newAtom = new ValueAtom(valueBytes);
                            }
                        }
                        else
                        {
                            valueStr = new StringBuilder();
                        }
                    }
                    else
                    {
                        valueStr = new StringBuilder();
                        valueStr.Append(atom.Substring(1, atom.Length - 1));
                    }
                }
                else if (atom.Length > 0)
                {
                    var valueBytes = BufferOperations.FromByteString(atom, Endianness.BigEndian);
                    newAtom = new ValueAtom(valueBytes);
                }
                if (newAtom != null)
                {
                    script.Atoms.Add(newAtom);
                }
            }
            if (valueStr != null)
            {
                throw new FormatException("Unclosed script string found");
            }
            script.Freeze();
            return script;
        }

        [Pure]
        public Script Subscript(int offset)
        {
            ContractsCommon.ValidOffset(0, Atoms.Count, offset);

            var script = new Script();
            script.Atoms.AddAll(this.Atoms.Skip(offset));
            return script;
        }

        [Pure]
        public Script Subscript(int offset, int length)
        {
            ContractsCommon.ValidOffsetLength(0, Atoms.Count, offset, length);

            var script = new Script();
            script.Atoms.AddAll(this.Atoms.Skip(offset).Take(length));
            return script;
        }

        private void AtomsChanged(object sender)
        {
            ContractsCommon.NotFrozen(this);

            InvalidateBitcoinHashes(sender, EventArgs.Empty);
        }

        protected sealed override void Deserialize(Stream stream)
        {
            var scriptSize = (uint)ReadVarInt(stream);
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
            Contract.Ensures(Contract.Result<string>() != null);

            return this.ToString(null, null);
        }

        public string ToString(string format)
        {
            Contract.Ensures(Contract.Result<string>() != null);

            return this.ToString(format, null);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            Contract.Ensures(Contract.Result<string>() != null);

            format = format ?? "";

            string scriptString;
            if (format.StartsWith("X", StringComparison.InvariantCultureIgnoreCase))
            {
                scriptString = this.SerializeToByteArray().ToByteString(Endianness.BigEndian).Substring(2);

                int length = 0;
                if (format.Length > 1)
                {
                    int.TryParse(format.Substring(1), out length);
                    if (length != 0)
                    {
                        scriptString = scriptString.Substring(0, length);
                    }
                }
            }
            else
            {
                scriptString = string.Join(" ", Atoms);

                if (format.StartsWith("S", StringComparison.InvariantCultureIgnoreCase))
                {
                    int length = 30;
                    if (format.Length > 1)
                    {
                        if (!int.TryParse(format.Substring(1), out length))
                        {
                            length = 30;
                        }
                    }
                    scriptString = scriptString.Substring(0, length);
                }
            }
            return scriptString;
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
