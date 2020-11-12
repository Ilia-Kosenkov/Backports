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
            var culture = new CultureInfo("en-US");
            var ver = Assembly.GetEntryAssembly()?
                .GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;

            Console.WriteLine($"Target Framework: {ver}");
            Console.WriteLine($"Negative currency format index: {NumberFormatInfo.GetInstance(culture).CurrencyNegativePattern}");

            foreach (var (number, format) in Decimals)
                Console.WriteLine($"{format, -6}: {number.ToString(format, culture)}");
        }

        internal static IEnumerable<decimal> DecimalNumbers { get; } = new[] {decimal.MaxValue, decimal.MinValue, 0};
        internal static IEnumerable<string> DecimalFormats { get; } = new[] {"C", "P"};

        internal static IEnumerable<(decimal, string)> Decimals =>
            from x in DecimalNumbers
            from y in DecimalFormats
            select (x, y);


    }
}
