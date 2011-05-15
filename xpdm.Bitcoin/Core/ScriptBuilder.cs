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

        public static ScriptBuilder GenerateScriptToPublicKeyHash(Hash160 pubKeyHash)
        {
            var sb = new ScriptBuilder();
            sb.Atoms.Add(xpdm.Bitcoin.Scripting.ScriptAtomFactory.GetOpAtom(xpdm.Bitcoin.Scripting.ScriptOpCode.OP_DUP));
            sb.Atoms.Add(xpdm.Bitcoin.Scripting.ScriptAtomFactory.GetOpAtom(xpdm.Bitcoin.Scripting.ScriptOpCode.OP_HASH160));
            sb.Atoms.Add(new xpdm.Bitcoin.Scripting.Atoms.ValueAtom(pubKeyHash.Bytes));
            sb.Atoms.Add(xpdm.Bitcoin.Scripting.ScriptAtomFactory.GetOpAtom(xpdm.Bitcoin.Scripting.ScriptOpCode.OP_EQUALVERIFY));
            sb.Atoms.Add(new xpdm.Bitcoin.Scripting.Atoms.OpCheckSigAtom());
            return sb;
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
