﻿using System;
using System.Collections.Generic;
using System.Text;
using RefTools;

namespace Backports
{
    internal static class DateTimeExtensions
    {
        private const               ulong TicksMask = 0x3FFFFFFFFFFFFFFF;
        // Number of 100ns ticks per time unit
        private const               long  TicksPerMillisecond = 10000;
        private const               long  TicksPerSecond      = TicksPerMillisecond * 1000;
        private const               long  TicksPerMinute      = TicksPerSecond      * 60;
        private const               long  TicksPerHour        = TicksPerMinute      * 60;
        private const               long  TicksPerDay         = TicksPerHour        * 24;

        // Number of days in a non-leap year
        private const               int   DaysPerYear = 365;
        // Number of days in 4 years
        private const               int   DaysPer4Years = DaysPerYear * 4 + 1; // 1461
        // Number of days in 100 years
        private const               int   DaysPer100Years = DaysPer4Years * 25 - 1; // 36524
        // Number of days in 400 years
        private const               int   DaysPer400Years = DaysPer100Years * 4 + 1; // 146097
        
        private static readonly     int[] SDaysToMonth365 = {
            0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334, 365 };
        private static readonly     int[] SDaysToMonth366 = {
            0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335, 366 };
        private static ref readonly ulong GetDateData(in DateTime @this) => ref Ref.As<DateTime, ulong>(in @this);

        private static long InternalTicks(this DateTime @this) => (long)(GetDateData(@this) & TicksMask);

        // Exactly the same as GetDatePart, except computing all of
        // year/month/day rather than just one of them. Used when all three
        // are needed rather than redoing the computations for each.
        public static void GetDate(this DateTime @this, out int year, out int month, out int day)
        {
            var ticks = @this.InternalTicks();
            // n = number of days since 1/1/0001
            var n = (int)(ticks / TicksPerDay);
            // y400 = number of whole 400-year periods since 1/1/0001
            var y400 = n / DaysPer400Years;
            // n = day number within 400-year period
            n -= y400 * DaysPer400Years;
            // y100 = number of whole 100-year periods within 400-year period
            var y100 = n / DaysPer100Years;
            // Last 100-year period has an extra day, so decrement result if 4
            if (y100 == 4) y100 = 3;
            // n = day number within 100-year period
            n -= y100 * DaysPer100Years;
            // y4 = number of whole 4-year periods within 100-year period
            var y4 = n / DaysPer4Years;
            // n = day number within 4-year period
            n -= y4 * DaysPer4Years;
            // y1 = number of whole years within 4-year period
            var y1 = n / DaysPerYear;
            // Last year has an extra day, so decrement result if 4
            if (y1 == 4) y1 = 3;
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
            while (n >= days[m]) m++;
            // compute month and day
            month = m;
            day = n - days[m - 1] + 1;
        }
    }
}
