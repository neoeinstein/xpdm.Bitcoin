using System;
using System.Diagnostics.Contracts;
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

            var plaintextArr = new byte[plaintext.Length + 1];
            Array.Copy(plaintext, plaintextArr, plaintext.Length);
            var workingValue = new BigInteger(plaintextArr);
            StringBuilder sb = new StringBuilder();
            while (workingValue.CompareTo(Base) >= 0)
            {
                BigInteger remainder;
                workingValue = BigInteger.DivRem(workingValue, Base, out remainder);
                sb.Insert(0, Alphabet[(int)remainder]);
            }
            sb.Insert(0, Alphabet[(int)workingValue]);

            foreach (byte b in plaintext)
            {
                if (b == 0)
                {
                    sb.Insert(0, Alphabet[0]);
                }
                else break;
            }

            return sb.ToString();
        }

        public static string EncodeWithCheck(byte[] plaintext)
        {
            var plaintextArr = (byte[])plaintext.Clone();
            Array.Reverse(plaintextArr);
            var hash = Cryptography.CryptoFunctionProviderFactory.Default.Hash256(plaintextArr);
            var plaintextExtended = new byte[plaintext.Length + 4];
            Array.Copy(plaintext, plaintextExtended, plaintext.Length);
            plaintextExtended[3] = hash[0];
            plaintextExtended[2] = hash[1];
            plaintextExtended[1] = hash[2];
            plaintextExtended[0] = hash[3];
            Array.Copy(hash.Bytes, 0, plaintextExtended, plaintext.Length, 4);
            return Encode(plaintextExtended);
        }

        public static byte[] Decode(string enc)
        {
            Contract.Requires<ArgumentNullException>(enc != null);
            Contract.Requires(Contract.ForAll(0, enc.Length, i => Alphabet.IndexOf(enc[i]) != -1));
            Contract.Ensures(Contract.Result<byte[]>() != null);

            return DecodeToBigInteger(enc).ToByteArray();
        }

        public static BigInteger DecodeToBigInteger(string enc)
        {
            Contract.Requires<ArgumentNullException>(enc != null);
            Contract.Requires(Contract.ForAll(0, enc.Length, i => Alphabet.IndexOf(enc[i]) != -1));
            //Contract.Ensures(Contract.Result<BigInteger>().ToByteArray()[Contract.Result<BigInteger>().ToByteArray().Length - 1] != 0);

            var workingValue = BigInteger.Zero;
            for (int i = 0; i < enc.Length; ++i)
            {
                var index = new BigInteger(Alphabet.IndexOf(enc[i]));
                workingValue = workingValue + index * BigInteger.Pow(Base, enc.Length - 1 - i);
            }
            var bytes = workingValue.ToByteArray();
            if (bytes[bytes.Length - 1] == 0 && workingValue > 0)
            {
                var newBytes = new byte[bytes.Length - 1];
                Array.Copy(bytes, newBytes, newBytes.Length);
                workingValue = new BigInteger(newBytes);
            }
            return workingValue;
        }

        public static byte[] DecodeWithCheck(string enc)
        {
            byte[] plaintext;
            return DecodeWithCheck(enc, out plaintext) ? plaintext : null;
        }

        public static bool DecodeWithCheck(string enc, out byte[] plaintext)
        {
            var plaintextExtended = Decode(enc);
            var hash = Cryptography.CryptoFunctionProviderFactory.Default.Hash256(plaintextExtended, 0, plaintextExtended.Length - 4).Bytes;
            if (hash[0] == plaintextExtended[plaintextExtended.Length - 4]
             && hash[1] == plaintextExtended[plaintextExtended.Length - 3]
             && hash[2] == plaintextExtended[plaintextExtended.Length - 2]
             && hash[3] == plaintextExtended[plaintextExtended.Length - 1])
            {
                var pt = new byte[plaintextExtended.Length - 4];
                Array.Copy(plaintextExtended, pt, pt.Length);
                plaintext = pt;
                return true;
            }
            else
            {
                var pt = new byte[plaintextExtended.Length - 4];
                Array.Copy(plaintextExtended, pt, pt.Length);
                plaintext = pt;
                //plaintext = new byte[0];
                return false;
            }
        }
    }
}