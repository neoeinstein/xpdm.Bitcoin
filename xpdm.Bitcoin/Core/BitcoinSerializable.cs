using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using SCG = System.Collections.Generic;

namespace xpdm.Bitcoin.Core
{
    [ContractClass(typeof(Contracts.BitcoinSerializableContract))]
    public abstract class BitcoinSerializable : IBitcoinSerializable
    {
        protected BitcoinSerializable() { }

        #region Serialization

        #region Serialization Constructors

        protected BitcoinSerializable(Stream stream)
        {
            ContractsCommon.NotNull(stream, "stream");
            //Contract.Requires<ArgumentOutOfRangeException>(length <= stream.Length, "length");
            //Contract.Requires<ArgumentOutOfRangeException>(stream.Position + length <= stream.Length, "length");

            this.Deserialize(stream);
        }

        protected BitcoinSerializable(byte[] buffer, int offset)
        {
            ContractsCommon.NotNull(buffer, "buffer");
            ContractsCommon.ValidOffset(0, buffer.Length, offset, "offset");

            var stream = new MemoryStream(buffer, offset, buffer.Length - offset);

            this.Deserialize(stream);
        }

        #endregion

        #region Serialization Methods

        protected abstract void Deserialize(Stream stream);
        public abstract void Serialize(Stream stream);
        public abstract int SerializedByteSize { get; }

        public void SerializeToBuffer(byte[] buffer, int offset)
        {
            ContractsCommon.NotNull(buffer, "buffer");
            ContractsCommon.ValidOffsetLength(0, buffer.Length, offset, this.SerializedByteSize, "offset", "offset");

            var stream = new MemoryStream(buffer, offset, buffer.Length - offset);
            this.Serialize(stream);
        }

        public byte[] SerializeToByteArray()
        {
            var stream = new MemoryStream();
            this.Serialize(stream);
            return stream.ToArray();
        }

        #endregion

        #region Static Serialization Utility Methods

        #region Stream Reading Methods

        protected static T[] ReadVarArray<T>(Stream stream) where T : BitcoinSerializable, new()
        {
            ContractsCommon.NotNull(stream, "stream");
            ContractsCommon.ResultIsNonNull<T[]>();

            ulong count = ReadVarInt(stream);
            var retArr = new T[count];
            for (ulong i = 0; i < count; ++i)
            {
                retArr[i] = new T();
                retArr[i].Deserialize(stream);
            }
            return retArr;
        }

        protected static ulong ReadVarInt(Stream stream)
        {
            ContractsCommon.NotNull(stream, "stream");

            ulong val = ReadByte(stream);
            switch (val)
            {
                case 255:
                    val = ReadUInt64(stream);
                    break;
                case 254:
                    val = ReadUInt32(stream);
                    break;
                case 253:
                    val = ReadUInt16(stream);
                    break;
            }
            return val;
        }

        protected static T Read<T>(Stream stream) where T : BitcoinSerializable, new()
        {
            ContractsCommon.NotNull(stream, "stream");
            ContractsCommon.ResultIsNonNull<T>();

            var retVal = new T();
            retVal.Deserialize(stream);

            return retVal;
        }

        protected static ulong ReadUInt64(Stream stream)
        {
            ContractsCommon.NotNull(stream, "stream");
            return (ulong)ReadInt64(stream);
        }

        protected static long ReadInt64(Stream stream)
        {
            ContractsCommon.NotNull(stream, "stream");
            return ReadUInt32(stream) | ((long)ReadUInt32(stream) << 32);
        }

        protected static uint ReadUInt32(Stream stream)
        {
            ContractsCommon.NotNull(stream, "stream");
            return (uint)ReadInt32(stream);
        }

        protected static int ReadInt32(Stream stream)
        {
            ContractsCommon.NotNull(stream, "stream");
            int value = stream.ReadByte();
            value |= stream.ReadByte() << 8;
            value |= stream.ReadByte() << 16;
            value |= stream.ReadByte() << 24;
            return value;
        }

        protected static ushort ReadUInt16(Stream stream)
        {
            ContractsCommon.NotNull(stream, "stream");
            return (ushort)ReadInt16(stream);
        }

        protected static short ReadInt16(Stream stream)
        {
            ContractsCommon.NotNull(stream, "stream");
            int value = stream.ReadByte() | (stream.ReadByte() << 8);
            return (short)value;
        }

        protected static byte ReadByte(Stream stream)
        {
            ContractsCommon.NotNull(stream, "stream");
            return (byte)stream.ReadByte();
        }

        protected static sbyte ReadSByte(Stream stream)
        {
            ContractsCommon.NotNull(stream, "stream");
            return (sbyte)stream.ReadByte();
        }

        protected static byte[] ReadBytes(Stream stream, int length)
        {
            ContractsCommon.NotNull(stream, "stream");
            ContractsCommon.ResultIsNonNull<byte[]>();
            var arr = new byte[length];
            var read = stream.Read(arr, 0, length);
            if (read != length)
            {
                throw new SerializationException(string.Format("Unable to read bytes. Expected {0}, Read {1}", length, read));
            }
            return arr;
        }

        #endregion

        #region Stream Writing Methods

        protected static void WriteVarArray<T>(Stream stream, T[] objs) where T : BitcoinSerializable, new()
        {
            ContractsCommon.NotNull(objs, "objs");
            ContractsCommon.CanWriteToStream(stream, VarIntByteSize(objs.Length) + objs.Sum(o => o.SerializedByteSize));

            WriteVarInt(stream, objs.Length);
            foreach (var obj in objs)
            {
                obj.Serialize(stream);
            }
        }

        protected static void WriteCollection<T>(Stream stream, SCG.ICollection<T> objs) where T : BitcoinSerializable, new()
        {
            ContractsCommon.NotNull(objs, "objs");
            ContractsCommon.CanWriteToStream(stream, VarIntByteSize(objs.Count) + objs.Sum(o => o.SerializedByteSize));

            WriteVarInt(stream, objs.Count);
            foreach (var obj in objs)
            {
                obj.Serialize(stream);
            }
        }

        protected static void WriteVarInt(Stream stream, ulong value)
        {
            ContractsCommon.CanWriteToStream(stream, VarIntByteSize(value));
            if (value < 253)
            {
                Write(stream, (byte)value);
            }
            else if (value < 65536)
            {
                Write(stream, (byte)253);
                Write(stream, (ushort)value);
            }
            else if (value < 4294967296L)
            {
                Write(stream, (byte)254);
                Write(stream, (uint)value);
            }
            else
            {
                Write(stream, (byte)255);
                Write(stream, value);
            }
        }

        protected static void WriteVarInt(Stream stream, long value)
        {
            ContractsCommon.CanWriteToStream(stream, VarIntByteSize(value));
            WriteVarInt(stream, (ulong)value);
        }

        protected static void WriteVarInt(Stream stream, int value)
        {
            ContractsCommon.CanWriteToStream(stream, VarIntByteSize(value));
            // Prevent sign extension when upconverting to long
            WriteVarInt(stream, (ulong)value);
        }

        protected static void WriteVarInt(Stream stream, short value)
        {
            ContractsCommon.CanWriteToStream(stream, VarIntByteSize(value));
            // Prevent sign extension when upconverting to long
            WriteVarInt(stream, (ulong)value);
        }

        protected static void WriteVarInt(Stream stream, sbyte value)
        {
            ContractsCommon.CanWriteToStream(stream, VarIntByteSize(value));
            // Prevent sign extension when upconverting to long
            WriteVarInt(stream, (ulong)value);
        }

        protected static void Write(Stream stream, IBitcoinSerializable serializable)
        {
            ContractsCommon.CanWriteToStream(stream, serializable.SerializedByteSize);
            serializable.Serialize(stream);
        }

        protected static void Write(Stream stream, ulong value)
        {
            ContractsCommon.CanWriteToStream(stream, BufferOperations.UINT64_SIZE);
            var bytes = BitConverter.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
        }

        protected static void Write(Stream stream, long value)
        {
            ContractsCommon.CanWriteToStream(stream, BufferOperations.UINT64_SIZE);
            var bytes = BitConverter.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
        }

        protected static void Write(Stream stream, uint value)
        {
            ContractsCommon.CanWriteToStream(stream, BufferOperations.UINT32_SIZE);
            var bytes = BitConverter.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
        }

        protected static void Write(Stream stream, int value)
        {
            ContractsCommon.CanWriteToStream(stream, BufferOperations.UINT32_SIZE);
            var bytes = BitConverter.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
        }

        protected static void Write(Stream stream, ushort value)
        {
            ContractsCommon.CanWriteToStream(stream, BufferOperations.UINT16_SIZE);
            var bytes = BitConverter.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
        }

        protected static void Write(Stream stream, short value)
        {
            ContractsCommon.CanWriteToStream(stream, BufferOperations.UINT16_SIZE);
            var bytes = BitConverter.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
        }

        protected static void Write(Stream stream, byte value)
        {
            ContractsCommon.CanWriteToStream(stream, BufferOperations.UINT8_SIZE);
            stream.WriteByte(value);
        }

        protected static void Write(Stream stream, sbyte value)
        {
            ContractsCommon.CanWriteToStream(stream, BufferOperations.UINT8_SIZE);
            stream.WriteByte((byte)value);
        }

        protected static void WriteBytes(Stream stream, byte[] bytes)
        {
            ContractsCommon.NotNull(bytes, "bytes");
            ContractsCommon.CanWriteToStream(stream, BufferOperations.UINT8_SIZE * bytes.Length);
            stream.Write(bytes, 0, bytes.Length);
        }

        protected static void WriteBytes(Stream stream, byte[] buffer, int offset, int length)
        {
            ContractsCommon.CanWriteToStream(stream, BufferOperations.UINT8_SIZE * length);
            ContractsCommon.NotNull(buffer, "buffer");
            ContractsCommon.ValidOffsetLength(0, buffer.Length, offset, length);
            stream.Write(buffer, offset, length);
        }

        #endregion

        #region VarInt Size Utilities

        protected static int VarIntByteSize(ulong value)
        {
            if (value < 253)
            {
                return 1;
            }
            if (value < 65536)
            {
                return 3;
            }
            if (value < 4294967296L)
            {
                return 5;
            }
            return 9;
        }

        protected static int VarIntByteSize(long value)
        {
            return VarIntByteSize((ulong)value);
        }

        protected static int VarIntByteSize(int value)
        {
            // Prevent sign extension when upconverting to long
            return VarIntByteSize((ulong)value);
        }

        protected static int VarIntByteSize(short value)
        {
            // Prevent sign extension when upconverting to long
            return VarIntByteSize((ulong)value);
        }

        protected static int VarIntByteSize(sbyte value)
        {
            // Prevent sign extension when upconverting to long
            return VarIntByteSize((ulong)value);
        }

        #endregion

        public static BitcoinSerializable DeserializeFromStream<T>(Stream stream) where T : BitcoinSerializable, new()
        {
            ContractsCommon.NotNull(stream, "stream");

            var ooze = new T();
            ooze.Deserialize(stream);
            return ooze;
        }

        public static void Serialize(Stream stream, BitcoinSerializable obj)
        {
            ContractsCommon.NotNull(stream, "stream");
            ContractsCommon.NotNull(obj, "obj");

            obj.Serialize(stream);
        }

        #endregion

        #endregion
    }

    namespace Contracts
    {
        [ContractClassFor(typeof(BitcoinSerializable))]
        internal abstract class BitcoinSerializableContract : BitcoinSerializable
        {
            protected sealed override void Deserialize(Stream stream)
            {
                ContractsCommon.NotFrozen(this);
                ContractsCommon.NotNull(stream, "stream");
                //Contract.Requires<ArgumentOutOfRangeException>(length <= stream.Length, "length");
                //Contract.Requires<ArgumentOutOfRangeException>(stream.Position + length <= stream.Length, "length");
            }

            public sealed override void Serialize(Stream stream)
            {
            }

            public sealed override int SerializedByteSize
            {
                get
                {
                    Contract.Ensures(Contract.Result<int>() >= 0);
                    return default(int);
                }
            }
        }
    }
}
