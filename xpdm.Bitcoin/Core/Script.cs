using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using C5;
using SCG = System.Collections.Generic;

namespace xpdm.Bitcoin.Core
{
    public sealed class Script : ScriptBase
    {
        public override IList<Scripting.IScriptAtom> Atoms
        {
            get
            {
                return base.Atoms;
            }
            protected set
            {
                base.Atoms = new GuardedList<Scripting.IScriptAtom>(value);
            }
        }

        public Script()
        {
            Atoms = new WrappedArray<Scripting.IScriptAtom>(new Scripting.IScriptAtom[0]);
        }

        public Script(SCG.IEnumerable<Scripting.IScriptAtom> atoms) : base(atoms) { }
        public Script(Stream stream) : base(stream) { }
        public Script(byte[] buffer, int offset) : base(buffer, offset) { }

        public Script Subscript(int index, int length)
        {
            Contract.Requires<ArgumentOutOfRangeException>(0 <= index && index < Atoms.Count, "index");
            Contract.Requires<ArgumentOutOfRangeException>(0 <= length && length <= Atoms.Count, "length");
            Contract.Requires<ArgumentOutOfRangeException>(index + length <= Atoms.Count, "length");

            return Subscript<Script>(index, length);
        }
    }
}
