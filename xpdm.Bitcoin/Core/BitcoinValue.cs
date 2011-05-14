using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Numerics;
using System.Text;

namespace xpdm.Bitcoin.Core
{
    [Serializable]
    public struct BitcoinValue : IComparable, IComparable<BitcoinValue>, IEquatable<BitcoinValue>, IFormattable
    {
        public static readonly long NanoCoinsPerWholeCoin = 100000000;
        public static readonly BitcoinValue OneBitcent = new BitcoinValue(NanoCoinsPerWholeCoin / 100);
        public static readonly BitcoinValue OneBitcoin = new BitcoinValue(NanoCoinsPerWholeCoin);

        public BigInteger NanoCoins { get; private set; }
        public decimal WholeCoins
        {
            get { return (decimal) NanoCoins / NanoCoinsPerWholeCoin; }
        }

        public BitcoinValue(BigInteger value) : this()
        {
            NanoCoins = value;
        }

        public BitcoinValue(long value) : this()
        {
            NanoCoins = value;
        }

        public BitcoinValue(ulong value) : this()
        {
            NanoCoins = value;
        }

        public static BitcoinValue FromWholeCoins(decimal wholeCoins)
        {
            return new BitcoinValue(new BigInteger(wholeCoins * NanoCoinsPerWholeCoin));
        }

        public static BitcoinValue FromWholeCoins(double wholeCoins)
        {
            return new BitcoinValue(new BigInteger((decimal)wholeCoins * NanoCoinsPerWholeCoin));
        }

        public static BitcoinValue FromWholeCoins(long wholeCoins)
        {
            return new BitcoinValue(new BigInteger(wholeCoins * NanoCoinsPerWholeCoin));
        }

        public static BitcoinValue FromWholeCoins(ulong wholeCoins)
        {
            return new BitcoinValue(new BigInteger(wholeCoins * (ulong)NanoCoinsPerWholeCoin));
        }

        #region System.Object overrides

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is BitcoinValue))
                return false;
            return this.Equals((BitcoinValue) obj);
        }

        public override int GetHashCode()
        {
            return NanoCoins.GetHashCode();
        }

        public override string ToString()
        {
            Contract.Ensures(Contract.Result<string>() != null);

            return this.ToString(null, null);
        }

        public string ToString(string format)
        {
            Contract.Ensures(Contract.Result<string>() != null);

            return this.ToString(format, null);
        }

        #endregion

        #region IFormattable Members

        private const string _defaultFormat = "0.00########";

        public string ToString(string format, IFormatProvider formatProvider)
        {
            Contract.Ensures(Contract.Result<string>() != null);

            if (format == null)
            {
                format = _defaultFormat;
            }
            if (format.StartsWith("N", StringComparison.InvariantCultureIgnoreCase))
            {
                return NanoCoins.ToString(format.Substring(1), formatProvider);
            }

            return WholeCoins.ToString(format, formatProvider);
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj == null || !(obj is BitcoinValue))
                return -1;
            return this.CompareTo((BitcoinValue)obj);
        }

        #endregion

        #region IComparable<BitcoinValue> Members

        public int CompareTo(BitcoinValue other)
        {
            Contract.Ensures(-1 <= Contract.Result<int>() && Contract.Result<int>() <= 1);

            return NanoCoins.CompareTo(other);
        }

        #endregion

        #region IEquatable<BitcoinValue> Members

        public bool Equals(BitcoinValue other)
        {
            Contract.Ensures(Contract.Result<bool>() == false || this.GetHashCode() == other.GetHashCode());

            return NanoCoins.Equals(other.NanoCoins);
        }

        #endregion

        #region Operators

        #region Arithmetic Operators

        public static BitcoinValue operator +(BitcoinValue a)
        {
            return new BitcoinValue(a.NanoCoins);
        }

        public static BitcoinValue operator -(BitcoinValue a)
        {
            return new BitcoinValue(-a.NanoCoins);
        }

        public static BitcoinValue operator +(BitcoinValue a, BitcoinValue b)
        {
            return new BitcoinValue(a.NanoCoins + b.NanoCoins);
        }

        public static BitcoinValue operator -(BitcoinValue a, BitcoinValue b)
        {
            return new BitcoinValue(a.NanoCoins - b.NanoCoins);
        }

        public static BitcoinValue operator *(BitcoinValue a, decimal b)
        {
            return new BitcoinValue(new BigInteger((decimal)a.NanoCoins * b));
        }

        public static BitcoinValue operator *(decimal a, BitcoinValue b)
        {
            return new BitcoinValue(new BigInteger(a * (decimal)b.NanoCoins));
        }

        public static BitcoinValue operator *(BitcoinValue a, double b)
        {
            return new BitcoinValue(new BigInteger((decimal)a.NanoCoins * (decimal)b));
        }

        public static BitcoinValue operator *(double a, BitcoinValue b)
        {
            return new BitcoinValue(new BigInteger((decimal)a * (decimal)b.NanoCoins));
        }

        public static BitcoinValue operator *(BitcoinValue a, ulong b)
        {
            return new BitcoinValue(a.NanoCoins * b);
        }

        public static BitcoinValue operator *(BitcoinValue a, long b)
        {
            return new BitcoinValue(a.NanoCoins * b);
        }

        public static BitcoinValue operator *(ulong a, BitcoinValue b)
        {
            return new BitcoinValue(a * b.NanoCoins);
        }

        public static BitcoinValue operator *(long a, BitcoinValue b)
        {
            return new BitcoinValue(a * b.NanoCoins);
        }

        public static BitcoinValue operator /(BitcoinValue a, decimal b)
        {
            return new BitcoinValue(new BigInteger((decimal)a.NanoCoins / b));
        }

        public static BitcoinValue operator /(decimal a, BitcoinValue b)
        {
            return new BitcoinValue(new BigInteger(a / (decimal)b.NanoCoins));
        }

        public static BitcoinValue operator /(BitcoinValue a, double b)
        {
            return new BitcoinValue(new BigInteger((decimal)a.NanoCoins / (decimal)b));
        }

        public static BitcoinValue operator /(double a, BitcoinValue b)
        {
            return new BitcoinValue(new BigInteger((decimal)a / (decimal)b.NanoCoins));
        }

        public static BitcoinValue operator /(BitcoinValue a, ulong b)
        {
            return new BitcoinValue(a.NanoCoins / b);
        }

        public static BitcoinValue operator /(BitcoinValue a, long b)
        {
            return new BitcoinValue(a.NanoCoins / b);
        }

        public static BitcoinValue operator /(ulong a, BitcoinValue b)
        {
            return new BitcoinValue(a / b.NanoCoins);
        }

        public static BitcoinValue operator /(long a, BitcoinValue b)
        {
            return new BitcoinValue(a / b.NanoCoins);
        }


        #endregion

        #region Comparison Operators

        public static bool operator ==(BitcoinValue a, BitcoinValue b)
        {
            return a.Equals(b);
        }

        public static bool operator ==(BitcoinValue a, long b)
        {
            return a.NanoCoins == b;
        }

        public static bool operator ==(long a, BitcoinValue b)
        {
            return b.NanoCoins == a;
        }

        public static bool operator ==(BitcoinValue a, ulong b)
        {
            return a.NanoCoins == b;
        }

        public static bool operator ==(ulong a, BitcoinValue b)
        {
            return b.NanoCoins == a;
        }

        public static bool operator !=(BitcoinValue a, BitcoinValue b)
        {
            return !a.Equals(b);
        }

        public static bool operator !=(BitcoinValue a, long b)
        {
            return a.NanoCoins != b;
        }

        public static bool operator !=(long a, BitcoinValue b)
        {
            return b.NanoCoins != a;
        }

        public static bool operator !=(BitcoinValue a, ulong b)
        {
            return a.NanoCoins != b;
        }

        public static bool operator !=(ulong a, BitcoinValue b)
        {
            return b.NanoCoins != a;
        }

        public static bool operator <=(BitcoinValue a, BitcoinValue b)
        {
            return a.CompareTo(b) <= 0;
        }

        public static bool operator <=(BitcoinValue a, long b)
        {
            return a.NanoCoins <= b;
        }

        public static bool operator <=(long a, BitcoinValue b)
        {
            return b.NanoCoins >= a;
        }

        public static bool operator <=(BitcoinValue a, ulong b)
        {
            return a.NanoCoins <= b;
        }

        public static bool operator <=(ulong a, BitcoinValue b)
        {
            return b.NanoCoins >= a;
        }

        public static bool operator >=(BitcoinValue a, BitcoinValue b)
        {
            return a.CompareTo(b) >= 0;
        }

        public static bool operator >=(BitcoinValue a, long b)
        {
            return a.NanoCoins >= b;
        }

        public static bool operator >=(long a, BitcoinValue b)
        {
            return b.NanoCoins <= a;
        }

        public static bool operator >=(BitcoinValue a, ulong b)
        {
            return a.NanoCoins >= b;
        }

        public static bool operator >=(ulong a, BitcoinValue b)
        {
            return b.NanoCoins <= a;
        }

        public static bool operator <(BitcoinValue a, BitcoinValue b)
        {
            return a.CompareTo(b) < 0;
        }

        public static bool operator <(BitcoinValue a, long b)
        {
            return a.NanoCoins < b;
        }

        public static bool operator <(long a, BitcoinValue b)
        {
            return b.NanoCoins > a;
        }

        public static bool operator <(BitcoinValue a, ulong b)
        {
            return a.NanoCoins < b;
        }

        public static bool operator <(ulong a, BitcoinValue b)
        {
            return b.NanoCoins > a;
        }

        public static bool operator >(BitcoinValue a, BitcoinValue b)
        {
            return a.CompareTo(b) > 0;
        }

        public static bool operator >(BitcoinValue a, long b)
        {
            return a.NanoCoins > b;
        }

        public static bool operator >(long a, BitcoinValue b)
        {
            return b.NanoCoins < a;
        }

        public static bool operator >(BitcoinValue a, ulong b)
        {
            return a.NanoCoins > b;
        }

        public static bool operator >(ulong a, BitcoinValue b)
        {
            return b.NanoCoins < a;
        }

        #endregion

        #region Conversion Operators

        public static implicit operator BitcoinValue(byte value)
        {
            return new BitcoinValue(value);
        }

        public static implicit operator BitcoinValue(sbyte value)
        {
            return new BitcoinValue(value);
        }

        public static implicit operator BitcoinValue(ushort value)
        {
            return new BitcoinValue(value);
        }

        public static implicit operator BitcoinValue(short value)
        {
            return new BitcoinValue(value);
        }

        public static implicit operator BitcoinValue(uint value)
        {
            return new BitcoinValue(value);
        }

        public static implicit operator BitcoinValue(int value)
        {
            return new BitcoinValue(value);
        }

        public static implicit operator BitcoinValue(ulong value)
        {
            return new BitcoinValue(value);
        }

        public static implicit operator BitcoinValue(long value)
        {
            return new BitcoinValue(value);
        }

        public static implicit operator BitcoinValue(BigInteger value)
        {
            return new BitcoinValue(value);
        }

        public static explicit operator byte(BitcoinValue value)
        {
            return (byte)value.NanoCoins;
        }

        public static explicit operator sbyte(BitcoinValue value)
        {
            return (sbyte)value.NanoCoins;
        }

        public static explicit operator ushort(BitcoinValue value)
        {
            return (ushort)value.NanoCoins;
        }

        public static explicit operator short(BitcoinValue value)
        {
            return (short)value.NanoCoins;
        }

        public static explicit operator uint(BitcoinValue value)
        {
            return (uint)value.NanoCoins;
        }

        public static explicit operator int(BitcoinValue value)
        {
            return (int)value.NanoCoins;
        }

        public static explicit operator ulong(BitcoinValue value)
        {
            return (ulong)value.NanoCoins;
        }

        public static explicit operator long(BitcoinValue value)
        {
            return (long)value.NanoCoins;
        }

        public static implicit operator BigInteger(BitcoinValue value)
        {
            return value.NanoCoins;
        }

        #endregion

        #endregion
    }
}
