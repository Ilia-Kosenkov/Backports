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
            if (typeof(T) == typeof(sbyte))
            {
                // For hex number styles AllowHexSpecifier >> 2 == 0x80 and cancels out MinValue so the check is effectively: (uint)i > byte.MaxValue
                // For integer styles it's zero and the effective check is (uint)(i - MinValue) > byte.MaxValue
                if (Number.TryParseInt32(input, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out var i) != Number.ParsingStatus.OK
                    || (uint)(i - sbyte.MinValue) > byte.MaxValue)
                {
                    value = default;
                    return false;
                }
                var sbyteVal = (sbyte)i;
                value = Unsafe.As<sbyte, T>(ref sbyteVal);
                return true;
            }

            if (typeof(T) == typeof(byte))
            {
                if (Number.TryParseUInt32(input, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out var i) != Number.ParsingStatus.OK
                    || i > byte.MaxValue)
                {
                    value = default;
                    return false;
                }

                var byteVal = (byte) i;
                value = Unsafe.As<byte, T>(ref byteVal);
                return true;
            }
            if (typeof(T) == typeof(short))
            {
                // For hex number styles AllowHexSpecifier << 6 == 0x8000 and cancels out MinValue so the check is effectively: (uint)i > ushort.MaxValue
                // For integer styles it's zero and the effective check is (uint)(i - MinValue) > ushort.MaxValue
                if (Number.TryParseInt32(input, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out var i) != Number.ParsingStatus.OK
                    || (uint)(i - short.MinValue) > ushort.MaxValue)
                {
                    value = default;
                    return false;
                }

                var shortVal = (short) i;
                value = Unsafe.As<short, T>(ref shortVal);
                return true;

            }
            if (typeof(T) == typeof(ushort))
            {
                if (Number.TryParseUInt32(input, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out var i) != Number.ParsingStatus.OK
                    || i > ushort.MaxValue)
                {
                    value = default;
                    return false;
                }

                var ushortVal = (ushort) i;
                value = Unsafe.As<ushort, T>(ref ushortVal);
                return true;
            }
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

            if (typeof(T) == typeof(sbyte))
            {
                var wasSuccessful = sbyte.TryParse(input, out var result);
                value = Unsafe.As<sbyte, T>(ref result);
                return wasSuccessful;
            }
            if (typeof(T) == typeof(byte))
            {
                var wasSuccessful = byte.TryParse(input, out var result);
                value = Unsafe.As<byte, T>(ref result);
                return wasSuccessful;
            }

            if (typeof(T) == typeof(short))
            {
                var wasSuccessful = short.TryParse(input, out var result);
                value = Unsafe.As<short, T>(ref result);
                return wasSuccessful;
            }
            if (typeof(T) == typeof(ushort))
            {
                var wasSuccessful = ushort.TryParse(input, out var result);
                value = Unsafe.As<ushort, T>(ref result);
                return wasSuccessful;
            }
            if (typeof(T) == typeof(int))
            {
                var wasSuccessful = int.TryParse(input, out var result);
                value = Unsafe.As<int, T>(ref result);
                return wasSuccessful;
            }
            if (typeof(T) == typeof(uint))
            {
                var wasSuccessful = uint.TryParse(input, out var result);
                value = Unsafe.As<uint, T>(ref result);
                return wasSuccessful;
            }
            if (typeof(T) == typeof(long)) 
            {
                var wasSuccessful = long.TryParse(input, out var result);
                value = Unsafe.As<long, T>(ref result);
                return wasSuccessful;
            }
            if (typeof(T) == typeof(ulong))
            {
                var wasSuccessful = ulong.TryParse(input, out var result);
                value = Unsafe.As<ulong, T>(ref result);
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
