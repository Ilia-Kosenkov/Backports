using System;
using System.Globalization;
using Backports;
using NUnit.Framework;

namespace Tests
{
    public class TryParseSanityChecks
    {
      

        [Test]
        public void Test_TryParseInt8_Hex()
        {
            sbyte numRaw = 123;
            var num = numRaw.ToString("X");

            Assert.IsTrue(num.AsSpan().TryParse(NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo, out sbyte value));
            Assert.AreEqual(value, numRaw);
        }

        [Test]
        public void Test_TryParseUInt8_Hex()
        {
            byte numRaw = 123;
            var num = numRaw.ToString("X");

            Assert.IsTrue(num.AsSpan().TryParse(NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo, out byte value));
            Assert.AreEqual(value, numRaw);
        }

        [Test]
        public void Test_TryParseFloat_Scientific()
        {
            var numRaw = (float) Math.PI;
            var num = numRaw.ToString("E7");

            Assert.IsTrue(num.AsSpan().TryParse(NumberStyles.Any, NumberFormatInfo.InvariantInfo, out float value));
            Assert.AreEqual(value, numRaw);
        }

        [Test]
        public void Test_TryParseDouble_Scientific()
        {
            var numRaw = Math.PI;
            var num = numRaw.ToString("E16");

            Assert.IsTrue(num.AsSpan().TryParse(NumberStyles.Any, NumberFormatInfo.InvariantInfo, out double value));
            Assert.AreEqual(value, numRaw);
        }

        [Test]
        public void Test_TryParseInt8()
        {
            sbyte numRaw = 123;
            var num = numRaw.ToString();

            Assert.IsTrue(num.AsSpan().TryParse(out sbyte value));
            Assert.AreEqual(value, numRaw);
        }

        [Test]
        public void Test_TryParseUInt8()
        {
            byte numRaw = 123;
            var num = numRaw.ToString();

            Assert.IsTrue(num.AsSpan().TryParse(out byte value));
            Assert.AreEqual(value, numRaw);
        }

        [Test]
        public void Test_TryParseInt16()
        {
            short numRaw = 12345;
            var num = numRaw.ToString();

            Assert.IsTrue(num.AsSpan().TryParse(out short value));
            Assert.AreEqual(value, numRaw);
        }

        [Test]
        public void Test_TryParseUInt16()
        {
            ushort numRaw = 12345;
            var num = numRaw.ToString();

            Assert.IsTrue(num.AsSpan().TryParse(out ushort value));
            Assert.AreEqual(value, numRaw);
        }

        [Test]
        public void Test_TryParseInt32()
        {
            var numRaw = 123456;
            var num = numRaw.ToString();

            Assert.IsTrue(num.AsSpan().TryParse(out int value));
            Assert.AreEqual(value, numRaw);
        }

        [Test]
        public void Test_TryParseUInt32()
        {
            var numRaw = 123456U;
            var num = numRaw.ToString();

            Assert.IsTrue(num.AsSpan().TryParse(out uint value));
            Assert.AreEqual(value, numRaw);
        }

        [Test]
        public void Test_TryParseInt64()
        {
            var numRaw = 123456L;
            var num = numRaw.ToString();

            Assert.IsTrue(num.AsSpan().TryParse(out long value));
            Assert.AreEqual(value, numRaw);
        }

        [Test]
        public void Test_TryParseUInt64()
        {
            var numRaw = 123456UL;
            var num = numRaw.ToString();

            Assert.IsTrue(num.AsSpan().TryParse(out ulong value));
            Assert.AreEqual(value, numRaw);
        }


        [Test]
        public void Test_TryParseSingle()
        {
            var numRaw = (float)Math.PI;
            var num = numRaw.ToString("R");
            Assert.IsTrue(num.AsSpan().TryParse(out float value));
            Assert.AreEqual(numRaw, value);
        }

        [Test]
        public void Test_TryParseDouble()
        {
            var numRaw = Math.PI;
            var num = numRaw.ToString("R");
            Assert.IsTrue(num.AsSpan().TryParse(out double value));
            Assert.AreEqual(numRaw, value);
        }

        [Test]
        public void Test_TryParseDecimal()
        {
            var numRaw = decimal.MaxValue;
            var num = numRaw.ToString(CultureInfo.InvariantCulture);
            Assert.IsTrue(num.AsSpan().TryParse(out decimal value));
            Assert.AreEqual(numRaw, value);
        }
    }
}