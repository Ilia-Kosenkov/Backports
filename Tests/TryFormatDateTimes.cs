using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public static IEnumerable<string> Formats
        {
            get
            {
                yield return "O";
                yield return "R";
            }
        }

        public static IEnumerable DateTime_TestCaseData =>
            from x in Dates
            from y in Formats
            select new TestCaseData(x, y);

    }

    [TestFixture]
    public class TryFormatDateTimes
    {
        
        [Test]
        [TestCaseSource(typeof(TryFormatDateTimesProvider), nameof(TryFormatDateTimesProvider.DateTime_TestCaseData))]
        public void Test_DateTime(DateTime input, string format)
        {
            Span<char> buff = stackalloc char[128];
            Assert.IsTrue(input.TryFormat(buff, out var nChars, format.AsSpan()));
            Assert.AreEqual(buff.Slice(0, nChars).ToString(), input.ToString(format));
        }
    }
}
