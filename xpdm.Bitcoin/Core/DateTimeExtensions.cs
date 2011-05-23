using System;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin.Core
{
    public static class DateTimeExtensions
    {
        public static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static readonly DateTime MaxValue = Epoch.AddSeconds(uint.MaxValue);

        [Pure]
        public static DateTime FromSecondsSinceEpoch(uint seconds)
        {
            return Epoch.AddSeconds(seconds);
        }

        [Pure]
        public static uint ToSecondsSinceEpoch(DateTime dateTime)
        {
            Contract.Requires<ArgumentOutOfRangeException>(Epoch <= dateTime && dateTime <= MaxValue);
            // The significand of a double contains 52 bits. This is enough to
            // contain a complete uint (at 32 bits) up to its max value without
            // loss of resolution. This ensures that this transformation is
            // safe. Use checked here to prevent silent overflows for dates in
            // the far future or before the UNIX Epoch.
            return checked((uint)(dateTime - Epoch).TotalSeconds);
        }
    }
}
