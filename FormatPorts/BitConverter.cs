using System.Runtime.CompilerServices;

namespace System.BackPorts
{
    internal static class BitConverter
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Int32BitsToSingle(int value) => Unsafe.As<int, float>(ref value);
    }
}
