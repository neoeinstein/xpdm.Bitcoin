using System;
using System.Diagnostics.Contracts;
using xpdm.Bitcoin;

namespace xpdm.Bitcoin.Protocol
{
    public class TxOut : BitcoinSerializableBase
    {
        public ulong Value { get; private set; }
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

        public TxOut(ulong value, byte[] script)
        {
            Contract.Requires<ArgumentNullException>(script != null, "script");

            Value = value;
            ScriptLength = (ulong)script.LongLength;
            _script = (byte[])script.Clone();

            ByteSize = Value.ByteSize() + ScriptLength.ByteSize + (uint)ScriptLength.Value * BufferOperations.UINT8_SIZE;
        }

        public TxOut(byte[] buffer, int offset)
            :base(buffer, offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Length >= TxOut.MinimumByteSize, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - TxOut.MinimumByteSize, "offset");

            Value = buffer.ReadUInt64(offset);
            ScriptLength = new VarInt(buffer, offset + SCRIPTLEN_OFFSET);
            var script = new byte[ScriptLength];
            Array.Copy(buffer, offset + SCRIPTLEN_OFFSET + (int) ScriptLength.ByteSize, script, 0, (int)ScriptLength.Value);
            _script = script;

            ByteSize = Value.ByteSize() + ScriptLength.ByteSize + (uint)ScriptLength.Value * BufferOperations.UINT8_SIZE;
        }

        private static readonly int SCRIPTLEN_OFFSET = BufferOperations.UINT64_SIZE;

        [Pure]
        public override void WriteToBitcoinBuffer(byte[] buffer, int offset)
        {
            Value.WriteBytes(buffer, offset);
            ScriptLength.WriteToBitcoinBuffer(buffer, offset + SCRIPTLEN_OFFSET);
            Array.Copy(_script, 0, buffer, offset + SCRIPTLEN_OFFSET + (int)ScriptLength.ByteSize, (int)ScriptLength.Value);
        }

        public static int MinimumByteSize
        {
            get
            {
                return BufferOperations.UINT64_SIZE + VarInt.MinimumByteSize;
            }
        }
    }
}
