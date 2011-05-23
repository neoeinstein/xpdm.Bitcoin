using System;
using System.Diagnostics.Contracts;
using System.Text;

namespace xpdm.Bitcoin.Protocol
{
    public struct VarString : IBitcoinSerializable
    {
        private readonly VarInt _length;
        private readonly string _value;

        public VarInt Length
        {
            get { return _length; }
        }

        public string Value
        {
            get { return _value; }
        }

        public VarString(string value)
        {
            _value = value;
            _length = new VarInt((ulong)value.Length);
        }

        public VarString(byte[] buffer, int offset)
        {
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Length >= VarString.MinimumByteSize, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - VarString.MinimumByteSize, "offset");
            Contract.EnsuresOnThrow<IndexOutOfRangeException>(Contract.ValueAtReturn(out this).Length == 0);
            Contract.EnsuresOnThrow<IndexOutOfRangeException>(Contract.ValueAtReturn(out this).Value.Length == 0);

            var l = new VarInt(buffer, offset);
            var v = Encoding.ASCII.GetString(buffer, (int)((uint)offset + l.ByteSize), (int)l.Value);

            _length = l;
            _value = v;
        }

        public uint ByteSize
        {
            get
            {
                return (uint)_value.Length + _length.ByteSize;
            }
        }

        public static uint GetByteSize(string value)
        {
            return (uint)value.Length + VarInt.GetByteSize((ulong)value.Length);
        }

        [Pure]
        public byte[] ToBytes()
        {
            var bytes = new byte[ByteSize];

            this.WriteToBitcoinBuffer(bytes, 0);

            return bytes;
        }

        [Pure]
        public void WriteToBitcoinBuffer(byte[] buffer, int offset)
        {
            Length.WriteToBitcoinBuffer(buffer, offset);
            Encoding.ASCII.GetBytes(_value, 0, (int)_value.Length, buffer, (int)((uint)offset + Length.ByteSize));
        }

        public static int MinimumByteSize
        {
            get { return VarInt.MinimumByteSize; }
        }

        public static implicit operator VarString(string value)
        {
            return new VarString(value);
        }

        public static implicit operator string(VarString value)
        {
            return value._value;
        }

        [ContractInvariantMethod]
        private void __Invariant()
        {
            Contract.Invariant((ulong)Value.Length == Length.Value);
        }
    }
}
