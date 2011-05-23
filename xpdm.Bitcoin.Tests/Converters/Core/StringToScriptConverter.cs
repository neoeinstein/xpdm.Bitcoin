using System;
using MbUnit.Framework;
using xpdm.Bitcoin.Core;
using xpdm.Bitcoin.Scripting;
using xpdm.Bitcoin.Scripting.Atoms;

namespace xpdm.Bitcoin.Tests.Converters.Core
{
    public static class StringToScriptConverter
    {
        [Converter]
        public static Script ConvertStringToScript(string s)
        {
            var atoms = s.Split(' ');
            var script = new Script();
            foreach (var atom in atoms)
            {
                IScriptAtom newAtom;
                if (atom.StartsWith("OP_"))
                {
                    var opcode = (ScriptOpCode)Enum.Parse(typeof(ScriptOpCode), atom, false);
                    newAtom = ScriptAtomFactory.GetOpAtom(opcode);
                }
                else
                {
                    var valueBytes = BufferOperations.FromByteString(atom, Endianness.BigEndian);
                    newAtom = new ValueAtom(valueBytes);
                }
                script.Atoms.Add(newAtom);
            }
            script.Freeze();
            return script;
        }
    }
}
