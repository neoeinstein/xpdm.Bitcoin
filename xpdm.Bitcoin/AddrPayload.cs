﻿using System;
using System.Diagnostics.Contracts;

namespace xpdm.Bitcoin
{
    public class AddrPayload : MessagePayloadBase
    {
        public override string Command
        {
            get { return AddrPayload.CommandText; }
        }

        public override uint ByteSize
        {
            get
            {
                return Count.ByteSize + (uint)_addressList.Length * (_addressList.Length > 0 ? _addressList[0].ByteSize : 0);
            }
        }

        public VarInt Count { get; private set; }

        private TimestampedNetworkAddress[] _addressList;
        public TimestampedNetworkAddress[] AddressList
        {
            get
            {
                var retVal = new TimestampedNetworkAddress[_addressList.Length];
                Array.Copy(_addressList, retVal, _addressList.Length);

                return retVal;
            }
        }

        public AddrPayload(TimestampedNetworkAddress[] addressList)
        {
            Contract.Requires<ArgumentNullException>(addressList != null);

            _addressList = new TimestampedNetworkAddress[addressList.Length];
            Array.Copy(addressList, _addressList, addressList.Length);
            Count = new VarInt((uint)_addressList.Length);
        }

        public AddrPayload(byte[] buffer, int offset)
        {
            Count = new VarInt(buffer, offset);
            var addrList = new TimestampedNetworkAddress[Count];
            int off = offset + (int)Count.ByteSize;
            for (uint i = 0; i < Count; ++i)
            {
                addrList[i] = new TimestampedNetworkAddress(buffer, off);
                off += (int)addrList[i].ByteSize;
            }
            _addressList = addrList;
        }

        private const uint INCLUDE_TIMESTAMP_VERSION = 31402;

        public override void WriteToBitcoinBuffer(byte[] buffer, int offset)
        {
            Count.WriteToBitcoinBuffer(buffer, offset);
            int off = offset + (int)Count.ByteSize;
            foreach(var addr in _addressList)
            {
                addr.WriteToBitcoinBuffer(buffer, off);
                off += (int)addr.ByteSize;
            }
        }

        public static string CommandText
        {
            get { return "addr"; }
        }

        public static int MinimumByteSize
        {
            get { return VarInt.MinimumByteSize; }
        }

        [ContractInvariantMethod]
        private void __Invariant()
        {
            Contract.Invariant((ulong)_addressList.Length == Count);
        }
    }
}
