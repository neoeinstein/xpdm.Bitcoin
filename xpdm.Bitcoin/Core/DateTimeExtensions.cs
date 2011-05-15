using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xpdm.Bitcoin.Core
{
    public static class DateTimeExtensions
    {
        public static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime FromSecondsSinceEpoch(uint seconds)
        {
            return Epoch.AddSeconds(seconds);
        }

        public static uint ToSecondsSinceEpoch(DateTime dateTime)
        {
            return (uint)(dateTime - Epoch).TotalSeconds;
        }
    }
}
