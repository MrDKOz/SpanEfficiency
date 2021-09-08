# Span Efficiency
Span is both a type and memory safe way of accessing a sequential region of memory.

Further reading about Span can be found on the [Microsoft documentation site](https://docs.microsoft.com/en-us/dotnet/api/system.span-1?view=net-5.0).

# What is this Project?
This project hopes to serve as a simple way of demonstrating, and showing the benefits of using span, in this case as an alternative to using
"Substring" to [Country Codes](https://en.wikipedia.org/wiki/List_of_country_calling_codes) from a given string e.g. "OSBO44".

# How do I run it?
1. Clone the repository
2. Restore the project (this will download the NuGet package [Benchmark Dotnet](https://www.nuget.org/packages/BenchmarkDotNet/))
3. Run it and you'll be given 3 options:
   1. "Run all functions" - This will run all the functions that will be benchmarked and output their results.
   2. "Run Benchmark" - This will benchmark all functions and provide you with the results after it's finished.
   3. "Exit" - This exits the application.

# The Results
|                               Method |     Mean |    Error |   StdDev |  Gen 0 | Allocated |
|------------------------------------- |---------:|---------:|---------:|-------:|----------:|
|   FetchAllCountryCodesUsingSubstring | 84.32 ns | 1.701 ns | 2.698 ns | 0.0331 |     208 B |
|        FetchAllCountryCodesUsingSpan | 49.36 ns | 1.018 ns | 1.555 ns | 0.0076 |      48 B |
|      FetchSingleCountryCodeUsingSpan | 11.32 ns | 0.247 ns | 0.362 ns |      - |         - |
| FetchSingleCountryCodeUsingSubstring | 16.21 ns | 0.343 ns | 0.644 ns | 0.0051 |      32 B |
