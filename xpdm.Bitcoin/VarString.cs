using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin
{
    public struct VarString
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
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length, "offset");
            Contract.EnsuresOnThrow<IndexOutOfRangeException>(Contract.ValueAtReturn(out this).Length == 0);
            Contract.EnsuresOnThrow<IndexOutOfRangeException>(Contract.ValueAtReturn(out this).Value.Length == 0);

            var l = new VarInt(buffer, offset);
            var v = Encoding.ASCII.GetString(buffer, offset + l.ByteSize, (int)l.Value);

            _length = l;
            _value = v;
        }

        public int ByteSize
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() > 0);

                return _value.Length + _length.ByteSize;
            }
        }

        public static int GetByteSize(string value)
        {
            Contract.Ensures(Contract.Result<int>() > 0);

            return value.Length + VarInt.GetByteSize((ulong)value.Length);
        }

        [Pure]
        public byte[] ToBytes()
        {
            Contract.Ensures(Contract.Result<byte[]>() != null);
            Contract.Ensures(Contract.Result<byte[]>().Length == ByteSize);

            var bytes = new byte[ByteSize];

            Length.ToBytes().CopyTo(bytes, 0);
            Encoding.ASCII.GetBytes(_value, 0, _value.Length, bytes, Length.ByteSize);

            return bytes;
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
