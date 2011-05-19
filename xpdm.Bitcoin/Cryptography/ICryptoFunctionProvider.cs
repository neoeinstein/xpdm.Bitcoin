using System;
using System.Diagnostics.Contracts;
using System.Numerics;
using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Cryptography
{
    [ContractClass(typeof(Contracts.ICryptoFunctionProviderContract))]
    public interface ICryptoFunctionProvider
    {
        Hash256 Hash256(byte[] buffer);
        Hash256 Hash256(byte[] buffer, int offset, int length);
        Hash256 Hash256(byte[] bufferA, byte[] bufferB);
        Hash256 Hash256(byte[] bufferA, int offsetA, int lengthA, byte[] bufferB, int offsetB, int lengthB);
        Hash160 Hash160(byte[] buffer);
        Hash160 Hash160(byte[] buffer, int offset, int length);
        Hash160 Sha1(byte[] buffer);
        Hash160 Sha1(byte[] buffer, int offset, int length);
        Hash160 Ripemd160(byte[] buffer);
        Hash160 Ripemd160(byte[] buffer, int offset, int length);
        Hash256 Sha256(byte[] buffer);
        Hash256 Sha256(byte[] buffer, int offset, int length);
    }

    namespace Contracts
    {
        [ContractClassFor(typeof(ICryptoFunctionProvider))]
        internal abstract class ICryptoFunctionProviderContract : ICryptoFunctionProvider
        {
            public Hash256 Hash256(byte[] buffer)
            {
                ContractsCommon.NotNull(buffer, "buffer");
                ContractsCommon.ResultIsNonNull<Hash256>();

                return default(Hash256);
            }

            public Hash256 Hash256(byte[] buffer, int offset, int length)
            {
                ContractsCommon.NotNull(buffer, "buffer");
                ContractsCommon.ValidOffsetLength(0, buffer.Length, offset, length);
                ContractsCommon.ResultIsNonNull<Hash256>();

                return default(Hash256);
            }

            public Hash256 Hash256(byte[] bufferA, byte[] bufferB)
            {
                ContractsCommon.NotNull(bufferA, "bufferA");
                ContractsCommon.NotNull(bufferB, "bufferB");
                ContractsCommon.ResultIsNonNull<Hash256>();

                return default(Hash256);
            }

            public Hash256 Hash256(byte[] bufferA, int offsetA, int lengthA, byte[] bufferB, int offsetB, int lengthB)
            {
                ContractsCommon.NotNull(bufferA, "bufferA");
                ContractsCommon.NotNull(bufferB, "bufferB");
                ContractsCommon.ValidOffsetLength(0, bufferA.Length, offsetA, lengthA);
                ContractsCommon.ValidOffsetLength(0, bufferB.Length, offsetB, lengthB);
                ContractsCommon.ResultIsNonNull<Hash256>();

                return default(Hash256);
            }

            public Hash160 Hash160(byte[] buffer)
            {
                ContractsCommon.NotNull(buffer, "buffer");
                ContractsCommon.ResultIsNonNull<Hash160>();

                return default(Hash160);
            }

            public Hash160 Hash160(byte[] buffer, int offset, int length)
            {
                ContractsCommon.NotNull(buffer, "buffer");
                ContractsCommon.ValidOffsetLength(0, buffer.Length, offset, length);
                ContractsCommon.ResultIsNonNull<Hash160>();

                return default(Hash160);
            }

            public Hash160 Sha1(byte[] buffer)
            {
                ContractsCommon.NotNull(buffer, "buffer");
                ContractsCommon.ResultIsNonNull<Hash160>();

                return default(Hash160);
            }

            public Hash160 Sha1(byte[] buffer, int offset, int length)
            {
                ContractsCommon.NotNull(buffer, "buffer");
                ContractsCommon.ValidOffsetLength(0, buffer.Length, offset, length);
                ContractsCommon.ResultIsNonNull<Hash160>();

                return default(Hash160);
            }

            public Hash160 Ripemd160(byte[] buffer)
            {
                ContractsCommon.NotNull(buffer, "buffer");
                ContractsCommon.ResultIsNonNull<Hash160>();

                return default(Hash160);
            }

            public Hash160 Ripemd160(byte[] buffer, int offset, int length)
            {
                ContractsCommon.NotNull(buffer, "buffer");
                ContractsCommon.ValidOffsetLength(0, buffer.Length, offset, length);
                ContractsCommon.ResultIsNonNull<Hash160>();

                return default(Hash160);
            }

            public Hash256 Sha256(byte[] buffer)
            {
                ContractsCommon.NotNull(buffer, "buffer");
                ContractsCommon.ResultIsNonNull<Hash256>();

                return default(Hash256);
            }

            public Hash256 Sha256(byte[] buffer, int offset, int length)
            {
                ContractsCommon.NotNull(buffer, "buffer");
                ContractsCommon.ValidOffsetLength(0, buffer.Length, offset, length);
                ContractsCommon.ResultIsNonNull<Hash256>();

                return default(Hash256);
            }
        }
    }
}
