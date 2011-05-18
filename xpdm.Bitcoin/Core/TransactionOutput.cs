using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;

namespace xpdm.Bitcoin.Core
{
    public sealed class TransactionOutput : BitcoinObject, IFreezable, IThawable<TransactionOutput>
    {
        private BitcoinValue _value;
        public BitcoinValue Value
        {
            get
            {
                return _value;
            }
            set
            {
                ContractsCommon.NotFrozen(this);

                _value = value;
                InvalidateBitcoinHashes();
            }
        }
        private Script _script = Script.Empty;
        public Script Script
        {
            get
            {
                ContractsCommon.ResultIsNonNull<Script>();

                return _script;
            }
            set
            {
                ContractsCommon.NotFrozen(this);
                ContractsCommon.NotNull(value, "value");

                _script.HashesInvalidated -= InvalidateBitcoinHashes;
                _script = value;
                _script.HashesInvalidated += InvalidateBitcoinHashes;
                InvalidateBitcoinHashes();
            }
        }

        public TransactionOutput()
        {
        }
        public TransactionOutput(TransactionOutput txOut) : this(txOut, false) { }
        public TransactionOutput(TransactionOutput txOut, bool thawChildren)
        {
            ContractsCommon.NotNull(txOut, "txOut");
            ContractsCommon.ChildThawed(Script, thawChildren);

            _value = txOut._value;
            _script = FreezableExtensions.ThawChild(txOut._script, thawChildren);
        }

        public TransactionOutput(Stream stream) : base(stream) { }
        public TransactionOutput(byte[] buffer, int offset) : base(buffer, offset) { }

        protected override void Deserialize(System.IO.Stream stream)
        {
            Value = ReadUInt64(stream);
            Script = new Script(stream);

            Freeze();
        }

        public override void Serialize(System.IO.Stream stream)
        {
            Write(stream, (ulong)Value);
            Script.Serialize(stream);
        }

        public override int SerializedByteSize
        {
            get { return BufferOperations.UINT64_SIZE + Script.SerializedByteSize; }
        }

        public override string ToString()
        {
            return string.Format("{0} [ {1} ]", Value, Script);
        }

        public bool IsFrozen { get; private set; }

        public void Freeze()
        {
            Script.Freeze();
            IsFrozen = true;
        }

        public TransactionOutput Thaw()
        {
            return new TransactionOutput(this, false);
        }

        public TransactionOutput ThawTree()
        {
            ContractsCommon.IsThawed(Contract.Result<TransactionOutput>().Script);

            return new TransactionOutput(this, true);
        }

        static TransactionOutput()
        {
            var empty = new TransactionOutput();
            empty.Freeze();
            _empty = empty;
        }

        private static readonly TransactionOutput _empty;
        public static TransactionOutput Empty { get { return _empty; } }

        [ContractInvariantMethod]
        private void __Invariant()
        {
            Contract.Invariant(_script != null);
        }
    }
}
