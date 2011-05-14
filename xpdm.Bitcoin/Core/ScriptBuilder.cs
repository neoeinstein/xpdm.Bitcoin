using System;
using SCG = System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using C5;
using System.IO;

namespace xpdm.Bitcoin.Core
{
    public sealed class ScriptBuilder : BitcoinObject
    {
        public IList<Scripting.IScriptAtom> Atoms { get; private set; }

        public ScriptBuilder()
        {
            Atoms = new ArrayList<Scripting.IScriptAtom>();
        }

        public Script FreezeToScript()
        {
            return new Script(Atoms);
        }

        protected override void Deserialize(Stream stream)
        {
            throw new NotImplementedException();
        }

        public override void Serialize(Stream stream)
        {
            throw new NotImplementedException();
        }

        public override int SerializedByteSize
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string ToString()
        {
            return string.Join(" ", Atoms);
        }
    }
}
