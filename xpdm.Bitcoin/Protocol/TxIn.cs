using System;
using System.Diagnostics.Contracts;
using xpdm.Bitcoin;

namespace xpdm.Bitcoin.Protocol
{
    public class TxIn : BitcoinSerializableBase
    {
        public TxOutpoint Outpoint { get; private set; }
        public VarInt ScriptLength { get; private set; }
        private byte[] _script;
        public byte[] Script
        {
            get
            {
                Contract.Ensures(Contract.Result<byte[]>() != null);

                var retVal = (byte[])_script.Clone();
                return retVal;
            }
        }
        public uint Sequence { get; private set; }

        public TxIn(TxOutpoint outpoint, byte[] script, uint sequence)
        {
            Contract.Requires<ArgumentNullException>(outpoint != null, "outpoint");
            Contract.Requires<ArgumentNullException>(script != null, "script");

            Outpoint = outpoint;
            ScriptLength = (ulong)script.LongLength;
            _script = (byte[])script.Clone();
            Sequence = sequence;

            ByteSize = Outpoint.ByteSize + ScriptLength.ByteSize + (uint)ScriptLength.Value * BufferOperations.UINT8_SIZE + Sequence.ByteSize();
        }

        public TxIn(byte[] buffer, int offset)
            :base(buffer, offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Length >= TxIn.MinimumByteSize, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - TxIn.MinimumByteSize, "offset");

            Outpoint = new TxOutpoint(buffer, offset);
            ScriptLength = new VarInt(buffer, offset + SCRIPTLEN_OFFSET);
            var script = new byte[ScriptLength];
            Array.Copy(buffer, offset + SCRIPTLEN_OFFSET + (int) ScriptLength.ByteSize, script, 0, (int)ScriptLength.Value);
            _script = script;
            Sequence = buffer.ReadUInt32(offset + SCRIPTLEN_OFFSET + (int)ScriptLength.ByteSize + (int)ScriptLength.Value * BufferOperations.UINT8_SIZE);

            ByteSize = Outpoint.ByteSize + ScriptLength.ByteSize + (uint)ScriptLength.Value * BufferOperations.UINT8_SIZE + Sequence.ByteSize();
        }

        private static readonly int SCRIPTLEN_OFFSET = TxOutpoint.ConstantByteSize;

        [Pure]
        public override void WriteToBitcoinBuffer(byte[] buffer, int offset)
        {
            Outpoint.WriteToBitcoinBuffer(buffer, offset);
            ScriptLength.WriteToBitcoinBuffer(buffer, offset + SCRIPTLEN_OFFSET);
            Array.Copy(_script, 0, buffer, offset + SCRIPTLEN_OFFSET + (int)ScriptLength.ByteSize, (int)ScriptLength.Value);
            Sequence.WriteBytes(buffer, offset + SCRIPTLEN_OFFSET + (int)ScriptLength.ByteSize + (int)ScriptLength.Value * BufferOperations.UINT8_SIZE);
        }

        public static int MinimumByteSize
        {
            get
            {
                return TxOutpoint.ConstantByteSize + VarInt.MinimumByteSize + BufferOperations.UINT32_SIZE;
            }
        }

        [ContractInvariantMethod]
        private void __Invariant()
        {
            Contract.Invariant(Outpoint != null);
        }
    }
}
