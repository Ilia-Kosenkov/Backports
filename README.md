![Build & Test](https://github.com/Ilia-Kosenkov/Backports/workflows/Build%20&%20Test/badge.svg?branch=master)

# Backports
Backporting some Span APIs to .NET Standard2.0

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

# Getting rid of native pointers
Original code heavily relies on native pointers and unsafe context. While `ref` structs (like `Span`) have been introduced, they are sometimes underused and are immediately converted into native pointers within `fixed` statement. Stack allocations also occur unsafely, and reuslting pointer together with the size of hte allocated block is passed around. 
This issues can be (at least partially) eliminated using managed pointers and corresponding `Unsafe` operations. In some cases this may yield performance improvements (though unlikely in case of this port), because elimination of pinned memory chunks (within `fixed`) helps GC to deal with the memory.
Managed pointers come at a price. There is no (safe) notion of pointer to a pointer (to a pointer), which can be required sometimes.
A common use case of native pointers is iterating over an array and perfoming assignments in a form of
```csharp
byte* p = GetPtr();
while(true)
    *(++p) = 0;
```
In the managed pointer world, this results into weird constructs, similar to the following one
```csharp
ref var p = ref GetRef();
while(true)
    (p = ref Increment(ref p)) = 0;
/// ....
static ref T Increment<T>(ref T value) where T : unmanaged => ref Unsafe.Add(ref value, 1);
```
At the same time, `*(p++)` has no one-line equivalent in managed pointers, and transforms into two assignments.

Overall, using C# 7+ feature improves the readability and clarity of the code. Certain Asserts that check incoming pointers against `null` are no longer needed, as `ref T` should not be `null` in safe context (though, `unsafe {ref byte p = ref Unsafe.As<byte>(null);}` is possible).

# Incorrect formatting
Legacy framework exhibits substantially less elegant methods of numbers formatting. As a result, ported from .NET Core implementation does not agree with the results of legacy framework's `.ToString(fmt)` methods. Thus, using backported methods can not only save one-two string allocations, but also produce correct results when formatting numbers.
