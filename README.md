# Learn C# in 7 Days — Gaurav Arora

A hands-on C# 7 (and beyond) learning project structured as a 7-day curriculum. Each day covers a set of topics with fully runnable console examples.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download) or later

## Structure

| Day | Project | Topics |
|-----|---------|--------|
| 1 | `Day01-BasicConcepts` | Variables, data types, operators, control flow, methods, string operations |
| 2 | `Day02-OOPConcepts` | Classes, encapsulation, inheritance, interfaces, polymorphism, generics |
| 3 | `Day03-AsyncAwait` | **Async Main** (C# 7.1), async/await, `Task`, `Task<T>`, `ValueTask`, cancellation, `Task.WhenAll` / `Task.WhenAny` |
| 4 | `Day04-TuplesPatternMatching` | **Named tuples** (C# 7), tuple deconstruction, `is`-pattern, switch-pattern, `when` clauses, switch expressions (C# 8) |
| 5 | `Day05-LINQ` | `Where`, `Select`, `OrderBy`, `GroupBy`, aggregation, joins, set operations, deferred execution, query syntax |
| 6 | `Day06-RegexIndexers` | **Indexers** (1D, 2D, string-key), regular expressions (groups, named groups, lookahead/lookbehind, replace, split) |
| 7 | `Day07-MoreFeatures` | **Local functions** (C# 7), **ref locals/returns** (C# 7), **out variables** (C# 7), throw expressions (C# 7), delegates, events, lambdas, expression-bodied members, numeric/binary literals (C# 7) |

## Running the Examples

### Run a single day

```bash
dotnet run --project Day01/Day01-BasicConcepts/Day01-BasicConcepts.csproj
dotnet run --project Day02/Day02-OOPConcepts/Day02-OOPConcepts.csproj
dotnet run --project Day03/Day03-AsyncAwait/Day03-AsyncAwait.csproj
dotnet run --project Day04/Day04-TuplesPatternMatching/Day04-TuplesPatternMatching.csproj
dotnet run --project Day05/Day05-LINQ/Day05-LINQ.csproj
dotnet run --project Day06/Day06-RegexIndexers/Day06-RegexIndexers.csproj
dotnet run --project Day07/Day07-MoreFeatures/Day07-MoreFeatures.csproj
```

### Build all

```bash
dotnet build LearnCSharpIn7Days.slnx
```

## Key C# 7 Features Covered

- **Async Main** — `static async Task Main(string[] args)` (C# 7.1)
- **Tuples** — named tuples `(string Name, int Age)`, deconstruction, `Deconstruct` method
- **Pattern matching** — `is` type patterns, `switch` with type patterns and `when` guards
- **Local functions** — functions defined inside other functions/methods
- **`out` variable declarations** — `int.TryParse("42", out int x)`
- **`ref` locals and returns** — return a reference to a variable
- **Throw expressions** — `value ?? throw new ArgumentNullException()`
- **Digit separators** — `1_000_000`, `3.141_592`
- **Binary literals** — `0b1010_1010`
- **Expression-bodied members** — constructors, destructors, properties, methods

