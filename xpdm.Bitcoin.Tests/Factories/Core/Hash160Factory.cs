using System;
using MbUnit.Framework;
using xpdm.Bitcoin.Core;
using System.Collections.Generic;

namespace xpdm.Bitcoin.Tests.Factories.Core
{
    public static class Hash160Factory
    {
        public static Hash160 Create(byte[] hashBytes)
        {
            Hash160 hash160 = new Hash160(hashBytes);
            return hash160;
        }

        public static IEnumerable<Hash160> RandomValidHashSet()
        {
            yield return Hash160.Empty;
            yield return Create(new byte[]
                {
                    0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                    0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                });
            yield return Create(new byte[]
                {
                    0x87, 0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80, 0x03,
                    0x81, 0x07, 0x0E, 0x1C, 0x38, 0x70, 0xE0, 0xC1, 0x83, 0x0F,
                });
            yield return Create(new byte[]
                {
                    0x94, 0x32, 0x93, 0x43, 0x93, 0x24, 0x93, 0x18, 0x91, 0x32,
                    0xDE, 0xAD, 0xBE, 0xEF, 0xFE, 0xED, 0xFA, 0xCE, 0x3D, 0x29
                });
        }
    }
}
