using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin.Core
{
    public struct Timestamp : IEquatable<Timestamp>, IComparable<Timestamp>, IComparable, IFormattable
    {
        public Timestamp(uint seconds)
        {
            _secondsSinceEpoch = seconds;
        }
        public Timestamp(DateTime datetime) : this((uint)(datetime - EpochDateTime).TotalSeconds)
        {
            Contract.Requires<ArgumentOutOfRangeException>(MinValue.DateTime <= datetime && datetime <= MaxValue.DateTime, "datetime");
        }

        private readonly uint _secondsSinceEpoch;
        public uint SecondsSinceEpoch
        {
            get
            {
                return _secondsSinceEpoch;
            }
        }
        public DateTime DateTime
        {
            get
            {
                return EpochDateTime.AddSeconds(SecondsSinceEpoch);
            }
        }

        public static implicit operator uint(Timestamp timestamp)
        {
            return timestamp.SecondsSinceEpoch;
        }
        public static implicit operator DateTime(Timestamp timestamp)
        {
            return timestamp.DateTime;
        }
        public static implicit operator Timestamp(uint seconds)
        {
            return new Timestamp(seconds);
        }
        public static explicit operator Timestamp(DateTime datetime)
        {
            return new Timestamp(datetime);
        }

        [Pure]
        public int CompareTo(Timestamp other)
        {
            return SecondsSinceEpoch.CompareTo(other.SecondsSinceEpoch);
        }

        [Pure]
        public int CompareTo(object obj)
        {
            return (obj is Timestamp ? this.CompareTo((Timestamp)obj) : 1);
        }

        [Pure]
        public bool Equals(Timestamp other)
        {
            return SecondsSinceEpoch.Equals(other.SecondsSinceEpoch);
        }

        [Pure]
        public override bool Equals(object obj)
        {
            return (obj is Timestamp) && this.Equals((Timestamp)obj);
        }

        [Pure]
        public override int GetHashCode()
        {
            return SecondsSinceEpoch.GetHashCode();
        }

        [Pure]
        public override string ToString()
        {
            Contract.Ensures(Contract.Result<string>() != null);

            return this.ToString(null, null);
        }

        [Pure]
        public string ToString(string format)
        {
            Contract.Ensures(Contract.Result<string>() != null);

            return this.ToString(format, null);
        }

        private const string _defaultFormat = "Ts";

        [Pure]
        public string ToString(string format, IFormatProvider formatProvider)
        {
            ContractsCommon.ResultIsNonNull<string>();

            if (format == null)
            {
                format = _defaultFormat;
            }
            if (format.StartsWith("T", StringComparison.InvariantCultureIgnoreCase))
            {
                return DateTime.ToString(format.Substring(1), formatProvider);
            }

            return SecondsSinceEpoch.ToString(format, formatProvider);
        }

        private static readonly DateTime EpochDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static readonly Timestamp MinValue = new Timestamp(uint.MinValue);
        public static readonly Timestamp MaxValue = new Timestamp(uint.MaxValue);
    }
}
