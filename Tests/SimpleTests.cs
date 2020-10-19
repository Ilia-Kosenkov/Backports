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
        public void Test1()
        {
            var num = "123456".AsSpan();

            Assert.IsTrue(num.TryParseInto(out int value));
            Assert.AreEqual(value, 123456);

        }
    }
}