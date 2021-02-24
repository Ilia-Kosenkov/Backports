using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Backports;
using NUnit.Framework;

namespace Tests
{
    public class FloatingPointDataProvider
    {
        public static IEnumerable<string> DataFilesPaths
        {
            get
            {
                var root = Environment.GetEnvironmentVariable(@"PARSE_NUMBER_FXX_TEST_DATA_ROOT");
                if (root is null)
                {
                    throw new Exception("`PARSE_NUMBER_FXX_TEST_DATA_ROOT` environment variable is not set.");
                }
                if (!Directory.Exists(root))
                {
                    throw new Exception("Test data root folder not found.");
                }

                var data = Path.GetFullPath(Path.Combine(root, "data"));
                return Directory.EnumerateFiles(data, "*txt");
            }
        }

        public static IEnumerable<FPDataSetProvider> DataSetProviders =>
            DataFilesPaths
               .Select(x => new FPDataSetProvider(x));
    }

    [TestFixture]
    // ReSharper disable once InconsistentNaming
    public class FPDataTests
    {
        [Test]
        [Parallelizable(ParallelScope.All)]
        [TestCaseSource(typeof(FloatingPointDataProvider), nameof(FloatingPointDataProvider.DataSetProviders))]
        public async Task Test_FloatingPointParsing(FPDataSetProvider provider)
        {
            await foreach (var item in provider.ReadTestDataAsync())
            {
#if !NET40_OR_GREATER
                // Disabled for .NET Core-based runtimes
                if(string.Equals(
                    item.StrRep,
                    "6250000000000000000000000000000000e-12", 
                    StringComparison.Ordinal
                    )
                )
                {
                    // Avoid rare parsing issue
                    // https://github.com/dotnet/runtime/issues/48648
                    // https://github.com/pgovind/runtime/commit/9897a27aaace156628735acdcb06938e3a24ce15
                    continue;
                }
#endif
                Assert.IsTrue(
                    item.StrRep.AsSpan().TryParse(
                        NumberStyles.Any, NumberFormatInfo.InvariantInfo, out float single
                    ),
                    $"Cant parse single from `{item.StrRep}`"
                );
                Assert.IsTrue(
                    item.StrRep.AsSpan().TryParse(
                        NumberStyles.Any, NumberFormatInfo.InvariantInfo, out double @double
                    ),
                    $"Cant parse double from `{item.StrRep}`"
                );

                var doubleBytes = BitConverter.GetBytes(@double);
                var singleBytes = BitConverter.GetBytes(single);

                CollectionAssert.AreEqual(
                    item.DoubleBytes,
                    doubleBytes,
                    $"Failed double comparison for `{item.StrRep}`"
                    );
                CollectionAssert.AreEqual(
                    item.SingleBytes,
                    singleBytes,
                    $"Failed single comparison for `{item.StrRep}`"
                    );
            }
        }
    }
}
