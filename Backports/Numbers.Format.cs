using System;
using System.Runtime.CompilerServices;

namespace Backports
{
    public static partial class Numbers
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryFormat<T>(this T @this, Span<char> destination, out int charsWritten,
            ReadOnlySpan<char> format = default, IFormatProvider? provider = null) where T : unmanaged
        {
#if NETSTANDARD2_0
            return TryFormatBackported(@this, destination, out charsWritten, format, provider);

#else
            return TryFormatRuntime(@this, destination, out charsWritten, format, provider);
#endif
        }

#if NETSTANDARD2_0
        private static bool TryFormatBackported<T>(this T @this, Span<char> destination, out int charsWritten,
            ReadOnlySpan<char> format = default, IFormatProvider? provider = null) where T : unmanaged
        {
            if (typeof(T) == typeof(int))
                return System.Number.TryFormatInt32(Unsafe.As<T, int>(ref @this), ~0, format, provider, destination,
                    out charsWritten);
            throw TypeDoesNotSupportTryFormat<T>();
        }
#else
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryFormatRuntime<T>(this T @this, Span<char> destination, out int charsWritten,
            ReadOnlySpan<char> format = default, IFormatProvider? provider = null) where T : unmanaged
        {
            if (typeof(T) == typeof(int))
                return Unsafe.As<T, int>(ref @this).TryFormat(destination, out charsWritten, format, provider);
            if (typeof(T) == typeof(uint))
                return Unsafe.As<T, uint>(ref @this).TryFormat(destination, out charsWritten, format, provider);
            if (typeof(T) == typeof(long))
                return Unsafe.As<T, long>(ref @this).TryFormat(destination, out charsWritten, format, provider);
            if (typeof(T) == typeof(ulong))
                return Unsafe.As<T, ulong>(ref @this).TryFormat(destination, out charsWritten, format, provider);
            throw TypeDoesNotSupportTryFormat<T>();
        }
#endif
        private static void Test()
        {
            
            5.TryFormat(Span<char>.Empty, out _);
        }

        private static Exception TypeDoesNotSupportTryFormat<T>() where T : unmanaged => new NotSupportedException($"{typeof(T)} has no compatible TryFormat method");

    }
}
