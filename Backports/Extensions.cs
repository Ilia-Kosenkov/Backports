#if NETSTANDARD2_0

using System.Globalization;

namespace Backports
{
    internal static class Extensions
    {
        
        public static bool HasInvariantNumberSigns(this NumberFormatInfo info) =>
            info.PositiveSign == "+" && info.NegativeSign == "-";
    }

    internal static class MathP
    {
        public static int Clamp(int value, int min, int max)
        {
            if (value < min)
                return min;
            return value > max ? max : value;
        }

        public static int DivRem(int a, int b, out int result)
        {
            // TODO https://github.com/dotnet/runtime/issues/5213:
            // Restore to using % and / when the JIT is able to eliminate one of the idivs.
            // In the meantime, a * and - is measurably faster than an extra /.

            var div = a / b;
            result = a - (div * b);
            return div;
        }

        public static long DivRem(long a, long b, out long result)
        {
            var div = a / b;
            result = a - (div * b);
            return div;
        }

        internal static uint DivRem(uint a, uint b, out uint result)
        {
            var div = a / b;
            result = a - (div * b);
            return div;
        }

        internal static ulong DivRem(ulong a, ulong b, out ulong result)
        {
            var div = a / b;
            result = a - (div * b);
            return div;
        }



    }
}

#endif