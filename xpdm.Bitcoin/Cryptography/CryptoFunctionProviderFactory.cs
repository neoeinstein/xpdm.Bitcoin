using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Cryptography
{
    public static class CryptoFunctionProviderFactory
    {
        public static ICryptoFunctionProvider GetProvider(string typeName)
        {
            ContractsCommon.NotNull(typeName, "typeName");

            var type = Type.GetType(typeName, true);
            var constructor = type.GetConstructor(new Type[0]);
            if (constructor == null || !type.IsSubclassOf(typeof(ICryptoFunctionProvider)))
            {
                throw new ArgumentException("Unable to instantiate type " + typeName);
            }
            return (ICryptoFunctionProvider)constructor.Invoke(new object[0]);
        }

        public static readonly ICryptoFunctionProvider Default = new BouncyCastleCryptoFunctionProvider();
    }
}
