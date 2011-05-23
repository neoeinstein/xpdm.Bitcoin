using System.Diagnostics.Contracts;
using System.Numerics;

namespace xpdm.Bitcoin.Core
{
    public static class Difficulty
    {
        [Pure]
        public static BigInteger FromCompact(uint compact)
        {
            uint size = compact >> 24;
            var bytes = new byte[size + 1];
            if (size >= 1) bytes[size - 2] = (byte)(compact >> 16);
            if (size >= 2) bytes[size - 3] = (byte)(compact >> 8);
            if (size >= 3) bytes[size - 4] = (byte)(compact >> 0);
            return new BigInteger(bytes);
        }

        [Pure]
        public static uint ToCompact(this BigInteger value)
        {
            var fullBytes = value.ToByteArray();
            uint compact = (uint)fullBytes.Length;
            while (compact > 0 && fullBytes[compact - 1] == 0) --compact;
            var size = compact;
            compact <<= 24;
            if (size >= 1) compact |= (uint)fullBytes[size - 1] << 16;
            if (size >= 2) compact |= (uint)fullBytes[size - 2] << 8;
            if (size >= 3) compact |= (uint)fullBytes[size - 3] << 0;
            return compact;
        }

        public static readonly uint TargetTimespanSeconds = 14 * 24 * 60 * 60; // 2 weeks
        public static readonly uint TargetSpacingSeconds = 10 * 60; // 10 minutes
        public static readonly uint BlockInterval = TargetTimespanSeconds / TargetSpacingSeconds; // 2016

        public const uint MaxDifficultyBits = 0x00000000U;
        public const uint OneDifficultyBits = 0x1d00FFFFU;
        public const uint MinDifficultyBits = 0x23FFFFFFU;

        public const uint DifficultyValueMask = 0x00FFFFFFU;
        public const uint DifficultyRotateMask = 0xFF000000U;
    }
}
