#if NETSTANDARD2_0

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using RefTools;

namespace Backports
{
    internal static class DateTimeExtensions
    {
        private const ulong TicksMask = 0x3FFFFFFFFFFFFFFF;

        // Number of 100ns ticks per time unit
        private const  long TicksPerMillisecond = 10000;
        private const  long TicksPerSecond      = TicksPerMillisecond * 1000;
        private const  long TicksPerMinute      = TicksPerSecond      * 60;
        private const  long TicksPerHour        = TicksPerMinute      * 60;
        internal const long TicksPerDay         = TicksPerHour        * 24;

        // Number of days in a non-leap year
        private const int DaysPerYear = 365;

        // Number of days in 4 years
        private const int DaysPer4Years = DaysPerYear * 4 + 1; // 1461

        // Number of days in 100 years
        private const int DaysPer100Years = DaysPer4Years * 25 - 1; // 36524

        // Number of days in 400 years
        private const int DaysPer400Years = DaysPer100Years * 4 + 1; // 146097

        private static readonly int[] SDaysToMonth365 =
        {
            0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334, 365
        };

        private static readonly int[] SDaysToMonth366 =
        {
            0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335, 366
        };

        private static ref readonly ulong GetDateData(in DateTime @this) => ref Ref.As<DateTime, ulong>(in @this);

        private static long InternalTicks(this DateTime @this) => (long) (GetDateData(@this) & TicksMask);

        // Exactly the same as GetDatePart, except computing all of
        // year/month/day rather than just one of them. Used when all three
        // are needed rather than redoing the computations for each.
        public static void GetDate(this DateTime @this, out int year, out int month, out int day)
        {
            var ticks = @this.InternalTicks();
            // n = number of days since 1/1/0001
            var n = (int) (ticks / TicksPerDay);
            // y400 = number of whole 400-year periods since 1/1/0001
            var y400 = n / DaysPer400Years;
            // n = day number within 400-year period
            n -= y400 * DaysPer400Years;
            // y100 = number of whole 100-year periods within 400-year period
            var y100 = n / DaysPer100Years;
            // Last 100-year period has an extra day, so decrement result if 4
            if (y100 == 4)
                y100 = 3;
            // n = day number within 100-year period
            n -= y100 * DaysPer100Years;
            // y4 = number of whole 4-year periods within 100-year period
            var y4 = n / DaysPer4Years;
            // n = day number within 4-year period
            n -= y4 * DaysPer4Years;
            // y1 = number of whole years within 4-year period
            var y1 = n / DaysPerYear;
            // Last year has an extra day, so decrement result if 4
            if (y1 == 4)
                y1 = 3;
            // compute year
            year = y400 * 400 + y100 * 100 + y4 * 4 + y1 + 1;
            // n = day number within year
            n -= y1 * DaysPerYear;
            // dayOfYear = n + 1;
            // Leap year calculation looks different from IsLeapYear since y1, y4,
            // and y100 are relative to year 1, not year 0
            var leapYear = y1 == 3 && (y4 != 24 || y100 == 3);
            int[] days = leapYear ? SDaysToMonth366 : SDaysToMonth365;
            // All months have less than 32 days, so n >> 5 is a good conservative
            // estimate for the month
            var m = (n >> 5) + 1;
            // m = 1-based month number
            while (n >= days[m])
                m++;
            // compute month and day
            month = m;
            day = n - days[m - 1] + 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void GetTime(this DateTime @this, out int hour, out int minute, out int second)
        {
            var n = @this.InternalTicks() / TicksPerSecond;
            n = Math.DivRem(n, 60, out var m);
            second = (int) m;
            n = Math.DivRem(n, 60, out m);
            minute = (int) m;
            hour = (int) (n % 24);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void GetTimePrecise(
            this DateTime @this, out int hour, out int minute, out int second,
            out  int      tick
        )
        {
            var n = Math.DivRem(@this.InternalTicks(), TicksPerSecond, out var m);
            tick = (int) m;
            n = Math.DivRem(n, 60, out m);
            second = (int) m;
            n = Math.DivRem(n, 60, out m);
            minute = (int) m;
            hour = (int) (n % 24);
        }

        /// Return the default pattern DateTimeOffset : shortDate + long time + time zone offset.
        /// This is used by DateTimeFormat.cs to get the pattern for short Date + long time +  time zone offset
        /// We put this internal property here so that we can avoid doing the
        /// concatation every time somebody uses this form.
        internal static string DateTimeOffsetPattern(this DateTimeFormatInfo @this)
        {
            /* LongTimePattern might contain a "z" as part of the format string in which case we don't want to append a time zone offset */

            bool foundZ = false;
            bool inQuote = false;
            char quote = '\'';
            for (int i = 0; !foundZ && i < @this.LongTimePattern.Length; i++)
            {
                switch (@this.LongTimePattern[i])
                {
                    case 'z':
                        /* if we aren't in a quote, we've found a z */
                        foundZ = !inQuote;
                        /* we'll fall out of the loop now because the test includes !foundZ */
                        break;
                    case '\'':
                    case '\"':
                        if (inQuote && (quote == @this.LongTimePattern[i]))
                        {
                            /* we were in a quote and found a matching exit quote, so we are outside a quote now */
                            inQuote = false;
                        }
                        else if (!inQuote)
                        {
                            quote = @this.LongTimePattern[i];
                            inQuote = true;
                        }
                        else
                        {
                            /* we were in a quote and saw the other type of quote character, so we are still in a quote */
                        }

                        break;
                    case '%':
                    case '\\':
                        i++; /* skip next character that is escaped by this backslash */
                        break;
                    default:
                        break;
                }
            }

            dateTimeOffsetPattern =
                foundZ
                    ? ShortDatePattern + " " + LongTimePattern
                    : ShortDatePattern + " " + LongTimePattern + " zzz";

        }
    }
}
#endif
