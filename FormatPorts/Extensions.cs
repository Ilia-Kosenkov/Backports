using System;
using System.Globalization;
using System.Reflection;

namespace Backports
{
    internal static class Extensions
    {
        private static PropertyInfo? _getHasInvariantNumberSigns;


        public static bool HasInvariantNumberSigns(this NumberFormatInfo info) =>
            (bool) (_getHasInvariantNumberSigns ??=
                    typeof(NumberFormatInfo).GetProperty(nameof(HasInvariantNumberSigns),
                        BindingFlags.NonPublic | BindingFlags.Instance) ??
                    throw new InvalidOperationException("Failed to reflect property"))
            .GetValue(info);
    }

    internal static class Math
    {
        public static int Clamp(int value, int min, int max)
        {
            if (value < min)
                return min;
            return value > max ? max : value;
        }
            


    }
}
