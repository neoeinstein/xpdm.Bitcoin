using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Numerics;
using System.Text;

namespace xpdm.Bitcoin
{
    public static class Base58Convert
    {
        private const string Alphabet = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
        private const int Base = 58;

        public static string Encode(byte[] plaintext)
        {
            Contract.Requires<ArgumentNullException>(plaintext != null);
            Contract.Ensures(Contract.Result<string>() != null);

            var workingValue = new BigInteger(plaintext);
            StringBuilder sb = new StringBuilder();
            while (workingValue.CompareTo(Base) >= 0)
            {
                BigInteger remainder;
                workingValue = BigInteger.DivRem(workingValue, Base, out remainder);
                sb.Insert(0, Alphabet[(int) remainder]);
            }
            sb.Insert(0, Alphabet[0]);

            foreach (byte b in plaintext)
            {
                if (b == 0)
                {
                    sb.Insert(0, Alphabet[(int)workingValue]);
                }
                else break;
            }

            return sb.ToString();
        }

        public static byte[] Decode(string enc)
        {
            Contract.Requires<ArgumentNullException>(enc != null);
            Contract.Ensures(Contract.Result<byte[]>() != null);

            return DecodeToBigInteger(enc).ToByteArray();
        }

        public static BigInteger DecodeToBigInteger(string enc)
        {
            Contract.Requires<ArgumentNullException>(enc != null);
            Contract.Ensures(Contract.Result<BigInteger>() >= 0);

            var workingValue = BigInteger.Zero;
            for (int i = 0; i <= enc.Length; ++i)
            {
                var index = new BigInteger(Alphabet.IndexOf(enc[i]));
                workingValue = workingValue + index*BigInteger.Pow(Base, enc.Length - 1 - i);
            }
            return workingValue;
        }
    }
}