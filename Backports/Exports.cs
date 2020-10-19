using System;
using System.Runtime.CompilerServices;
#if NETSTANDARD2_0
using System.Globalization;
using Backports.System;
#endif

namespace Backports

{
    public static class Exports
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
                value = Unsafe.As<int, T>(ref result);
                return wasSuccessful;
            }
            if (typeof(T) == typeof(uint))
            {
                var wasSuccessful = Number.TryParseUInt32IntegerStyle(input, NumberStyles.Integer,
                    NumberFormatInfo.CurrentInfo, out var result) is Number.ParsingStatus.OK;
                value = Unsafe.As<uint, T>(ref result);
                return wasSuccessful;
            }
            if (typeof(T) == typeof(long))
            {
                var wasSuccessful = Number.TryParseInt64IntegerStyle(input, NumberStyles.Integer,
                    NumberFormatInfo.CurrentInfo, out var result) is Number.ParsingStatus.OK;
                value = Unsafe.As<long, T>(ref result);
                return wasSuccessful;
            }
            if (typeof(T) == typeof(ulong))
            {
                var wasSuccessful = Number.TryParseUInt64IntegerStyle(input, NumberStyles.Integer,
                    NumberFormatInfo.CurrentInfo, out var result) is Number.ParsingStatus.OK;
                value = Unsafe.As<ulong, T>(ref result);
                return wasSuccessful;
            }
            if (typeof(T) == typeof(float))
            {
                var wasSuccessful = Number.TryParseSingle(input, NumberStyles.Float | NumberStyles.AllowThousands,
                    NumberFormatInfo.CurrentInfo, out var result);
                value = Unsafe.As<float, T>(ref result);;
                return wasSuccessful;
            }
            if (typeof(T) == typeof(double))
            {
                var wasSuccessful = Number.TryParseDouble(input, NumberStyles.Float | NumberStyles.AllowThousands,
                    NumberFormatInfo.CurrentInfo, out var result);
                value = Unsafe.As<double, T>(ref result);;
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
                value = Unsafe.As<int, T>(ref result);
                return wasSuccessful;
            }
            if(typeof(T) == typeof(long)) 
            {
                var wasSuccessful = long.TryParse(input, out var result);
                value = Unsafe.As<long, T>(ref result);
                return wasSuccessful;
            }
            if (typeof(T) == typeof(float)) 
            {
                var wasSuccessful = float.TryParse(input, out var result);
                value = Unsafe.As<float, T>(ref result);
                return wasSuccessful;
            }
            if (typeof(T) == typeof(double)) 
            {
                var wasSuccessful = double.TryParse(input, out var result);
                value = Unsafe.As<double, T>(ref result);
                return wasSuccessful;
            }
            throw new NotSupportedException($"{typeof(T)} has no compatible TryParse method");
        }
#endif
    }
}
