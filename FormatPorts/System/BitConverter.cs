using System.Runtime.CompilerServices;

namespace Backports.System
{
    internal static class BitConverter
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Int32BitsToSingle(int value) => Unsafe.As<int, float>(ref value);
    }
}
