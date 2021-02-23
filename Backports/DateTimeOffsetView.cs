//#if NETSTANDARD2_0
using System;

namespace Backports
{
    internal readonly struct DateTimeOffsetView
    {
        public readonly DateTime DateTime;
        public readonly short    Offset;
        public DateTimeOffsetView(DateTime dateTime, short offset)
        {
            Offset = offset;
            DateTime = dateTime;
        }
    }
}
//#endif