using System;
using System.Diagnostics.Contracts;
using System.IO;
using xpdm.Bitcoin.Core;

namespace xpdm.Bitcoin.Messaging.Payloads
{
    public class TxPayload : PayloadBase
    {
        public override string Command
        {
            get { return TxPayload.CommandText; }
        }

        public Transaction Transaction { get; private set; }

        public TxPayload(Transaction transaction)
        {
            Contract.Requires<ArgumentNullException>(transaction != null, "transaction");

            Transaction = transaction;
        }

        public TxPayload(Stream stream) : base(stream) { }
        public TxPayload(byte[] buffer, int offset) : base(buffer, offset) { }

        protected override void Deserialize(Stream stream)
        {
            Transaction = Read<Transaction>(stream);
        }

        public override void Serialize(Stream stream)
        {
            Write(stream, Transaction);
        }

        public override int SerializedByteSize
        {
            get { return Transaction.SerializedByteSize; }
        }

        public static string CommandText
        {
            get { return "tx"; }
        }
    }
}
