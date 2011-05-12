﻿using System;
using System.Diagnostics.Contracts;
using C5;
using System.Numerics;

namespace xpdm.Bitcoin.Scripting
{
    public class ExecutionContext
    {
        public static readonly int MaximumCombinedStackSize = 1000;

        public IStack<byte[]> ValueStack { get; private set; }
        public IStack<byte[]> AltStack { get; private set; }
        public IStack<bool> ControlStack { get; private set; }

        private bool _hardFailure = false;
        public bool HardFailure
        {
            get { return _hardFailure; }
            set
            {
                Contract.Ensures(Contract.OldValue<bool>(HardFailure) == true || HardFailure == value);

                _hardFailure |= value; 
            }
        }

        public ExecutionContext()
        {
            ValueStack = new CircularQueue<byte[]>();
            AltStack = new CircularQueue<byte[]>();
            ControlStack = new CircularQueue<bool>();
        }

        public bool? ExecutionResult
        {
            get
            {
                if (HardFailure || !IsValid)
                {
                    return false;
                }
                if (ValueStack.Count == 1 && AltStack.Count == 0 && ControlStack.Count == 0)
                {
                    return ExecutionContext.ToBool(ValueStack[0]);
                }
                return null;
            }
        }

        public bool ExecuteBlock
        {
            get
            {
                return !ControlStack.All(b => b);
            }
        }

        public bool IsValid
        {
            get
            {
                return ValueStack.Count + AltStack.Count <= MaximumCombinedStackSize;
            }
        }

        public static bool ToBool(byte[] val)
        {
            for (int i = 0; i < val.Length; ++i)
            {
                if (val[i] != 0)
                {
                    // Can be negative zero
                    if (i == val.Length - 1 && val[i] == 0x80)
                        return false;
                    return true;
                }
            }
            return false;
        }

        public static byte[] ToStackValue(bool val)
        {
            return val ? ExecutionContext.True : ExecutionContext.False;
        }

        public static byte[] True
        {
            get
            {
                return new byte[] { 0x01 };
            }
        }

        public static byte[] False
        {
            get
            {
                return new byte[] { 0x00 };
            }
        }
    }
}
