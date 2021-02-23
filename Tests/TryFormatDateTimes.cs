using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Backports;
using NUnit.Framework;

namespace Tests
{
    public class TryFormatDateTimesProvider
    {
        public static IEnumerable<DateTime> Dates
        {
            get
            {
                yield return DateTime.Now;
                yield return DateTime.UtcNow;
                yield return DateTime.Today;
            }
        }

        public static IEnumerable<DateTimeOffset> DateOffsets
        {
            get
            {
                //yield return DateTimeOffset.Now;
                yield return DateTimeOffset.UtcNow;
            }
        }

        public static IEnumerable<string> Formats
        {
            get
            {
                yield return "O";
                yield return "R";
                yield return "d";
                yield return "D";
                yield return "f";
                yield return "F";
                yield return "g";
                yield return "G";
                yield return "t";
                yield return "T";
                yield return "u";
                yield return "U";
                yield return "m";
                yield return "M";
                yield return "s";
                yield return "%d \\d \\M";
                yield return "yyyy/MM/dd:HH:mm:ss.ffffff tt";
                yield return "yyyy/MM/dd:HH:mm:ss.ffffff tt zz";
                yield return "yyyy/MM/dddd FFFFF g  t K";
            }
        }

        public static IEnumerable<IFormatProvider> CultureInfo
        {
            get
            {
                yield return System.Globalization.CultureInfo.InvariantCulture;
                yield return new CultureInfo("en-US");
                yield return new CultureInfo("en-GB");
                yield return new CultureInfo("ru-RU");
                yield return new CultureInfo("fi-FI");
                yield return new CultureInfo("se-FI");
                yield return new CultureInfo("ja-JP");
                yield return new CultureInfo("he-IL");
                yield return new CultureInfo("he-IL")
                {
                    DateTimeFormat = {Calendar = new HebrewCalendar()}
                };

            }
        }

        public static IEnumerable DateTime_TestCaseData =>
            from x in Dates
            from y in Formats
            from z in CultureInfo
            // Does not use custom formats with UTC
            where x.Kind != DateTimeKind.Utc || y.Length < 30
            select new TestCaseData(x, y, z);

        public static IEnumerable DateTimeOffset_TestCaseData =>
            from x in DateOffsets
            from y in Formats
            from z in CultureInfo
            where y is not ("U" or "u")
            select new TestCaseData(x, y, z);
    }

    [TestFixture]
    public class TryFormatDateTimes
    {
        
        [Test]
        [TestCaseSource(typeof(TryFormatDateTimesProvider), nameof(TryFormatDateTimesProvider.DateTime_TestCaseData))]
        public void Test_DateTime(DateTime input, string format, IFormatProvider provider)
        {
            Span<char> buff = stackalloc char[128];
            Assert.IsTrue(input.TryFormat(buff, out var nChars, format.AsSpan(), provider));
            Assert.AreEqual(input.ToString(format, provider), buff.Slice(0, nChars).ToString());
        }

        [Test]
        [TestCaseSource(typeof(TryFormatDateTimesProvider), nameof(TryFormatDateTimesProvider.DateTimeOffset_TestCaseData))]
        public void Test_DateTimeOffset(DateTimeOffset input, string format, IFormatProvider provider)
        {
            Span<char> buff = stackalloc char[128];
            var result = input.TryFormat(buff, out var nChars, format.AsSpan(), provider);
            Assert.IsTrue(result);
            Assert.AreEqual(input.ToString(format, provider), buff.Slice(0, nChars).ToString());
        }

    }
}
