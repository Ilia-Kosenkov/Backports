using System;
#if NETSTANDARD2_0
using System.Globalization;
using Backports.System;
#endif

namespace Backports

{
    public static class Exports
    {
        public static bool TryParseInto<T>(this ReadOnlySpan<char> input, out T value) where T : unmanaged
        {
#if NETSTANDARD2_0
            return TryParseIntoBackported(input, out value);
#else
            return TryParseIntoRuntime(input, out value);
#endif
        }

#if NETSTANDARD2_0
        private static bool TryParseIntoBackported<T>(this ReadOnlySpan<char> input, out T value) where T : unmanaged
        {
            if (typeof(T) == typeof(int))
            {
                var wasSuccessful = Number.TryParseInt32IntegerStyle(input, NumberStyles.Integer,
                    NumberFormatInfo.CurrentInfo, out var result) is Number.ParsingStatus.OK;
                value = (T)(object)result;
                return wasSuccessful;
            }
            throw new NotSupportedException($"{typeof(T)} has no compatible TryParse method");
        }
#else
        private static bool TryParseIntoRuntime<T>(this ReadOnlySpan<char> input, out T value) where T : unmanaged
        {
            if (typeof(T) == typeof(int))
            {
                var wasSuccessful = int.TryParse(input, out var result);
                value = (T)(object)result;
                return wasSuccessful;
            }
            throw new NotSupportedException($"{typeof(T)} has no compatible TryParse method");
        }
#endif
    }
}
