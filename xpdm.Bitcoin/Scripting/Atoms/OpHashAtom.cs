﻿using System;
using System.Diagnostics.Contracts;
using xpdm.Bitcoin.Cryptography;

namespace xpdm.Bitcoin.Scripting.Atoms
{
    public sealed class OpHashAtom : OpAtom
    {
        public override int OperandCount(IExecutionContext context)
        {
            return 1;
        }

        public override int ResultCount(IExecutionContext context)
        {
            return 1;
        }

        protected override void ExecuteImpl(IExecutionContext context)
        {
            byte[] result;
            switch (OpCode)
            {
                case ScriptOpCode.OP_RIPEMD160:
                    result = CryptoFunctionProviderFactory.Default.Ripemd160(context.ValueStack.Peek()).Bytes;
                    break;
                case ScriptOpCode.OP_SHA1:
                    result = CryptoFunctionProviderFactory.Default.Sha1(context.ValueStack.Peek()).Bytes;
                    break;
                case ScriptOpCode.OP_SHA256:
                    result = CryptoFunctionProviderFactory.Default.Sha256(context.ValueStack.Peek()).Bytes;
                    break;
                case ScriptOpCode.OP_HASH160:
                    result = CryptoFunctionProviderFactory.Default.Hash160(context.ValueStack.Peek()).Bytes;
                    break;
                case ScriptOpCode.OP_HASH256:
                    result = CryptoFunctionProviderFactory.Default.Hash256(context.ValueStack.Peek()).Bytes;
                    break;
                default:
                    context.HardFailure = true;
                    return;
            }
            context.ValueStack.Pop();
            context.ValueStack.Push(result);
        }

        public OpHashAtom(ScriptOpCode opcode)
            : base(opcode)
        {
            Contract.Requires<ArgumentException>(opcode == ScriptOpCode.OP_RIPEMD160 ||
                                                 opcode == ScriptOpCode.OP_SHA1 ||
                                                 opcode == ScriptOpCode.OP_SHA256 ||
                                                 opcode == ScriptOpCode.OP_HASH160 ||
                                                 opcode == ScriptOpCode.OP_HASH256,
                                                 "opcode");
        }
    }
}
