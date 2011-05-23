using System.Security.Cryptography;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace xpdm.Bitcoin.Cryptography
{
    public sealed class ECDsaBouncyCastle : ISigningKey
    {
        private ECPublicKeyParameters _publicKey;
        private ECPrivateKeyParameters _privateKey;

        private static readonly ECDomainParameters s_secp256k1;
        public static ECDomainParameters SecP256k1
        {
            get
            {
                return new ECDomainParameters(s_secp256k1.Curve, s_secp256k1.G, s_secp256k1.N, s_secp256k1.H, s_secp256k1.GetSeed());
            }
        }

        static ECDsaBouncyCastle()
        {
            var secp256k1 = SecNamedCurves.GetByName("secp256k1");
            s_secp256k1 = new ECDomainParameters(secp256k1.Curve, secp256k1.G, secp256k1.N, secp256k1.H, secp256k1.GetSeed());
        }

        public ECDsaBouncyCastle()
        {
            var secp256k1 = SecP256k1;
            var privateKey = new byte[secp256k1.N.BitLength];
            BigInteger d;

            using (var cryptoRng = RandomNumberGenerator.Create())
            {
                do
                {
                    cryptoRng.GetBytes(privateKey);
                    d = new BigInteger(1, privateKey);
                }
                while (d.SignValue == 0 || (d.CompareTo(secp256k1.N) >= 0));

            }

            _publicKey = new ECPublicKeyParameters(secp256k1.G.Multiply(d), secp256k1);
            _privateKey = new ECPrivateKeyParameters(d, secp256k1);
        }

        public ECDsaBouncyCastle(byte[] encodedKey, bool isPrivateKey)
        {
            ContractsCommon.NotNull(encodedKey, "encodedKey");

            var secp256k1 = SecP256k1;
            if (isPrivateKey)
            {
                var d = new BigInteger(encodedKey);
                _publicKey = new ECPublicKeyParameters(secp256k1.G.Multiply(d), secp256k1);
                _privateKey = new ECPrivateKeyParameters(d, secp256k1);
            }
            else
            {
                _publicKey = new ECPublicKeyParameters(secp256k1.Curve.DecodePoint(encodedKey), secp256k1);
            }
        }


        public ECDsaBouncyCastle(byte[] encodedPublicKey)
        {
            ContractsCommon.NotNull(encodedPublicKey, "encodedPublicKey");

            var secp256k1 = SecP256k1;
            _publicKey = new ECPublicKeyParameters(secp256k1.Curve.DecodePoint(encodedPublicKey), secp256k1);
        }

        public bool HasPrivateKey
        {
            get { return _privateKey != null; }
        }

        public byte[] RetreivePrivateKey()
        {
            ContractsCommon.ResultIsNonNull<byte[]>();

            return _privateKey.D.ToByteArray();
        }

        public byte[] PublicKey
        {
            get
            {
                ContractsCommon.ResultIsNonNull<byte[]>();

                return _publicKey.Q.GetEncoded();
            }
        }

        public byte[] SignHash(byte[] hash)
        {
            ContractsCommon.ResultIsNonNull<byte[]>();

            var ecdsa = SignerUtilities.GetSigner("NONEwithECDSA");
            ecdsa.Init(true, _privateKey);

            ecdsa.BlockUpdate(hash, 0, hash.Length);

            return ecdsa.GenerateSignature();
        }

        public bool VerifyHash(byte[] hash, byte[] signature)
        {
            var ecdsa = SignerUtilities.GetSigner("NONEwithECDSA");
            ecdsa.Init(false, _publicKey);

            ecdsa.BlockUpdate(hash, 0, hash.Length);

            return ecdsa.VerifySignature(signature);
        }
    }
}
