#if NETSTANDARD2_0

using System.Runtime.CompilerServices;

namespace Backports.System
{
    internal static class Buffer
    {
        public static void ZeroMemory(ref byte ptr, uint len) => 
            Unsafe.InitBlock(ref ptr, 0, len);
        public static void Memcpy(ref byte dest, in byte src, int len) =>
            // Using AsRef to bypass limitations
            Unsafe.CopyBlock(ref dest, ref Unsafe.AsRef(in src), (uint)len);

        public static void Memcpy<T>(ref T dest, in T src, int len) where T : unmanaged =>
            Unsafe.CopyBlock(ref Unsafe.As<T, byte>(ref dest), ref Unsafe.As<T, byte>(ref Unsafe.AsRef(in src)),
                (uint) (Unsafe.SizeOf<T>() * len));
    }
}

#endif