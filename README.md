# Testing Private Methods: The Right Way

A demonstration project showing how to properly test private methods in C# — by **not** testing them directly.

## The Key Takeaway

> **Private methods should be tested indirectly through the public API.** This preserves encapsulation while ensuring full coverage.

---

## What is a "Unit" in Unit Testing?

A common misconception: *"A unit is a single method, class, or line of code."*

**The reality:** A unit is the **minimal reasonable business behavior** that can be tested. This typically means the **public API** — the contract your code exposes to consumers.

Testing every private method directly leads to:
- Brittle tests that break when you refactor
- Tests coupled to implementation details
- Broken encapsulation (using reflection or `internal` visibility hacks)

Instead, test the **observable behavior** through public methods.

---

## The Pattern in Practice

### The Class Structure

`TextAnalyzer` has **1 public method** and **6 private methods**:

```csharp
public static class TextAnalyzer
{
    // PUBLIC: The only method consumers can call
    public static TextAnalysisResult Analyze(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return TextAnalysisResult.Empty;

        var words = ExtractWords(text);

        return new TextAnalysisResult
        {
            WordCount = CountWords(words),
            SentenceCount = CountSentences(text),
            AverageWordLength = CalculateAverageWordLength(words),
            LongestWord = FindLongestWord(words)
        };
    }

    // PRIVATE: Implementation details hidden from tests
    private static string[] ExtractWords(string text) { ... }
    private static string CleanWord(string word) { ... }
    private static int CountWords(string[] words) { ... }
    private static int CountSentences(string text) { ... }
    private static double CalculateAverageWordLength(string[] words) { ... }
    private static string FindLongestWord(string[] words) { ... }
}
```

### How Tests Work

All 23 tests call **only** `Analyze()` and verify the results. No reflection. No `[InternalsVisibleTo]`. No hacks.

```csharp
// Testing CleanWord() indirectly:
// If CleanWord() works, "Hello," becomes "Hello" (5 chars) and "world!" becomes "world" (5 chars)
// Average = (5 + 5) / 2 = 5.0
[Test]
public void Analyze_CleansPunctuationFromWords()
{
    var result = TextAnalyzer.Analyze("Hello, world!");
    Assert.That(result.AverageWordLength, Is.EqualTo(5.0));
}

// Testing ExtractWords() with various delimiters:
[Test]
public void Analyze_HandlesTabsAndNewlines()
{
    var result = TextAnalyzer.Analyze("Word1\tWord2\nWord3");
    Assert.That(result.WordCount, Is.EqualTo(3));
}

// Testing FindLongestWord() indirectly:
[Test]
public void Analyze_FindsLongestWord()
{
    var result = TextAnalyzer.Analyze("The quick brown fox");
    Assert.That(result.LongestWord, Is.EqualTo("quick"));
}
```

---

## How Each Private Method Gets Tested

| Private Method | Tested By Verifying |
|----------------|---------------------|
| `ExtractWords()` | `WordCount` with spaces, tabs, newlines |
| `CleanWord()` | `AverageWordLength` with punctuated input |
| `CountWords()` | `WordCount` property across various inputs |
| `CountSentences()` | `SentenceCount` with `.`, `!`, `?` delimiters |
| `CalculateAverageWordLength()` | `AverageWordLength` property |
| `FindLongestWord()` | `LongestWord` property |

**Result:** 100% private method coverage through 23 public API tests.

---

## Benefits

- **Encapsulation preserved** — Private methods stay private
- **Refactoring safety** — Change implementation without breaking tests
- **Tests as documentation** — Tests describe behavior, not implementation
- **Real-world validation** — Tests verify what users actually experience

---

## Running the Tests

```bash
# Build
dotnet build

# Run tests
dotnet test

# Run with detailed output
dotnet test --logger "console;verbosity=detailed"
```

---

## Project Structure

```
PrivateTests/
├── PrivateTests/
│   ├── TextAnalyzer.cs          # Main class (1 public + 6 private methods)
│   └── TextAnalysisResult.cs    # Result record
└── PrivateTests.Tests/
    └── TextAnalyzerTests.cs     # 23 tests via public API only
```

---

## Summary

Don't test private methods directly. Test the **public contract** and let the private methods be exercised naturally. Your tests will be more maintainable, your code will stay properly encapsulated, and you'll catch the bugs that actually matter — the ones users can observe.
