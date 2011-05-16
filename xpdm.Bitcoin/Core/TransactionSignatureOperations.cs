using System;
using System.IO;
using C5;
using SCG = System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace xpdm.Bitcoin.Core
{
    public class TransactionSignatureOperations
    {
        public static byte[] SignTransaction(byte[] privateKey,
            ScriptBase script, Transaction transaction, int transactionInputIndex, SignatureHashType signatureType)
        {
            //var cs = System.Security.Cryptography.ECDsa.Create();
            var hash = new ECDsaCng(CngKey.Import(privateKey, CngKeyBlobFormat.EccPrivateBlob));
            var sigHash = hash.SignHash(HashTransactionForSigning(script, transaction, transactionInputIndex, signatureType).Bytes);
            return sigHash;
        }

        public static bool VerifySignature(byte[] publicKey, byte[] sigHash,
            ScriptBase script, Transaction transaction, int transactionInputIndex, SignatureHashType signatureType)
        {
            //var cs = System.Security.Cryptography.ECDsa.Create();
            var hash = new ECDsaCng(CngKey.Import(publicKey, CngKeyBlobFormat.EccPublicBlob));
            var valid = hash.VerifyHash(HashTransactionForSigning(script, transaction, transactionInputIndex, signatureType).Bytes, sigHash);
            return valid;
        }

        public static Hash256 HashTransactionForSigning(ScriptBase script, Transaction transaction, int transactionInputIndex, SignatureHashType signatureType)
        {
            var t = TransformTransactionForSigning(script, transaction, transactionInputIndex, signatureType);
            var bytes = new byte[t.SerializedByteSize + 1];
            bytes[t.SerializedByteSize] = (byte)signatureType;
            var ms = new MemoryStream(bytes);
            t.Serialize(ms);
            return HashUtil.Hash256(bytes);
        }

        public static Transaction TransformTransactionForSigning(ScriptBase script, Transaction transaction, int transactionInputIndex, SignatureHashType signatureType)
        {
            var tb = new TransactionBuilder(transaction);

            script.Atoms.RemoveAllCopies(Scripting.Atoms.OpCodeSeparatorAtom.Atom);

            ClearAllScripts(tb.TransactionInputs);

            tb.TransactionInputs[transactionInputIndex].Script.AddAll(script.Atoms);

            // By default, signing a transaction with SignatureHashType = 0 means the signer
            // agrees with the transaction only if the inputs and outputs are exactly the same
            // as they were when originally signed, except input scripts, which are cleared and
            // the input script for this index set as passed above.

            switch(signatureType & SignatureHashType.OutputMask)
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

            return tb.FreezeToTransaction();
        }

        public static void ClearAllScripts(SCG.IEnumerable<TransactionInputBuilder> tis)
        {
            foreach (var ti in tis)
            {
                ti.Script.Clear();
            }
        }

        public static void OnlyUseSingleOutput(IList<TransactionOutputBuilder> tos, int indexToUse)
        {
            if (indexToUse >= tos.Count)
            {
                throw new Exception();
            }
            var to = tos[indexToUse];
            tos.Clear();
            tos.AddAll(Enumerable.Repeat(TransactionOutputBuilder.Empty, indexToUse));
            tos.Add(to);
        }

        public static void OnlyUseSingleInput(IList<TransactionInputBuilder> tis, int indexToUse)
        {
            var ti = tis[indexToUse];
            tis.Clear();
            tis.Add(ti);
        }

        public static void SetSequenceNumbersToZero(IList<TransactionInputBuilder> tis, int indexToLeaveAlone)
        {
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
