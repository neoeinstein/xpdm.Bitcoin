using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin.Cryptography
{
    [ContractClass(typeof(Contracts.ISigningKeyContract))]
    public interface ISigningKey
    {
        bool HasPrivateKey { get; }
        byte[] RetreivePrivateKey();
        byte[] PublicKey { get; }
        byte[] SignHash(byte[] hash);
        bool VerifyHash(byte[] hash, byte[] signature);
    }

    namespace Contracts
    {
        [ContractClassFor(typeof(ISigningKey))]
        internal abstract class ISigningKeyContract : ISigningKey
        {
            public bool HasPrivateKey
            {
                get { return default(bool); }
            }

            public byte[] RetreivePrivateKey()
            {
                Contract.Requires(HasPrivateKey);
                ContractsCommon.ResultIsNonNull<byte[]>();

                return default(byte[]);
            }

            public byte[] PublicKey
            {
                get
                {
                    ContractsCommon.ResultIsNonNull<byte[]>();

                    return default(byte[]);
                }
            }

            public byte[] SignHash(byte[] hash)
            {
                Contract.Requires(HasPrivateKey);
                ContractsCommon.NotNull(hash, "hash");
                ContractsCommon.ResultIsNonNull<byte[]>();

                return default(byte[]);
            }

            public bool VerifyHash(byte[] hash, byte[] signature)
            {
                ContractsCommon.NotNull(hash, "hash");
                ContractsCommon.NotNull(signature, "signature");

                return default(bool);
            }
        }
    }
}
