using System;
using System.Collections.Generic;
using System.Numerics;

namespace xpdm.Bitcoin
{
    public sealed class ByteArrayComparer : Comparer<byte[]>, IEqualityComparer<byte[]>
    {
        public static readonly ByteArrayComparer Instance = new ByteArrayComparer();

        public override int Compare(byte[] x, byte[] y)
        {
            if (x == null && y == null)
            {
                return 0;
            }
            if (x == null)
            {
                return -1;
            }
            if (y == null)
            {
                return 1;
            }

            int compare = 0;
            int i = Math.Max(x.Length, y.Length) - 1;
            while (compare == 0 && i >= 0)
            {
                compare = (i < x.Length ? x[i] : (byte)0).CompareTo(i < y.Length? y[i] : (byte)0);
                --i;
            }
            return compare;
        }

        public bool Equals(byte[] x, byte[] y)
        {
            return Compare(x, y) == 0;
        }

        public int GetHashCode(byte[] obj)
        {
            ContractsCommon.NotNull(obj, "obj");

            int hashCode = 0;
            foreach (byte b in obj)
            {
                hashCode = (hashCode * 297) ^ obj.GetHashCode();
            }
            return hashCode;
        }
    }
}
