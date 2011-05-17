using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using C5;
using SCG = System.Collections.Generic;

namespace xpdm.Bitcoin.Core
{
    public sealed class ScriptBuilder : ScriptBase
    {
        public ScriptBuilder()
        {
            Atoms = new ArrayList<Scripting.IScriptAtom>();
        }

        public ScriptBuilder(ScriptBase script) : this(script.Atoms) { }
        public ScriptBuilder(SCG.IEnumerable<Scripting.IScriptAtom> atoms) : base(atoms) { }
        public ScriptBuilder(Stream stream) : base(stream) { }
        public ScriptBuilder(byte[] buffer, int offset) : base(buffer, offset) { }

        public ScriptBuilder Subscript(int offset, int length)
        {
            ContractsCommon.ValidOffsetLength(0, Atoms.Count, offset, length);

            return Subscript<ScriptBuilder>(offset, length);
        }
        
        public Script FreezeToScript()
        {
            ContractsCommon.ResultIsNonNull<Script>();
            Contract.Ensures(Contract.Result<Script>().Atoms.SequencedEquals(Atoms));

            return new Script(Atoms);
        }

        public static ScriptBuilder GenerateScriptToPublicKeyHash(Hash160 pubKeyHash)
        {
            ContractsCommon.NotNull(pubKeyHash, "pubKeyhash");
            ContractsCommon.ResultIsNonNull<ScriptBuilder>();
            Contract.Ensures(Contract.Result<ScriptBuilder>().Atoms.Count == 5);

            var sb = new ScriptBuilder();
            sb.Atoms.Add(xpdm.Bitcoin.Scripting.ScriptAtomFactory.GetOpAtom(xpdm.Bitcoin.Scripting.ScriptOpCode.OP_DUP));
            sb.Atoms.Add(xpdm.Bitcoin.Scripting.ScriptAtomFactory.GetOpAtom(xpdm.Bitcoin.Scripting.ScriptOpCode.OP_HASH160));
            sb.Atoms.Add(new xpdm.Bitcoin.Scripting.Atoms.ValueAtom(pubKeyHash.Bytes));
            sb.Atoms.Add(xpdm.Bitcoin.Scripting.ScriptAtomFactory.GetOpAtom(xpdm.Bitcoin.Scripting.ScriptOpCode.OP_EQUALVERIFY));
            sb.Atoms.Add(new xpdm.Bitcoin.Scripting.Atoms.OpCheckSigAtom());
            return sb;
        }
    }
}
