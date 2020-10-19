using System;
using Backports;
using NUnit.Framework;

namespace Tests
{
    public class SimpleTests
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void Test_TryParseInt32()
        {
            var num = "123456".AsSpan();

            Assert.IsTrue(num.TryParseInto(out int value));
            Assert.AreEqual(value, 123456);
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