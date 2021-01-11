using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backports;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class TryFormatDateTimes
    {
        [Test]
        public void Test_SimpleDateTime()
        {
            Span<char> buff = stackalloc char[128];
            var now = DateTime.Now;

            Assert.IsTrue(now.TryFormat(buff, out var nChar, "O".AsSpan()));
            Assert.AreEqual(buff.Slice(0, nChar).ToString(), now.ToString("O"));
        }
    }
}
