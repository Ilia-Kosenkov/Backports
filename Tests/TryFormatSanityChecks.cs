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

    public class UInt32TryFormatSource
    {
        public static IEnumerable<uint> IntValues { get; } = new uint[] { uint.MaxValue, uint.MinValue, 4, 10500, 42 };

        public static IEnumerable<string> Formats { get; } = new[] { string.Empty, "C", "X", "G", "E", "E8", "G10", "D5", "D05", "###.0###" };



        public static IEnumerable<TestCaseData> TryFormatData =>
            from x in IntValues
            from y in Formats
            select new TestCaseData(x, y);
    }

    public class Int64TryFormatSource
    {
        public static IEnumerable<long> IntValues { get; } = new[] { long.MaxValue, long.MinValue, 0, 4, -4, 42 };

        public static IEnumerable<string> Formats { get; } = new[] { string.Empty, "C", "X", "G", "E", "E8", "G10", "D5", "D05", "###.0###" };



        public static IEnumerable<TestCaseData> TryFormatData =>
            from x in IntValues
            from y in Formats
            select new TestCaseData(x, y);
    }

    public class UInt64TryFormatSource
    {
        public static IEnumerable<ulong> IntValues { get; } = new ulong[] { ulong.MaxValue, ulong.MinValue, 4, 10500, 42 };

        public static IEnumerable<string> Formats { get; } = new[] { string.Empty, "C", "X", "G", "E", "E8", "G10", "D5", "D05", "###.0###" };



        public static IEnumerable<TestCaseData> TryFormatData =>
            from x in IntValues
            from y in Formats
            select new TestCaseData(x, y);
    }

    public class SingleTryFormatSource
    {
        public static IEnumerable<float> IntValues { get; } = new [] { float.MinValue, float.MaxValue, (float)Math.PI, float.NaN, float.NegativeInfinity, float.PositiveInfinity,  4, 10500, 42 };

        public static IEnumerable<string> Formats { get; } = new[] { "G3", "G6", "E", "E6", "###.0###" };



        public static IEnumerable<TestCaseData> TryFormatData =>
            from x in IntValues
            from y in Formats
            select new TestCaseData(x, y);

        public static IEnumerable<TestCaseData> TryFormatSpecialCaseData
        {
            get
            {
                const float fPi = (float) Math.PI;
                yield return new TestCaseData(fPi, "G", "3.1415927");
                yield return new TestCaseData(fPi, "G7", "3.141593");
                yield return new TestCaseData(fPi, "G10", "3.141592741");
                yield return new TestCaseData(fPi, "G30", "3.1415927410125732421875");
                yield return new TestCaseData(fPi, "C", "$3.14");
                yield return new TestCaseData(fPi, "C10", "$3.1415927410");
                yield return new TestCaseData(fPi, "E", "3.141593E+000");
                yield return new TestCaseData(fPi, "E7", "3.1415927E+000");
                yield return new TestCaseData(fPi, "E10", "3.1415927410E+000");
                yield return new TestCaseData(fPi, "E30", "3.141592741012573242187500000000E+000");
                yield return new TestCaseData(fPi, "F", "3.14");
                yield return new TestCaseData(fPi, "F7", "3.1415927");
                yield return new TestCaseData(fPi, "F10", "3.1415927410");
                yield return new TestCaseData(fPi, "F30", "3.141592741012573242187500000000");

                yield return new TestCaseData(float.MinValue, "G", "-3.4028235E+38");
                yield return new TestCaseData(float.MinValue, "G7", "-3.402823E+38");
                yield return new TestCaseData(float.MinValue, "G10", "-3.402823466E+38");
                yield return new TestCaseData(float.MinValue, "G30", "-3.40282346638528859811704183485E+38");
                yield return new TestCaseData(float.MinValue, "C", "($340,282,346,638,528,859,811,704,183,484,516,925,440.00)");
                yield return new TestCaseData(float.MinValue, "C10", "($340,282,346,638,528,859,811,704,183,484,516,925,440.0000000000)");
                yield return new TestCaseData(float.MinValue, "E", "-3.402823E+038");
                yield return new TestCaseData(float.MinValue, "E7", "-3.4028235E+038");
                yield return new TestCaseData(float.MinValue, "E10", "-3.4028234664E+038");
                yield return new TestCaseData(float.MinValue, "E30", "-3.402823466385288598117041834845E+038");
                yield return new TestCaseData(float.MinValue, "F", "-340282346638528859811704183484516925440.00");
                yield return new TestCaseData(float.MinValue, "F7", "-340282346638528859811704183484516925440.0000000");
                yield return new TestCaseData(float.MinValue, "F10", "-340282346638528859811704183484516925440.0000000000");
                yield return new TestCaseData(float.MinValue, "F30", "-340282346638528859811704183484516925440.000000000000000000000000000000");

                yield return new TestCaseData(float.MaxValue, "G", "3.4028235E+38");
                yield return new TestCaseData(float.MaxValue, "G7", "3.402823E+38");
                yield return new TestCaseData(float.MaxValue, "G10", "3.402823466E+38");
                yield return new TestCaseData(float.MaxValue, "G30", "3.40282346638528859811704183485E+38");
                yield return new TestCaseData(float.MaxValue, "C", "$340,282,346,638,528,859,811,704,183,484,516,925,440.00");
                yield return new TestCaseData(float.MaxValue, "C10", "$340,282,346,638,528,859,811,704,183,484,516,925,440.0000000000");
                yield return new TestCaseData(float.MaxValue, "E", "3.402823E+038");
                yield return new TestCaseData(float.MaxValue, "E7", "3.4028235E+038");
                yield return new TestCaseData(float.MaxValue, "E10", "3.4028234664E+038");
                yield return new TestCaseData(float.MaxValue, "E30", "3.402823466385288598117041834845E+038");
                yield return new TestCaseData(float.MaxValue, "F", "340282346638528859811704183484516925440.00");
                yield return new TestCaseData(float.MaxValue, "F7", "340282346638528859811704183484516925440.0000000");
                yield return new TestCaseData(float.MaxValue, "F10", "340282346638528859811704183484516925440.0000000000");
                yield return new TestCaseData(float.MaxValue, "F30", "340282346638528859811704183484516925440.000000000000000000000000000000");
            }
        }
    }

    [TestFixture]
    public class TryFormatSanityChecks
    {
        [Test]
        [Parallelizable(ParallelScope.All)]
        [TestCaseSource(typeof(Int32TryFormatSource), nameof(Int32TryFormatSource.TryFormatData))]
        public void Test_TryFormatInt32_Fmt(int value, string format)
        {
            Span<char> buff = stackalloc char[32];
            var wasFormatted = value.TryFormat<int>(buff, out var charsWritten, format.AsSpan());
            Assert.IsTrue(wasFormatted);

            var str = buff.Slice(0, charsWritten).ToString();
            Assert.AreEqual(value.ToString(format), str);
        }

        [Test]
        [Parallelizable(ParallelScope.All)]
        [TestCaseSource(typeof(UInt32TryFormatSource), nameof(UInt32TryFormatSource.TryFormatData))]
        public void Test_TryFormatUInt32_Fmt(uint value, string format)
        {
            Span<char> buff = stackalloc char[32];
            var wasFormatted = value.TryFormat<uint>(buff, out var charsWritten, format.AsSpan());
            Assert.IsTrue(wasFormatted);

            var str = buff.Slice(0, charsWritten).ToString();
            Assert.AreEqual(value.ToString(format), str);
        }

        [Test]
        [Parallelizable(ParallelScope.All)]
        [TestCaseSource(typeof(Int64TryFormatSource), nameof(Int64TryFormatSource.TryFormatData))]
        public void Test_TryFormatInt64_Fmt(long value, string format)
        {
            Span<char> buff = stackalloc char[32];
            var wasFormatted = value.TryFormat<long>(buff, out var charsWritten, format.AsSpan());
            Assert.IsTrue(wasFormatted);

            var str = buff.Slice(0, charsWritten).ToString();
            Assert.AreEqual(value.ToString(format), str);
        }


        [Test]
        [Parallelizable(ParallelScope.All)]
        [TestCaseSource(typeof(UInt64TryFormatSource), nameof(UInt64TryFormatSource.TryFormatData))]
        public void Test_TryFormatUInt64_Fmt(ulong value, string format)
        {
            Span<char> buff = stackalloc char[32];
            var wasFormatted = value.TryFormat<ulong>(buff, out var charsWritten, format.AsSpan());
            Assert.IsTrue(wasFormatted);

            var str = buff.Slice(0, charsWritten).ToString();
            Assert.AreEqual(value.ToString(format), str);
        }

        [Test]
        [TestCaseSource(typeof(SingleTryFormatSource), nameof(SingleTryFormatSource.TryFormatData))]
        public void Test_TryFormatSingle_Fmt(float value, string format)
        {
            Span<char> buff = stackalloc char[64];
            var wasFormatted = value.TryFormat<float>(buff, out var charsWritten, format.AsSpan());
            Assert.IsTrue(wasFormatted);

            var str = buff.Slice(0, charsWritten).ToString();
            var expStr = value.ToString(format);
            Assert.AreEqual(expStr, str);
        }

        [Test]
        [TestCaseSource(typeof(SingleTryFormatSource), nameof(SingleTryFormatSource.TryFormatSpecialCaseData))]
        public void Test_TryFormatSingle_Fmt(float value, string format, string expected)
        {
            Span<char> buff = stackalloc char[128];
            var wasFormatted = value.TryFormat<float>(buff, out var charsWritten, format.AsSpan());
            Assert.IsTrue(wasFormatted);

            var str = buff.Slice(0, charsWritten).ToString();
            Assert.AreEqual(expected, str);
        }
    }
}
