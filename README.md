# Backports
Backporting some Span APIs to .NetStd2.0

*This repository contains code from [.NET runtime](https://github.com/dotnet/runtime) licensed by .NET Foundation under MIT.*

# Why backport APIs?
.NET Standard 2.1, .NET Core 3.x, 5.0 provide a rich set of APIs that naturally work with Spans. Some of these APIs are available in .NET Standard 2.0, and therefore can be used from .NET 4.8 projects (aka legacy). 

However, there are very useful APIs that are unavailable in early versions of .NET Standard, including `<type>.Parse/TryParse`. These static methods allow number parsing directly from `ReadOnlySpan<char>`, meaning numbers can be parsed from a heap array of `char`s, a regular `string`, or even from `stackalloc`ed/borrowed buffer.

The goal of this project is to extract part of .NET libraries available in .NET 5.0 and implement them using tools available in .NET Standard 2.0.
With no tools to extend existing types with static methods, the best solution is to introduce a new set of simple functions.
`bool Numbers.TryParse<T>(ReadOnlySpan<char> input, out T value)` attempts to parse `T` from `char`  using default for a given `T` settings. `T` is naturally limited to `unmanaged` and method throws for non-primitive types.
This (and other similar) generic utilizes `JIT` capability of eliminating unused branches when invoking with a concrete `T`.
The method is compiled differently depending on the target framework. For .NET Standard 2.1 it defaults to BCL's implementation, while for .NET Standard 2.0 it uses brrowed code fragments to mimick .NET 5+ behavior.

The goal is to achieve performance at least as good as native `string`-based methods and eliminate unnecessary allocations of e.g. `string.Substring()` with the help of `Span.Slice` when parsing from large text blocks.
