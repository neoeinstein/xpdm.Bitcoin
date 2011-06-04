using System.Reflection;
using Gallio.Framework;

namespace xpdm.Bitcoin.Tests.SerializedData
{
    public static class Access
    {
        public static byte[] GetSerializedData(string dataResourceName)
        {
            var resource = GetSerializedData(dataResourceName, Assembly.GetCallingAssembly())
                ?? GetSerializedData(dataResourceName, Assembly.GetExecutingAssembly());
            if (resource == null)
            {
                DiagnosticLog.WriteLine("Unable to find resource '{0}'", dataResourceName);
                DiagnosticLog.WriteLine(string.Join("\n", System.Reflection.Assembly.GetCallingAssembly().GetManifestResourceNames()));
                DiagnosticLog.WriteLine(string.Join("\n", System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames()));
                return null;
            }
            return resource;
        }

        public static byte[] GetSerializedData(string dataResourceName, Assembly resourceAssembly)
        {
            dataResourceName = resourceAssembly.GetName().Name + ".SerializedData." + dataResourceName;
            using (var resStream = resourceAssembly.GetManifestResourceStream(dataResourceName))
            {
                if (resStream == null)
                {
                    return null;
                }
                using (var reader = new System.IO.BinaryReader(resStream))
                {
                    return reader.ReadBytes((int)reader.BaseStream.Length);
                }
            }
        }
    }
}
