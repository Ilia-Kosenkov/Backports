using System;
using Backports;
using NUnit.Framework;

namespace Tests
{
    public class SanityChecks
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void Test_TryParseInt8()
        {
            sbyte numRaw = 123;
            var num = numRaw.ToString();

            Assert.IsTrue(num.AsSpan().TryParseInto(out sbyte value));
            Assert.AreEqual(value, numRaw);
        }

        [Test]
        public void Test_TryParseUInt8()
        {
            byte numRaw = 123;
            var num = numRaw.ToString();

            Assert.IsTrue(num.AsSpan().TryParseInto(out byte value));
            Assert.AreEqual(value, numRaw);
        }

        [Test]
        public void Test_TryParseInt16()
        {
            short numRaw = 12345;
            var num = numRaw.ToString();

            Assert.IsTrue(num.AsSpan().TryParseInto(out short value));
            Assert.AreEqual(value, numRaw);
        }

        [Test]
        public void Test_TryParseUInt16()
        {
            ushort numRaw = 12345;
            var num = numRaw.ToString();

            Assert.IsTrue(num.AsSpan().TryParseInto(out ushort value));
            Assert.AreEqual(value, numRaw);
        }

        [Test]
        public void Test_TryParseInt32()
        {
            var numRaw = 123456;
            var num = numRaw.ToString();

            Assert.IsTrue(num.AsSpan().TryParseInto(out int value));
            Assert.AreEqual(value, numRaw);
        }

        [Test]
        public void Test_TryParseUInt32()
        {
            var numRaw = 123456u;
            var num = numRaw.ToString();

            Assert.IsTrue(num.AsSpan().TryParseInto(out uint value));
            Assert.AreEqual(value, numRaw);
        }

        [Test]
        public void Test_TryParseInt64()
        {
            var numRaw = 123456l;
            var num = numRaw.ToString();

            Assert.IsTrue(num.AsSpan().TryParseInto(out long value));
            Assert.AreEqual(value, numRaw);
        }

        [Test]
        public void Test_TryParseUInt64()
        {
            var numRaw = 123456ul;
            var num = numRaw.ToString();

            Assert.IsTrue(num.AsSpan().TryParseInto(out ulong value));
            Assert.AreEqual(value, numRaw);
        }


        [Test]
        public void Test_TryParseSingle()
        {
            var numRaw = (float)Math.PI;
            var num = numRaw.ToString("R");
            Assert.IsTrue(num.AsSpan().TryParseInto(out float value));
            Assert.AreEqual(numRaw, value);
        }

        [Test]
        public void Test_TryParseDouble()
        {
            var numRaw = Math.PI;
            var num = numRaw.ToString("R");
            Assert.IsTrue(num.AsSpan().TryParseInto(out double value));
            Assert.AreEqual(numRaw, value);
        }
    }
}