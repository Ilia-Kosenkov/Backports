using System;
using Backports;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class TryFormatSanityChecks
    {
        [Test]
        [TestCase(int.MaxValue)]
        [TestCase(int.MinValue)]
        [TestCase(0)]
        [TestCase(42)]
        public void Test_TryFormatInt32(int value)
        {
            Span<char> buff = stackalloc char[32];
            var wasFormatted = value.TryFormat(buff, out var charsWritten);
            Assert.IsTrue(wasFormatted);

            var str = buff.Slice(0, charsWritten).ToString();
            Assert.AreEqual(value.ToString(), str);
            
        }
    }
}
