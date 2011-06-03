using Gallio.Runtime;
using Gallio.Runtime.Conversions;
using Gallio.Runtime.Extensibility;
using Gallio.Runtime.Formatting;
using MbUnit.Framework;
using xpdm.Bitcoin.Core;
using xpdm.Bitcoin.Tests.Converters;
using xpdm.Bitcoin.Tests.Converters.Core;
using xpdm.Bitcoin.Tests.Formatters;

namespace xpdm.Bitcoin.Messaging.Tests
{
    /// <summary>
    /// Promote reuse of core converters and formatters by re-registering here.
    /// </summary>
    [AssemblyFixture]
    class AssemblyFixture
    {
        private static CustomFormatters formatters = new CustomFormatters();
        private static CustomConverters converters = new CustomConverters();
        private static IExtensionPoints extensionPoints;

        [FixtureSetUp]
        public static void FixtureSetUp()
        {
            extensionPoints = (IExtensionPoints)RuntimeAccessor.ServiceLocator.ResolveByComponentId("Gallio.ExtensionPoints");

            extensionPoints.CustomFormatters.Register<byte[]>(ByteArrayFormatter.FormatByteArray);
            extensionPoints.CustomConverters.Register<string, byte[]>(StringToByteArrayConverter.ConvertStringToByteArray);
            extensionPoints.CustomConverters.Register<string, Script>(StringToScriptConverter.ConvertStringToScript);
        }

        [FixtureTearDown]
        public static void FixtureTearDown()
        {
            extensionPoints.CustomFormatters.Unregister<byte[]>();
            extensionPoints.CustomConverters.Unregister<string, byte[]>();
            extensionPoints.CustomConverters.Unregister<string, Script>();
        }
    }
}
