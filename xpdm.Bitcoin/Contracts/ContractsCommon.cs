using System;
using System.Diagnostics.Contracts;
using SCG = System.Collections.Generic;
using xpdm.Bitcoin;
using xpdm.Bitcoin.Core;
using xpdm.Bitcoin.Scripting;

namespace xpdm.Bitcoin
{
    internal static class ContractsCommon
    {
        [ContractAbbreviator]
        public static void NotFrozen(object freezable)
        {
            Contract.Requires<ObjectFrozenException>(!(freezable is IFreezable) || !((IFreezable)freezable).IsFrozen);
        }

        [ContractAbbreviator]
        public static void NotNull(object param, string paramName)
        {
            Contract.Requires<ArgumentNullException>(param != null, paramName);
        }

        [ContractAbbreviator]
        public static void CanWriteToStream(System.IO.Stream stream, int expectedWriteLength)
        {
            ContractsCommon.CanWriteToStream(stream, "stream", expectedWriteLength);
        }

        [ContractAbbreviator]
        public static void CanWriteToStream(System.IO.Stream stream, string streamParamName, int expectedWriteLength)
        {
            ContractsCommon.NotNull(stream, streamParamName);
            Contract.Requires<ArgumentException>(stream.CanWrite, "Stream does not support writing: '" + streamParamName + "'.");
        }

        [ContractAbbreviator]
        public static void ValidIndex(int minimumInclusive, int maximumExclusive, int indexParam)
        {
            ValidIndex(minimumInclusive, maximumExclusive, indexParam, "index");
        }

        [ContractAbbreviator]
        public static void ValidIndex(int minimumInclusive, int maximumExclusive, int indexParam, string indexParamName)
        {
            Contract.Requires<IndexOutOfRangeException>(minimumInclusive <= indexParam && indexParam < maximumExclusive, indexParamName);
        }

        [ContractAbbreviator]
        public static void ValidOffset(int minimumInclusive, int maximumExclusive, int offsetParam)
        {
            ValidOffset(minimumInclusive, maximumExclusive, offsetParam, "offset");
        }

        [ContractAbbreviator]
        public static void ValidOffset(int minimumInclusive, int maximumExclusive, int offsetParam, string offsetParamName)
        {
            Contract.Requires<ArgumentOutOfRangeException>(minimumInclusive <= offsetParam && offsetParam < maximumExclusive, offsetParamName);
        }

        [ContractAbbreviator]
        public static void ValidLength(int minimumInclusive, int maximumInclusive, int lengthParam)
        {
            ValidLength(minimumInclusive, maximumInclusive, lengthParam, "length");
        }

        [ContractAbbreviator]
        public static void ValidLength(int minimumInclusive, int maximumInclusive, int lengthParam, string lengthParamName)
        {
            Contract.Requires<ArgumentOutOfRangeException>(minimumInclusive <= lengthParam && lengthParam <= maximumInclusive, lengthParamName);
        }

        [ContractAbbreviator]
        public static void ValidOffsetLength(int minimumIndexInclusive, int maximumIndexExclusive, int offsetParam, int lengthParam)
        {
            ValidOffsetLength(minimumIndexInclusive, maximumIndexExclusive, offsetParam, lengthParam, "offset", "length");
        }

        [ContractAbbreviator]
        public static void ValidOffsetLength(int minimumIndexInclusive, int maximumIndexExclusive, int offsetParam, int lengthParam, string offsetParamName, string lengthParamName)
        {
            ValidOffset(minimumIndexInclusive, maximumIndexExclusive, offsetParam, offsetParamName);
            ValidLength(minimumIndexInclusive, maximumIndexExclusive, lengthParam, lengthParamName);
            Contract.Requires<ArgumentOutOfRangeException>(minimumIndexInclusive <= offsetParam + lengthParam && offsetParam + lengthParam <= maximumIndexExclusive, lengthParamName);
        }

        [ContractAbbreviator]
        public static void ResultIsNonNull<T>()
        {
            Contract.Ensures(Contract.Result<T>() != null);
        }

        [ContractAbbreviator]
        public static void IsThawed<T>(IThawable<T> freezable) where T : IThawable<T>
        {
            Contract.Ensures(!freezable.IsFrozen);
        }

        [ContractAbbreviator]
        public static void IsThawed<T>(SCG.IEnumerable<IThawable<T>> freezables) where T : IThawable<T>
        {
            Contract.Ensures(Contract.ForAll(freezables, f => !f.IsFrozen));
        }

        [ContractAbbreviator]
        public static void ChildThawed<T>(IThawable<T> child, bool thawChildren) where T : IThawable<T>
        {
            Contract.Ensures(!thawChildren || !child.IsFrozen);
        }

        [ContractAbbreviator]
        public static void ChildrenThawed<T>(SCG.IEnumerable<IThawable<T>> children, bool thawChildren) where T : IThawable<T>
        {
            Contract.Ensures(!thawChildren || Contract.ForAll(children, child => !child.IsFrozen));
        }
    }
}
