using MbUnit.Framework;
using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Tests.Converters.Core
{
    public static class StringToScriptConverter
    {
        [Converter]
        public static Script ConvertStringToScript(string s)
        {
            return Script.Parse(s);
        }
    }
}
