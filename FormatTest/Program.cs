using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;

namespace FormatTest
{
    internal class Program
    {
        private static void Main()
        {
            var culture = new CultureInfo("en-US", false);
            var ver = Assembly.GetEntryAssembly()?
                .GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;

            Console.WriteLine($"Target Framework: {ver}");
            var formatInfo = culture.NumberFormat;
            Console.WriteLine($"Positive currency format index: {formatInfo.CurrencyPositivePattern}");
            Console.WriteLine($"Negative currency format index: {formatInfo.CurrencyNegativePattern}");
            Console.WriteLine($"Positive percent format index: {formatInfo.PercentPositivePattern}");
            Console.WriteLine($"Negative percent format index: {formatInfo.PercentNegativePattern}");



            foreach (var (number, format) in Decimals)
                Console.WriteLine($"{format, -6}: {number.ToString(format, formatInfo)}");
        }

        internal static IEnumerable<decimal> DecimalNumbers { get; } = new[] {decimal.MaxValue, decimal.MinValue, 0};
        internal static IEnumerable<string> DecimalFormats { get; } = new[] {"C", "P", "F"};

        internal static IEnumerable<(decimal, string)> Decimals =>
            from x in DecimalNumbers
            from y in DecimalFormats
            select (x, y);


    }
}
