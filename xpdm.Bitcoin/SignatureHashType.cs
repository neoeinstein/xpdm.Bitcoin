using System;

namespace xpdm.Bitcoin
{
    [Flags]
    public enum SignatureHashType : byte
    {
        All = 0x00,
        None = 0x01,
        Single = 0x02,
        OutputMask = 0x1f,
        AnyoneCanPay = 0x80,
    }
}
