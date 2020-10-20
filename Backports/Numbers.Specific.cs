using System;
using System.Runtime.CompilerServices;
using System.Globalization;
#if NETSTANDARD2_0
using Backports.System;
#endif

namespace Backports
{
    public static partial class Numbers
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParseDouble(this ReadOnlySpan<char> input, out double value) =>
#if NETSTANDARD2_0
            Number.TryParseDouble(input, NumberStyles.Float | NumberStyles.AllowThousands,
                NumberFormatInfo.CurrentInfo, out value);
#else
            double.TryParse(input, out value);
#endif
    }
}
