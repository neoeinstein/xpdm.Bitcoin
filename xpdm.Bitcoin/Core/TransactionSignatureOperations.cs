using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using C5;
using xpdm.Bitcoin.Cryptography;
using SCG = System.Collections.Generic;

namespace xpdm.Bitcoin.Core
{
    public class TransactionSignatureOperations
    {
        public static byte[] SignTransaction(byte[] privateKey,
            Script script, Transaction transaction, int transactionInputIndex, SignatureHashType signatureType)
        {
            ContractsCommon.NotNull(privateKey, "privateKey");
            ContractsCommon.NotNull(script, "script");
            ContractsCommon.NotNull(transaction, "transaction");
            ContractsCommon.ValidIndex(0, transaction.TransactionInputs.Count, transactionInputIndex, "transactionInputIndex");
            ContractsCommon.ResultIsNonNull<byte[]>();

            var hash = HashTransactionForSigning(script, transaction, transactionInputIndex, signatureType);

            var ecdsa = new ECDsaBouncyCastle(privateKey, true);

            return ecdsa.SignHash(hash.Bytes);
        }

        public static bool VerifySignature(byte[] publicKey, byte[] sigHash,
            Script script, Transaction transaction, int transactionInputIndex, SignatureHashType signatureType)
        {
            ContractsCommon.NotNull(publicKey, "publicKey");
            ContractsCommon.NotNull(sigHash, "sigHash");
            ContractsCommon.NotNull(script, "script");
            ContractsCommon.NotNull(transaction, "transaction");
            ContractsCommon.ValidIndex(0, transaction.TransactionInputs.Count, transactionInputIndex, "transactionInputIndex");

            // Cng ECC Public Key Blob format [72 bytes]:
            //6fbfcf5da60c7e59dfe724f4fb1b4e73f12bc48f17fb90f1e74f0d058c65e77c76aa75787f18ddd8be32b08014046ff62fef598011583e6f2d77e2b2ab850e5f0000002031534345
            //coordY[cBytes]BIG-ENDIAN coordX[cBytes]BIG-ENDIAN cBytes[4]LITTLE-ENDIAN magic[4]LITTLE-ENDIAN
            // For ECDsaP256, that magic number is: 0x31534345U, and cBytes will be 0x00000020

            // Bitcoin ECC Public Key Blob format [65 bytes]:
            //04e77ae594d5932f11547eea049b526044cbf8ac938fabfa97d3ab37732822572bd7d71c77a8ac1f5ca46fd73260f5aecc7270efbbf283bacd64ef0a9bb41e3ab9
            //type[1] coordX[cBytes] coordY[cBytes] (all LITTLE-ENDIAN)
            // type = 04 means curve points are uncompressed. No other value is expected.
            // For secp256k1, cBytes = 0x00000020

            // Probable issue here: Cng supports the NIST P-256 curve (alias of secp256r1), but Bitcoin uses the secp256k1 curve.

            if (signatureType == 0)
            {
                signatureType = (SignatureHashType)sigHash[sigHash.Length - 1];
            }
            else if (signatureType != (SignatureHashType)sigHash[sigHash.Length - 1])
            {
                return false;
            }
            var signature = new byte[72];
            Array.Copy(sigHash, signature, signature.Length);

            var hash = HashTransactionForSigning(script, transaction, transactionInputIndex, signatureType);

            var ecdsa = new ECDsaBouncyCastle(publicKey, false);

            return ecdsa.VerifyHash(hash.Bytes, signature);
        }

        public static Hash256 HashTransactionForSigning(Script script, Transaction transaction, int transactionInputIndex, SignatureHashType signatureType)
        {
            ContractsCommon.NotNull(script, "script");
            ContractsCommon.NotNull(transaction, "transaction");
            ContractsCommon.ValidIndex(0, transaction.TransactionInputs.Count, transactionInputIndex, "transactionInputIndex");
            ContractsCommon.ResultIsNonNull<Hash256>();

            var t = TransformTransactionForSigning(script, transaction, transactionInputIndex, signatureType);
            using (var ms = new MemoryStream(t.SerializedByteSize + BufferOperations.UINT32_SIZE))
            {
                t.Serialize(ms);
                var sigType = (int)signatureType;
                ms.WriteByte((byte)sigType);
                ms.WriteByte((byte)(sigType >> 8));
                ms.WriteByte((byte)(sigType >> 16));
                ms.WriteByte((byte)(sigType >> 24));
                return CryptoFunctionProviderFactory.Default.Hash256(ms.ToArray());
            }
        }

        public static Transaction TransformTransactionForSigning(Script script, Transaction transaction, int transactionInputIndex, SignatureHashType signatureType)
        {
            ContractsCommon.NotNull(script, "script");
            ContractsCommon.NotNull(transaction, "transaction");
            ContractsCommon.ValidIndex(0, transaction.TransactionInputs.Count, transactionInputIndex, "transactionInputIndex");
            ContractsCommon.ResultIsNonNull<Transaction>();

            var tb = transaction.ThawTree();

            //script.Atoms.RemoveAllCopies(Scripting.Atoms.OpCodeSeparatorAtom.Atom);

            ClearAllScripts(tb.TransactionInputs);

            tb.TransactionInputs[transactionInputIndex].Script.Atoms.AddAll(script.Atoms);

            // By default, signing a transaction with SignatureHashType = 0 means the signer
            // agrees with the transaction only if the inputs and outputs are exactly the same
            // as they were when originally signed, except input scripts, which are cleared and
            // the input script for this index set as passed above.

            switch (signatureType & SignatureHashType.OutputMask)
            {
                // Signer accepts transaction regardless of the outputs specified in the transaction
                // or the sequence numbers of the other inputs.
                case SignatureHashType.None:
                    tb.TransactionOutputs.Clear();
                    SetSequenceNumbersToZero(tb.TransactionInputs, transactionInputIndex);
                    break;
                // Signer accepts transaction if the matching outputs index of this transaction is
                // as originally signed, regarless of the sequence numbers of the other inputs.
                case SignatureHashType.Single:
                    OnlyUseSingleOutput(tb.TransactionOutputs, transactionInputIndex);
                    SetSequenceNumbersToZero(tb.TransactionInputs, transactionInputIndex);
                    break;
            }

            // Signer accepts all other inputs to this transaction, whatever they may be
            if ((signatureType & SignatureHashType.AnyoneCanPay) == SignatureHashType.AnyoneCanPay)
            {
                OnlyUseSingleInput(tb.TransactionInputs, transactionInputIndex);
            }

            tb.Freeze();

            return tb;
        }

        public static void ClearAllScripts(SCG.IEnumerable<TransactionInput> tis)
        {
            ContractsCommon.NotNull(tis, "tis");
            Contract.Requires(Contract.ForAll(tis, ti => ti != null));
            Contract.Ensures(Contract.ForAll(tis, ti => ti.Script.Atoms.IsEmpty));

            foreach (var ti in tis)
            {
                ti.Script.Atoms.Clear();
            }
        }

        public static void OnlyUseSingleOutput(IList<TransactionOutput> tos, int indexToUse)
        {
            ContractsCommon.NotNull(tos, "tos");
            ContractsCommon.ValidIndex(0, tos.Count, indexToUse, "indexToUse");

            var to = tos[indexToUse];
            tos.Clear();
            tos.AddAll(Enumerable.Repeat(new TransactionOutput { Value = -1 }, indexToUse));
            tos.Add(to);
        }

        public static void OnlyUseSingleInput(IList<TransactionInput> tis, int indexToUse)
        {
            ContractsCommon.NotNull(tis, "tis");
            ContractsCommon.ValidIndex(0, tis.Count, indexToUse, "indexToUse");

            var ti = tis[indexToUse];
            tis.Clear();
            tis.Add(ti);
        }

        public static void SetSequenceNumbersToZero(IList<TransactionInput> tis, int indexToLeaveAlone)
        {
            ContractsCommon.NotNull(tis, "tis");
            Contract.Ensures(Contract.ForAll(0, tis.Count, i => i == indexToLeaveAlone || tis[i].SequenceNumber == 0));

            for (int i = 0; i < tis.Count; ++i)
            {
                if (i != indexToLeaveAlone)
                {
                    tis[i].SequenceNumber = 0;
                }
            }
        }
    }
}
