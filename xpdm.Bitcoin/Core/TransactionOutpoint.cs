using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin.Core
{
    public sealed class TransactionOutpoint : BitcoinObject
    {
        public Hash256 SourceTransactionHash { get; private set; }
        public uint OutputSequenceNumber { get; private set; }

        public TransactionOutpoint(Hash256 sourceTransactionHash, uint outputSequenceNumber)
        {
            ContractsCommon.NotNull(sourceTransactionHash, "sourceTransactionHash");

            SourceTransactionHash = sourceTransactionHash;
            OutputSequenceNumber = outputSequenceNumber;
        }

        public TransactionOutpoint() { }
        public TransactionOutpoint(Stream stream) : base(stream) { }
        public TransactionOutpoint(byte[] buffer, int offset) : base(buffer, offset) { }

        protected override void Deserialize(System.IO.Stream stream)
        {
            SourceTransactionHash = new Hash256(stream);
            OutputSequenceNumber = ReadUInt32(stream);
        }

        public override void Serialize(System.IO.Stream stream)
        {
            SourceTransactionHash.Serialize(stream);
            Write(stream, OutputSequenceNumber);
        }

        public override int SerializedByteSize
        {
            get { return SourceTransactionHash.HashByteSize + BufferOperations.UINT32_SIZE; }
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", SourceTransactionHash, OutputSequenceNumber);
        }
    }
}
