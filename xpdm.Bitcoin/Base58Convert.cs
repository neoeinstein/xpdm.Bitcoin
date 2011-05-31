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
            ContractsCommon.NotNull(plaintext, "plaintext");
            ContractsCommon.ResultIsNonNull<string>();

            var plaintextArr = new byte[plaintext.Length + 1];
            Array.Copy(plaintext, 0, plaintextArr, 1, plaintext.Length);
            Array.Reverse(plaintextArr);
            var workingValue = new BigInteger(plaintextArr);
            StringBuilder sb = new StringBuilder(plaintext.Length * 138 / 100 + 1);
            while (workingValue.CompareTo(BigInteger.Zero) > 0)
            {
                BigInteger remainder;
                workingValue = BigInteger.DivRem(workingValue, Base, out remainder);
                sb.Append(Alphabet[(int)remainder]);
            }
            Contract.Assert(workingValue.Sign >= 0);
            //sb.Insert(0, Alphabet[(int)workingValue]);

            for (int i = 0; i < plaintext.Length && plaintext[i] == 0; ++i)
            {
                sb.Append(Alphabet[0]);
            }

            var retVal = new char[sb.Length];
            sb.CopyTo(0, retVal, 0, sb.Length);
            Array.Reverse(retVal);
            return new string(retVal);
        }

        public static string EncodeWithCheck(byte[] plaintext)
        {
            ContractsCommon.NotNull(plaintext, "plaintext");
            ContractsCommon.ResultIsNonNull<string>();

            return EncodeWithCheck(new byte[0], plaintext);
        }

        public static string EncodeWithCheck(byte[] prefix, byte[] plaintext)
        {
            ContractsCommon.NotNull(prefix, "prefix");
            ContractsCommon.NotNull(plaintext, "plaintext");
            ContractsCommon.ResultIsNonNull<string>();

            var plaintextArr = (byte[])plaintext.Clone();
            var hash = Cryptography.CryptoFunctionProviderFactory.Default.Hash256(plaintextArr, prefix);
            var plaintextExtended = new byte[plaintext.Length + prefix.Length + 4];
            Array.Copy(plaintext, 0, plaintextExtended, 0, plaintext.Length);
            Array.Copy(prefix, 0, plaintextExtended, plaintext.Length, prefix.Length);
            Array.Copy(hash.Bytes, 0, plaintextExtended, prefix.Length + plaintext.Length, 4);
            return Encode(plaintextExtended);
        }

        public static byte[] Decode(string enc)
        {
            ContractsCommon.NotNull(enc, "enc");
            Contract.Requires<FormatException>(Contract.ForAll(0, enc.Length, i => Alphabet.IndexOf(enc[i]) != -1));
            ContractsCommon.ResultIsNonNull<byte[]>();

            if (enc.Length == 0)
            {
                return new byte[0];
            }

            var workingValue = BigInteger.Zero;
            for (int i = 0; i < enc.Length; ++i)
            {
                var index = new BigInteger(Alphabet.IndexOf(enc[i]));
                workingValue = workingValue + index * BigInteger.Pow(Base, enc.Length - 1 - i);
            }

            var retVal = workingValue.ToByteArray();
            Array.Reverse(retVal);
            if (retVal[0] == 0 && workingValue > 0)
            {
                var newBytes = new byte[retVal.Length - 1];
                Array.Copy(retVal, 1, newBytes, 0, newBytes.Length);
                retVal = newBytes;
            }

            var count = 0;
            while (enc[count] == Alphabet[0]) ++count;

            if (count > 0)
            {
                var newBytes = new byte[retVal.Length + count];
                Array.Copy(retVal, 0, newBytes, count, retVal.Length);
                retVal = newBytes;
            }

            return retVal;
        }

        public static byte[] DecodeWithCheck(string enc)
        {
            ContractsCommon.NotNull(enc, "enc");
            Contract.Requires<FormatException>(Contract.ForAll(0, enc.Length, i => Alphabet.IndexOf(enc[i]) != -1));

            byte[] plaintext;
            return DecodeWithCheck(enc, out plaintext) ? plaintext : null;
        }

        public static bool DecodeWithCheck(string enc, out byte[] plaintext)
        {
            ContractsCommon.NotNull(enc, "enc");
            Contract.Requires<FormatException>(Contract.ForAll(0, enc.Length, i => Alphabet.IndexOf(enc[i]) != -1));
            Contract.Ensures(Contract.Result<bool>() == false || Contract.ValueAtReturn(out plaintext) != null);

            byte[] prefix;
            return DecodeWithCheck(enc, 0, out prefix, out plaintext);
        }

        public static bool DecodeWithCheck(string enc, int prefixLength, out byte[] prefix, out byte[] plaintext)
        {
            ContractsCommon.NotNull(enc, "enc");
            // Approximate decoded byte size = enc.Length * 100 / 138 + 1
            // Plaintext may be all prefix (-0)
            // Prefix may not include bytes reserved for hash (-4)
            ContractsCommon.ValidLength(0, (enc.Length) * 100 / 138 - 3, prefixLength, "prefixLength");
            Contract.Requires<FormatException>(Contract.ForAll(0, enc.Length, i => Alphabet.IndexOf(enc[i]) != -1));
            Contract.Ensures(Contract.Result<bool>() == false || Contract.ValueAtReturn(out prefix) != null && Contract.ValueAtReturn(out prefix).Length == prefixLength);
            Contract.Ensures(Contract.Result<bool>() == false || Contract.ValueAtReturn(out plaintext) != null);

            var plaintextExtended = Decode(enc);
            if (plaintextExtended.Length < 4 + prefixLength)
            {
                prefix = new byte[0];
                plaintext = plaintextExtended;
                return false;
            }

            var hash = Cryptography.CryptoFunctionProviderFactory.Default.Hash256(plaintextExtended, 0, plaintextExtended.Length - 4).Bytes;

            var pre = new byte[prefixLength];
            Array.Copy(plaintextExtended, pre, prefixLength);
            prefix = pre;

            var pt = new byte[plaintextExtended.Length - prefixLength - 4];
            Array.Copy(plaintextExtended, prefixLength, pt, 0, pt.Length);
            plaintext = pt;

            return (hash[0] == plaintextExtended[plaintextExtended.Length - 4]
                    && hash[1] == plaintextExtended[plaintextExtended.Length - 3]
                    && hash[2] == plaintextExtended[plaintextExtended.Length - 2]
                    && hash[3] == plaintextExtended[plaintextExtended.Length - 1]);
        }
    }
}