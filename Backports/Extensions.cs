﻿#if NETSTANDARD2_0

using System;
using System.Globalization;
using System.Runtime.CompilerServices;

using nint = System.IntPtr;



namespace Backports
{
    internal static class Extensions
    {

        // private const NumberStyles InvalidNumberStyles = unchecked((NumberStyles) 0xFFFFFC00);
        private const NumberStyles InvalidNumberStyles = ~(NumberStyles.AllowLeadingWhite |
                                                           NumberStyles.AllowTrailingWhite
                                                           | NumberStyles.AllowLeadingSign |
                                                           NumberStyles.AllowTrailingSign
                                                           | NumberStyles.AllowParentheses |
                                                           NumberStyles.AllowDecimalPoint
                                                           | NumberStyles.AllowThousands | NumberStyles.AllowExponent
                                                           | NumberStyles.AllowCurrencySymbol |
                                                           NumberStyles.AllowHexSpecifier);

        public static bool HasInvariantNumberSigns(this NumberFormatInfo info) =>
            info.PositiveSign == "+" && info.NegativeSign == "-";

        public static void ValidateParseStyleFloatingPoint(this NumberStyles style)
        {
            // Check for undefined flags or hex number
            if ((style & (InvalidNumberStyles | NumberStyles.AllowHexSpecifier)) == 0) return;

            throw (style & InvalidNumberStyles) != 0
                ? new ArgumentException("Invalid number style", nameof(style))
                : new ArgumentException("Hex stye not supported");
        }

        public static void ValidateParseStyleInteger(this NumberStyles style)
        {
            // Check for undefined flags or invalid hex number flags
            if ((style & (InvalidNumberStyles | NumberStyles.AllowHexSpecifier)) == 0 ||
                (style & ~NumberStyles.HexNumber) == 0) return;

            throw (style & InvalidNumberStyles) != 0
                ? new ArgumentException("Invalid number style", nameof(style))
                : new ArgumentException("Hex stye not supported");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFinite(this float f)
        {
            var bits = System.BitConverter.SingleToInt32Bits(f);
            return (bits & 0x7FFFFFFF) < 0x7F800000;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNegative(this float f) => System.BitConverter.SingleToInt32Bits(f) < 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static  bool IsFinite(this double d)
        {
            var bits = BitConverter.DoubleToInt64Bits(d);
            return (bits & 0x7FFFFFFFFFFFFFFF) < 0x7FF0000000000000;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNegative(this double d) => BitConverter.DoubleToInt64Bits(d) < 0;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNegative(this DecimalRep @this) => @this.Flags < 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNotEmpty<T>(this ReadOnlySpan<T> @this) where T : unmanaged => !@this.IsEmpty;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNotEmpty<T>(this Span<T> @this) where T : unmanaged => !@this.IsEmpty;

        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref DecimalRep AsBitsRep(ref this decimal d) => ref Unsafe.As<decimal, DecimalRep>(ref d);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref decimal BitsRepToDec(ref this DecimalRep rep) => ref Unsafe.As<DecimalRep, decimal>(ref rep);

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
            result = a - div * b;
            return div;
        }

        public static long DivRem(long a, long b, out long result)
        {
            var div = a / b;
            result = a - div * b;
            return div;
        }

        internal static uint DivRem(uint a, uint b, out uint result)
        {
            var div = a / b;
            result = a - div * b;
            return div;
        }

        internal static ulong DivRem(ulong a, ulong b, out ulong result)
        {
            var div = a / b;
            result = a - div * b;
            return div;
        }

    }

    internal static class Ref
    {
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T Inc<T>(in T @this) where T : unmanaged =>
            ref Unsafe.Add(ref Unsafe.AsRef(in @this), 1);
        

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T IncMut<T>(ref T @this) where T : unmanaged => ref Unsafe.Add(ref @this, 1);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T DecMut<T>(ref T @this) where T : unmanaged => ref Unsafe.Subtract(ref @this, 1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint Offset<T>(in T origin, in T target) where T : unmanaged =>
            new(Unsafe.ByteOffset(ref Unsafe.AsRef(in origin), ref Unsafe.AsRef(in target)).ToInt64() / Unsafe.SizeOf<T>());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint Offset(in byte origin, in byte target) =>
            Unsafe.ByteOffset(ref Unsafe.AsRef(in origin), ref Unsafe.AsRef(in target));


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T Add<T>(in T source, int elementOffset) where T : unmanaged =>
            ref Unsafe.Add(ref Unsafe.AsRef(in source), elementOffset);
    }
}

#endif