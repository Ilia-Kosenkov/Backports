#if NETSTANDARD2_0
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Backports
{
    [StructLayout(LayoutKind.Sequential)]
    // ReSharper disable once InconsistentNaming
    internal ref struct BigIntBuff128
    {
        
        [StructLayout(LayoutKind.Sequential)]
        internal struct Byte32
        {
            // 0
            public ulong field1;
            // 8
            public ulong field2;
            // 16
            public ulong field3;
            // 24
            public ulong field4;
            // 32
        }

        // 0
        public Byte32 field1;
        // 32
        public Byte32 field2;
        // 64
        public Byte32 field3;
        // 96
        public Byte32 field4;
        // 128


        public static ref T GetRef<T>(ref BigIntBuff128 buff) where T : unmanaged => ref Unsafe.As<ulong, T>(ref buff.field1.field1);
    }
}
#endif