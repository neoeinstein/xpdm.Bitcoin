using System;
using System.Linq;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin
{
    public class Tx : BitcoinSerializableBase
    {
        public uint Version { get; private set; }
        public VarArray<TxIn> TxIns { get; private set; }
        public VarArray<TxOut> TxOuts { get; private set; }
        public uint LockTime { get; private set; }

        public Tx(uint version, TxIn[] txIn, TxOut[] txOut, uint lockTime)
        {
            Contract.Requires<ArgumentNullException>(txIn != null, "txIn");
            Contract.Requires<ArgumentNullException>(txOut != null, "txOut");

            Version = version;
            TxIns = new VarArray<TxIn>(txIn);
            TxOuts = new VarArray<TxOut>(txOut);
            LockTime = lockTime;

            ByteSize = Version.ByteSize() + TxIns.ByteSize + TxOuts.ByteSize + LockTime.ByteSize();
        }

        public Tx(byte[] buffer, int offset)
            : base(buffer, offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Length >= Tx.MinimumByteSize, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - Tx.MinimumByteSize, "offset");

            Version = buffer.ReadUInt32(offset);
            TxIns = new VarArray<TxIn>(buffer, TxIns_Offset(ref offset));
            TxOuts = new VarArray<TxOut>(buffer, TxOuts_Offset(ref offset));
            LockTime = buffer.ReadUInt32(LockTime_Offset(ref offset));

            ByteSize = Version.ByteSize() + TxIns.ByteSize + TxOuts.ByteSize + LockTime.ByteSize();
        }

        private int TxIns_Offset(ref int currentOffset) { return currentOffset += (int)Version.ByteSize(); }
        private int TxOuts_Offset(ref int currentOffset) { return currentOffset += (int)TxIns.ByteSize; }
        private int LockTime_Offset(ref int currentOffset) { return currentOffset += (int)TxOuts.ByteSize; }

        [Pure]
        public override void WriteToBitcoinBuffer(byte[] buffer, int offset)
        {
            Version.WriteBytes(buffer, offset);
            TxIns.WriteToBitcoinBuffer(buffer, TxIns_Offset(ref offset));
            TxOuts.WriteToBitcoinBuffer(buffer, TxOuts_Offset(ref offset));
            LockTime.WriteBytes(buffer, LockTime_Offset(ref offset));
        }

        public static int MinimumByteSize
        {
            get
            {
                return BitcoinBufferOperations.UINT32_SIZE * 2 + VarArray<TxIn>.MinimumByteSize + VarArray<TxOut>.MinimumByteSize;
            }
        }
    }
}
