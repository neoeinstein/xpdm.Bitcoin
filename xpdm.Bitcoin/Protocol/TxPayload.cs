using System;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin.Protocol
{
    public class TxPayload : MessagePayloadBase
    {
        public override string Command
        {
            get { return TxPayload.CommandText; }
        }

        public Tx Transaction { get; private set; }

        public TxPayload(Tx transaction)
        {
            Contract.Requires<ArgumentNullException>(transaction != null, "transaction");

            Transaction = transaction;

            ByteSize = Transaction.ByteSize;
        }

        public TxPayload(byte[] buffer, int offset)
            : base(buffer, offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Length >= TxPayload.MinimumByteSize, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - TxPayload.MinimumByteSize, "offset");

            Transaction = new Tx(buffer, offset);

            ByteSize = Transaction.ByteSize;
        }

        [Pure]
        public override void WriteToBitcoinBuffer(byte[] buffer, int offset)
        {
            Transaction.WriteToBitcoinBuffer(buffer, offset);
        }

        public static string CommandText
        {
            get { return "tx"; }
        }

        public static int MinimumByteSize
        {
            get
            {
                return Tx.MinimumByteSize;
            }
        }
    }
}
