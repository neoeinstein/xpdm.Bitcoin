using System;
using System.Diagnostics.Contracts;
using xpdm.Bitcoin;

namespace xpdm.Bitcoin.Protocol
{
    /// <summary>
    /// A variable length integer capable of handling values up to 64-bits.
    /// </summary>
    public struct VarInt : IBitcoinSerializable
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
            Contract.Requires<ArgumentNullException>(buffer != null, "buffer");
            Contract.Requires<ArgumentException>(buffer.Length >= VarInt.MinimumByteSize, "buffer");
            Contract.Requires<ArgumentOutOfRangeException>(offset >= 0, "offset");
            Contract.Requires<ArgumentOutOfRangeException>(offset <= buffer.Length - VarInt.MinimumByteSize, "offset");
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

        public uint ByteSize
        {
            get
            {
                return VarInt.GetByteSize(_value);
            }
        }

        public static uint GetByteSize(ulong value)
        {
            if (value < 253)
                return 1;
            if (value < 65536)
                return 3;
            if (value < 4294967296L)
                return 5;
            return 9;
        }

        [Pure]
        public byte[] ToBytes()
        {
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
            return new byte[] { 0 };
        }

        [Pure]
        public void WriteToBitcoinBuffer(byte[] buffer, int offset)
        {
            switch(ByteSize)
            {
                case 1:
                    buffer[offset] = (byte)_value;
                    break;
                case 3:
                    buffer[offset] = (byte)253;
                    buffer[offset + 1] = (byte)_value;
                    buffer[offset + 2] = (byte)(_value >> 8);
                    break;
                case 5:
                    buffer[offset] = (byte)254;
                    buffer[offset + 1] = (byte)_value;
                    buffer[offset + 2] = (byte)(_value >> 8);
                    buffer[offset + 3] = (byte)(_value >> 16);
                    buffer[offset + 4] = (byte)(_value >> 24);
                    break;
                case 9:
                    buffer[offset] = (byte)254;
                    buffer[offset + 1] = (byte)_value;
                    buffer[offset + 2] = (byte)(_value >> 8);
                    buffer[offset + 3] = (byte)(_value >> 16);
                    buffer[offset + 4] = (byte)(_value >> 24);
                    buffer[offset + 5] = (byte)(_value >> 32);
                    buffer[offset + 6] = (byte)(_value >> 40);
                    buffer[offset + 7] = (byte)(_value >> 48);
                    buffer[offset + 8] = (byte)(_value >> 56);
                    break;
                default:
                    buffer[offset] = (byte)0;
                    break;
            }
        }

        public static int MinimumByteSize
        {
            get { return BufferOperations.UINT8_SIZE; }
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
