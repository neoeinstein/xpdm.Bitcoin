using System;

namespace xpdm.Bitcoin.Scripting
{
    [Flags]
    public enum SignatureHashType : byte
    {
        Unknown = 0x00,
        All = 0x01,
        None = 0x02,
        Single = 0x03,
        OutputMask = 0x1f,
        AnyoneCanPay = 0x80,
    }
}
