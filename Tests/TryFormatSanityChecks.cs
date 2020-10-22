using System;
using System.Collections.Generic;
using System.Linq;
using Backports;
using NUnit.Framework;

namespace Tests
{
    public class Int32TryFormatSource
    {
        public static IEnumerable<int> IntValues { get; } = new[] {int.MaxValue, int.MinValue, 0, 4, -4, 42};

        public static IEnumerable<string> Formats { get; } = new[] {string.Empty, "C", "X", "G", "E", "E8", "G10", "D5", "D05", "###.0###"};

        

        public static IEnumerable<TestCaseData> TryFormatData =>
            from x in IntValues
            from y in Formats
            select new TestCaseData(x, y);
    }

    [TestFixture]
    public class TryFormatSanityChecks
    {
        [Test]
        [TestCaseSource(typeof(Int32TryFormatSource), nameof(Int32TryFormatSource.TryFormatData))]
        public void Test_TryFormatInt32_Fmt(int value, string format)
        {
            Span<char> buff = stackalloc char[32];
            var wasFormatted = value.TryFormat<int>(buff, out var charsWritten, format.AsSpan());
            Assert.IsTrue(wasFormatted);

            var str = buff.Slice(0, charsWritten).ToString();
            Assert.AreEqual(value.ToString(format), str);
        }
    }
}
