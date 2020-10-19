#if NETSTANDARD2_0

using System.Runtime.CompilerServices;

namespace Backports.System
{
    internal static class Buffer
    {
        public static void ZeroMemory(ref byte ptr, uint len) => 
            Unsafe.InitBlock(ref ptr, 0, len);
        public static void Memcpy(ref byte dest, ref byte src, int len) =>
            Unsafe.CopyBlock(ref dest, ref src, (uint)global::System.Math.Max(0, len));
    }
}

#endif