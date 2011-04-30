using System;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin
{
    public struct VarInt
    {
        private readonly ulong _value;

        public ulong Value
        {
            get { return _value; }
        }

        public VarInt(ulong value)
        {
            _value = value;
        }

        public VarInt(byte[] buffer, int offset)
        {
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length, "offset");
            Contract.EnsuresOnThrow<IndexOutOfRangeException>(Contract.ValueAtReturn(out this).Value == 0);
            
            ulong val = buffer[offset];
            switch (val)
            {
                // Below, (byte) | (byte) => (int). Due to sign-extention when changed to a ulong, negative integers won't
                // bitwise or in the expected way. This doesn't pose a problem here, since two bitwise-or-ed bytes won't ever
                // set the sign bit on an integer, but we cast to uint to make it explicit to the complier so that we
                // don't see warning CS0675 here in the 255 case.
                case 255:
                    val  = (uint)(buffer[offset + 5] | buffer[offset + 6] << 8 | buffer[offset + 7] << 16 | buffer[offset + 8] << 24);
                    val <<= 32;
                    val |= (uint)(buffer[offset + 1] | buffer[offset + 2] << 8 | buffer[offset + 3] << 16 | buffer[offset + 4] << 24);
                    break;
                case 254:
                    val  = (uint)(buffer[offset + 1] | buffer[offset + 2] << 8 | buffer[offset + 3] << 16 | buffer[offset + 4] << 24);
                    break;
                case 253:
                    val  = (uint)(buffer[offset + 1] | buffer[offset + 2] << 8);
                    break;
            }
            _value = val;
        }

        public int ByteSize
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() == 1 || Contract.Result<int>() == 3 || Contract.Result<int>() == 5 || Contract.Result<int>() == 9);

                if (_value < 253)
                    return 1;
                if (_value < 65536)
                    return 3;
                if (_value < 4294967296L)
                    return 5;
                return 9;
            }
        }

        public byte[] ToBytes()
        {
            Contract.Ensures(Contract.Result<byte[]>() != null);
            Contract.Ensures(Contract.Result<byte[]>().Length == ByteSize);

            switch(ByteSize)
            {
                case 1:
                    return new [] { (byte) _value };
                case 3:
                    return new [] { (byte)253, (byte)_value, (byte)(_value >> 8) };
                case 5:
                    return new [] { (byte)254, (byte)_value, (byte)(_value >> 8), (byte)(_value >> 16), (byte)(_value >> 24) };
                case 9:
                    return new [] { (byte)255, (byte) _value,        (byte)(_value >> 8 ), (byte)(_value >> 16), (byte)(_value >> 24),
                                               (byte)(_value >> 32), (byte)(_value >> 40), (byte)(_value >> 48), (byte)(_value >> 56)};
            }
            return new byte[] {};
        }

        public static implicit operator VarInt(ulong value)
        {
            return new VarInt(value);
        }

        public static implicit operator ulong(VarInt value)
        {
            return value._value;
        }
    }
}
