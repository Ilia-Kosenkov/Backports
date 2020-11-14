using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Backports;
using NUnit.Framework;

namespace Tests
{
    public class Int32TryFormatSource
    {
        public static IEnumerable<int> IntValues { get; } = new[] {int.MaxValue, int.MinValue, 0, 4, -4, 42};

        public static IEnumerable<string> Formats { get; } = new[] {string.Empty, "C", "X", "G", "E", "E8", "G10", "D5", "D05", "###.0###", "P"};

        

        public static IEnumerable<TestCaseData> TryFormatData =>
            from x in IntValues
            from y in Formats
            select new TestCaseData(x, y);
    }

    public class UInt32TryFormatSource
    {
        public static IEnumerable<uint> IntValues { get; } = new uint[] { uint.MaxValue, uint.MinValue, 4, 10500, 42 };

        public static IEnumerable<string> Formats { get; } = new[] { string.Empty, "C", "X", "G", "E", "E8", "G10", "D5", "D05", "###.0###", "P" };



        public static IEnumerable<TestCaseData> TryFormatData =>
            from x in IntValues
            from y in Formats
            select new TestCaseData(x, y);
    }

    public class Int64TryFormatSource
    {
        public static IEnumerable<long> IntValues { get; } = new[] { long.MaxValue, long.MinValue, 0, 4, -4, 42 };

        public static IEnumerable<string> Formats { get; } = new[] { string.Empty, "C", "X", "G", "E", "E8", "G10", "D5", "D05", "###.0###", "P" };



        public static IEnumerable<TestCaseData> TryFormatData =>
            from x in IntValues
            from y in Formats
            select new TestCaseData(x, y);
    }

    public class UInt64TryFormatSource
    {
        public static IEnumerable<ulong> IntValues { get; } = new ulong[] { ulong.MaxValue, ulong.MinValue, 4, 10500, 42 };

        public static IEnumerable<string> Formats { get; } = new[] { string.Empty, "C", "X", "G", "E", "E8", "G10", "D5", "D05", "###.0###", "P"};



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
                yield return new TestCaseData(fPi, "F", "3.142");
                yield return new TestCaseData(fPi, "F7", "3.1415927");
                yield return new TestCaseData(fPi, "F10", "3.1415927410");
                yield return new TestCaseData(fPi, "F30", "3.141592741012573242187500000000");

                yield return new TestCaseData(float.MinValue, "G", "-3.4028235E+38");
                yield return new TestCaseData(float.MinValue, "G7", "-3.402823E+38");
                yield return new TestCaseData(float.MinValue, "G10", "-3.402823466E+38");
                yield return new TestCaseData(float.MinValue, "G30", "-3.40282346638528859811704183485E+38");
                yield return new TestCaseData(float.MinValue, "C", "-$340,282,346,638,528,859,811,704,183,484,516,925,440.00");
                yield return new TestCaseData(float.MinValue, "C10", "-$340,282,346,638,528,859,811,704,183,484,516,925,440.0000000000");
                yield return new TestCaseData(float.MinValue, "E", "-3.402823E+038");
                yield return new TestCaseData(float.MinValue, "E7", "-3.4028235E+038");
                yield return new TestCaseData(float.MinValue, "E10", "-3.4028234664E+038");
                yield return new TestCaseData(float.MinValue, "E30", "-3.402823466385288598117041834845E+038");
                yield return new TestCaseData(float.MinValue, "F", "-340282346638528859811704183484516925440.000");
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
                yield return new TestCaseData(float.MaxValue, "F", "340282346638528859811704183484516925440.000");
                yield return new TestCaseData(float.MaxValue, "F7", "340282346638528859811704183484516925440.0000000");
                yield return new TestCaseData(float.MaxValue, "F10", "340282346638528859811704183484516925440.0000000000");
                yield return new TestCaseData(float.MaxValue, "F30", "340282346638528859811704183484516925440.000000000000000000000000000000");
            }
        }
    }

    public class DoubleTryFormatSource
    {
        public static IEnumerable<double> IntValues { get; } = new[] { double.MinValue, double.MaxValue, Math.PI, double.NaN, double.NegativeInfinity, double.PositiveInfinity, 4, 10500, 42 };

        public static IEnumerable<string> Formats { get; } = new[] { "G3", "G6", "E", "E6", "###.0###" };



        public static IEnumerable<TestCaseData> TryFormatData =>
            from x in IntValues
            from y in Formats
            select new TestCaseData(x, y);

        public static IEnumerable<TestCaseData> TryFormatSpecialCaseData
        {
            get
            {
                
                const double pi = Math.PI;
                yield return new TestCaseData(pi, "G", "3.141592653589793");
                yield return new TestCaseData(pi, "G7", "3.141593");
                yield return new TestCaseData(pi, "G16", "3.141592653589793");
                yield return new TestCaseData(pi, "G30", "3.14159265358979311599796346854");
                yield return new TestCaseData(pi, "C", "$3.14");
                yield return new TestCaseData(pi, "C16", "$3.1415926535897931");
                yield return new TestCaseData(pi, "E", "3.141593E+000");
                yield return new TestCaseData(pi, "E7", "3.1415927E+000");
                yield return new TestCaseData(pi, "E16", "3.1415926535897931E+000");
                yield return new TestCaseData(pi, "E30", "3.141592653589793115997963468544E+000");
                yield return new TestCaseData(pi, "F", "3.142");
                yield return new TestCaseData(pi, "F7", "3.1415927");
                yield return new TestCaseData(pi, "F16", "3.1415926535897931");
                yield return new TestCaseData(pi, "F30", "3.141592653589793115997963468544");

                yield return new TestCaseData(double.MinValue, "G", "-1.7976931348623157E+308");
                yield return new TestCaseData(double.MinValue, "G7", "-1.797693E+308");
                yield return new TestCaseData(double.MinValue, "G16", "-1.797693134862316E+308");
                yield return new TestCaseData(double.MinValue, "G30", "-1.79769313486231570814527423732E+308");
                yield return new TestCaseData(double.MinValue, "E", "-1.797693E+308");
                yield return new TestCaseData(double.MinValue, "E7", "-1.7976931E+308");
                yield return new TestCaseData(double.MinValue, "E16", "-1.7976931348623157E+308");
                yield return new TestCaseData(double.MinValue, "E30", "-1.797693134862315708145274237317E+308");

                yield return new TestCaseData(double.MaxValue, "G", "1.7976931348623157E+308");
                yield return new TestCaseData(double.MaxValue, "G7", "1.797693E+308");
                yield return new TestCaseData(double.MaxValue, "G16", "1.797693134862316E+308");
                yield return new TestCaseData(double.MaxValue, "G30", "1.79769313486231570814527423732E+308");
                yield return new TestCaseData(double.MaxValue, "E", "1.797693E+308");
                yield return new TestCaseData(double.MaxValue, "E7", "1.7976931E+308");
                yield return new TestCaseData(double.MaxValue, "E16", "1.7976931348623157E+308");
                yield return new TestCaseData(double.MaxValue, "E30", "1.797693134862315708145274237317E+308");

            }
        }
    }

    public class DecimalTryFormatSource
    {
        public static IEnumerable<decimal> IntValues { get; } = new[] { decimal.MinValue, decimal.MaxValue, (decimal)Math.PI, decimal.Zero, decimal.One, -4, 10500, -42 };

        public static IEnumerable<string> Formats { get; } = new[] { "G3", "G6", "E", "E6", "F", "F6", "F10", "###.0###" };

        public static IEnumerable<string> Cultures { get; } = new[] {"en-US", "ru-RU", "fi-FI"};


        public static IEnumerable<TestCaseData> TryFormatData =>
            from x in IntValues
            from y in Formats
            from z in Cultures
            select new TestCaseData(x, y, new CultureInfo(z));

        public static IEnumerable<TestCaseData> TryFormatSpecialCaseData
        {
            get
            {

                const decimal pi = (decimal)Math.PI;
                yield return new TestCaseData(pi, "G", "3.14159265358979");
                yield return new TestCaseData(pi, "G7", "3.141593");
                yield return new TestCaseData(pi, "G16", "3.14159265358979");
                yield return new TestCaseData(pi, "C", "$3.14");
                yield return new TestCaseData(pi, "C7", "$3.1415927");
                yield return new TestCaseData(pi, "C16", "$3.1415926535897900");
                yield return new TestCaseData(pi, "E", "3.141593E+000");
                yield return new TestCaseData(pi, "E7", "3.1415927E+000");
                yield return new TestCaseData(pi, "E16", "3.1415926535897900E+000");
                yield return new TestCaseData(pi, "F", "3.142");
                yield return new TestCaseData(pi, "F7", "3.1415927");
                yield return new TestCaseData(pi, "F16", "3.1415926535897900");


                yield return new TestCaseData(decimal.MaxValue, "G", "79228162514264337593543950335");
                yield return new TestCaseData(decimal.MaxValue, "G7", "7.922816E+28");
                yield return new TestCaseData(decimal.MaxValue, "G16", "7.922816251426434E+28");
                yield return new TestCaseData(decimal.MaxValue, "C", "$79,228,162,514,264,337,593,543,950,335.00");
                yield return new TestCaseData(decimal.MaxValue, "C7", "$79,228,162,514,264,337,593,543,950,335.0000000");
                yield return new TestCaseData(decimal.MaxValue, "C16", "$79,228,162,514,264,337,593,543,950,335.0000000000000000");
                yield return new TestCaseData(decimal.MaxValue, "E", "7.922816E+028");
                yield return new TestCaseData(decimal.MaxValue, "E7", "7.9228163E+028");
                yield return new TestCaseData(decimal.MaxValue, "E16", "7.9228162514264338E+028");
                yield return new TestCaseData(decimal.MaxValue, "F", "79228162514264337593543950335.000");
                yield return new TestCaseData(decimal.MaxValue, "F7", "79228162514264337593543950335.0000000");
                yield return new TestCaseData(decimal.MaxValue, "F16", "79228162514264337593543950335.0000000000000000");

                yield return new TestCaseData(decimal.MinValue, "G", "-79228162514264337593543950335");
                yield return new TestCaseData(decimal.MinValue, "G7", "-7.922816E+28");
                yield return new TestCaseData(decimal.MinValue, "G16", "-7.922816251426434E+28");
                yield return new TestCaseData(decimal.MinValue, "C", "-$79,228,162,514,264,337,593,543,950,335.00");
                yield return new TestCaseData(decimal.MinValue, "C7", "-$79,228,162,514,264,337,593,543,950,335.0000000");
                yield return new TestCaseData(decimal.MinValue, "C16", "-$79,228,162,514,264,337,593,543,950,335.0000000000000000");
                yield return new TestCaseData(decimal.MinValue, "E", "-7.922816E+028");
                yield return new TestCaseData(decimal.MinValue, "E7", "-7.9228163E+028");
                yield return new TestCaseData(decimal.MinValue, "E16", "-7.9228162514264338E+028");
                yield return new TestCaseData(decimal.MinValue, "F", "-79228162514264337593543950335.000");
                yield return new TestCaseData(decimal.MinValue, "F7", "-79228162514264337593543950335.0000000");
                yield return new TestCaseData(decimal.MinValue, "F16", "-79228162514264337593543950335.0000000000000000");

            }
        }
    }

    [TestFixture]
    public class TryFormatSanityChecks

    {

        private CultureInfo _formatInfo = CultureInfo.InvariantCulture;
        [SetUp]
        public void SetUp()
        {
            _formatInfo = new CultureInfo("en-US", false)
            {
                // This ensures compatibility between default values provided by different runtimes
                NumberFormat =
                {
                    NumberDecimalDigits = 3,
                    CurrencyNegativePattern = 1
                }
            };
            //CultureInfo.CurrentCulture = _formatInfo;
            //CultureInfo.CurrentUICulture = _formatInfo;
            //CultureInfo.DefaultThreadCurrentCulture = _formatInfo;
            //CultureInfo.DefaultThreadCurrentUICulture = _formatInfo;
        }

        [Test]
        [Parallelizable(ParallelScope.All)]
        [TestCaseSource(typeof(Int32TryFormatSource), nameof(Int32TryFormatSource.TryFormatData))]
        public void Test_TryFormatInt32_Fmt(int value, string format)
        {
            Span<char> buff = stackalloc char[32];
            var wasFormatted = value.TryFormat<int>(buff, out var charsWritten, format.AsSpan(), _formatInfo.NumberFormat);
            Assert.IsTrue(wasFormatted);

            var str = buff.Slice(0, charsWritten).ToString();
            Assert.AreEqual(value.ToString(format, _formatInfo.NumberFormat), str);
        }

        [Test]
        [Parallelizable(ParallelScope.All)]
        [TestCaseSource(typeof(UInt32TryFormatSource), nameof(UInt32TryFormatSource.TryFormatData))]
        public void Test_TryFormatUInt32_Fmt(uint value, string format)
        {
            Span<char> buff = stackalloc char[32];
            var wasFormatted = value.TryFormat<uint>(buff, out var charsWritten, format.AsSpan(), _formatInfo.NumberFormat);
            Assert.IsTrue(wasFormatted);

            var str = buff.Slice(0, charsWritten).ToString();
            Assert.AreEqual(value.ToString(format, _formatInfo.NumberFormat), str);
        }

        [Test]
        [Parallelizable(ParallelScope.All)]
        [TestCaseSource(typeof(Int64TryFormatSource), nameof(Int64TryFormatSource.TryFormatData))]
        public void Test_TryFormatInt64_Fmt(long value, string format)
        {
            Span<char> buff = stackalloc char[64];
            var wasFormatted = value.TryFormat<long>(buff, out var charsWritten, format.AsSpan(), _formatInfo.NumberFormat);
            Assert.IsTrue(wasFormatted);

            var str = buff.Slice(0, charsWritten).ToString();
            Assert.AreEqual(value.ToString(format, _formatInfo.NumberFormat), str);
        }


        [Test]
        [Parallelizable(ParallelScope.All)]
        [TestCaseSource(typeof(UInt64TryFormatSource), nameof(UInt64TryFormatSource.TryFormatData))]
        public void Test_TryFormatUInt64_Fmt(ulong value, string format)
        {
            Span<char> buff = stackalloc char[64];
            var wasFormatted = value.TryFormat<ulong>(buff, out var charsWritten, format.AsSpan(), _formatInfo.NumberFormat);
            Assert.IsTrue(wasFormatted);

            var str = buff.Slice(0, charsWritten).ToString();
            Assert.AreEqual(value.ToString(format, _formatInfo.NumberFormat), str);
        }

        [Test]
        [Parallelizable(ParallelScope.All)]
        [TestCaseSource(typeof(SingleTryFormatSource), nameof(SingleTryFormatSource.TryFormatData))]
        public void Test_TryFormatSingle_Fmt(float value, string format)
        {
            Span<char> buff = stackalloc char[64];
            var wasFormatted = value.TryFormat<float>(buff, out var charsWritten, format.AsSpan(), _formatInfo.NumberFormat);
            Assert.IsTrue(wasFormatted);

            var str = buff.Slice(0, charsWritten).ToString();
            var expStr = value.ToString(format, _formatInfo.NumberFormat);
            Assert.AreEqual(expStr, str);
        }

        [Test]
        [Parallelizable(ParallelScope.All)]
        [TestCaseSource(typeof(SingleTryFormatSource), nameof(SingleTryFormatSource.TryFormatSpecialCaseData))]
        public void Test_TryFormatSingle_Fmt(float value, string format, string expected)
        {
            Span<char> buff = stackalloc char[128];
            var wasFormatted = value.TryFormat<float>(buff, out var charsWritten, format.AsSpan(), _formatInfo.NumberFormat);
            Assert.IsTrue(wasFormatted);

            var str = buff.Slice(0, charsWritten).ToString();
            Assert.AreEqual(expected, str);
        }


        [Test]
        [Parallelizable(ParallelScope.All)]
        [TestCaseSource(typeof(DoubleTryFormatSource), nameof(DoubleTryFormatSource.TryFormatData))]
        public void Test_TryFormatDouble_Fmt(double value, string format)
        {
            Span<char> buff = stackalloc char[384];
            var wasFormatted = value.TryFormat<double>(buff, out var charsWritten, format.AsSpan(), _formatInfo.NumberFormat);
            Assert.IsTrue(wasFormatted);

            var str = buff.Slice(0, charsWritten).ToString();
            var expStr = value.ToString(format, _formatInfo.NumberFormat);
            Assert.AreEqual(expStr, str);
        }

        [Test]
        [Parallelizable(ParallelScope.All)]
        [TestCaseSource(typeof(DoubleTryFormatSource), nameof(DoubleTryFormatSource.TryFormatSpecialCaseData))]
        public void Test_TryFormatDouble_Fmt(double value, string format, string expected)
        {
            Span<char> buff = stackalloc char[128];
            var wasFormatted = value.TryFormat<double>(buff, out var charsWritten, format.AsSpan(), _formatInfo.NumberFormat);
            Assert.IsTrue(wasFormatted);

            var str = buff.Slice(0, charsWritten).ToString();
            Assert.AreEqual(expected, str);
        }

        [Test]
        [Parallelizable(ParallelScope.All)]
        [TestCaseSource(typeof(DecimalTryFormatSource), nameof(DecimalTryFormatSource.TryFormatData))]
        public void Test_TryFormatDecimal_Fmt_Culture(decimal value, string format, IFormatProvider culture)
        {
            Span<char> buff = stackalloc char[384];
            var wasFormatted = value.TryFormat<decimal>(buff, out var charsWritten, format.AsSpan(), culture);
            Assert.IsTrue(wasFormatted);

            var str = buff.Slice(0, charsWritten).ToString();
            var expStr = value.ToString(format, culture);
            Assert.AreEqual(expStr, str);
        }

        [Test]
        [Parallelizable(ParallelScope.All)]
        [TestCaseSource(typeof(DecimalTryFormatSource), nameof(DecimalTryFormatSource.TryFormatSpecialCaseData))]
        public void Test_TryFormatDecimal_Fmt(decimal value, string format, string expected)
        {
            Span<char> buff = stackalloc char[128];
            var wasFormatted = value.TryFormat<decimal>(buff, out var charsWritten, format.AsSpan(), _formatInfo.NumberFormat);
            Assert.IsTrue(wasFormatted);

            var str = buff.Slice(0, charsWritten).ToString();
            Assert.AreEqual(expected, str);
        }

    }
}
