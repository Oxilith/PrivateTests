namespace PrivateTests.Tests;

public class TextAnalyzerTests
{
    [Test]
    public void Analyze_WithNullText_ReturnsEmptyResult()
    {
        var result = TextAnalyzer.Analyze(null!);

        Assert.Multiple(() =>
        {
            Assert.That(result.WordCount, Is.EqualTo(0));
            Assert.That(result.SentenceCount, Is.EqualTo(0));
            Assert.That(result.AverageWordLength, Is.EqualTo(0));
            Assert.That(result.LongestWord, Is.EqualTo(string.Empty));
        });
    }

    [Test]
    public void Analyze_WithEmptyText_ReturnsEmptyResult()
    {
        var result = TextAnalyzer.Analyze(string.Empty);

        Assert.That(result, Is.EqualTo(TextAnalysisResult.Empty));
    }

    [Test]
    public void Analyze_WithWhitespaceOnly_ReturnsEmptyResult()
    {
        var result = TextAnalyzer.Analyze("   \t\n  ");

        Assert.That(result.WordCount, Is.EqualTo(0));
    }

    [Test]
    public void Analyze_WithSingleWord_ReturnsCorrectWordCount()
    {
        var result = TextAnalyzer.Analyze("Hello");

        Assert.That(result.WordCount, Is.EqualTo(1));
    }

    [Test]
    public void Analyze_WithMultipleWords_ReturnsCorrectWordCount()
    {
        var result = TextAnalyzer.Analyze("The quick brown fox");

        Assert.That(result.WordCount, Is.EqualTo(4));
    }

    [Test]
    public void Analyze_WithWordsAndPunctuation_CountsWordsCorrectly()
    {
        var result = TextAnalyzer.Analyze("Hello, world! How are you?");

        Assert.That(result.WordCount, Is.EqualTo(5));
    }

    [Test]
    public void Analyze_WithSingleSentence_ReturnsCorrectSentenceCount()
    {
        var result = TextAnalyzer.Analyze("This is a sentence.");

        Assert.That(result.SentenceCount, Is.EqualTo(1));
    }

    [Test]
    public void Analyze_WithMultipleSentences_ReturnsCorrectSentenceCount()
    {
        var result = TextAnalyzer.Analyze("First sentence. Second sentence! Third sentence?");

        Assert.That(result.SentenceCount, Is.EqualTo(3));
    }

    [Test]
    public void Analyze_WithMixedDelimiters_CountsSentencesCorrectly()
    {
        var result = TextAnalyzer.Analyze("Really? Yes! I agree.");

        Assert.That(result.SentenceCount, Is.EqualTo(3));
    }

    [Test]
    public void Analyze_CalculatesAverageWordLength()
    {
        var result = TextAnalyzer.Analyze("cat dog");

        Assert.That(result.AverageWordLength, Is.EqualTo(3.0));
    }

    [Test]
    public void Analyze_CalculatesAverageWordLengthWithVaryingLengths()
    {
        var result = TextAnalyzer.Analyze("I am here");

        Assert.That(result.AverageWordLength, Is.EqualTo(7.0 / 3.0).Within(0.01));
    }

    [Test]
    public void Analyze_FindsLongestWord()
    {
        var result = TextAnalyzer.Analyze("The quick brown fox");

        Assert.That(result.LongestWord, Is.EqualTo("quick"));
    }

    [Test]
    public void Analyze_FindsLongestWordWithPunctuation()
    {
        var result = TextAnalyzer.Analyze("Hello, extraordinary world!");

        Assert.That(result.LongestWord, Is.EqualTo("extraordinary"));
    }

    [Test]
    public void Analyze_WithSingleWord_LongestWordIsThatWord()
    {
        var result = TextAnalyzer.Analyze("Testing");

        Assert.That(result.LongestWord, Is.EqualTo("Testing"));
    }

    [Test]
    public void Analyze_CleansPunctuationFromWords()
    {
        var result = TextAnalyzer.Analyze("Hello, world!");

        Assert.That(result.AverageWordLength, Is.EqualTo(5.0));
    }

    [Test]
    public void Analyze_HandlesTabsAndNewlines()
    {
        var result = TextAnalyzer.Analyze("Word1\tWord2\nWord3");

        Assert.That(result.WordCount, Is.EqualTo(3));
    }

    [Test]
    public void Analyze_ComplexText_ReturnsAllMetricsCorrectly()
    {
        var text = "The lazy dog sleeps. The quick fox jumps!";

        var result = TextAnalyzer.Analyze(text);

        Assert.Multiple(() =>
        {
            Assert.That(result.WordCount, Is.EqualTo(8));
            Assert.That(result.SentenceCount, Is.EqualTo(2));
            Assert.That(result.LongestWord, Is.EqualTo("sleeps"));
        });
    }

    [Test]
    public void Analyze_WithPunctuationOnly_ReturnsZeroAverageWordLength()
    {
        var result = TextAnalyzer.Analyze("!!!");

        Assert.That(result.AverageWordLength, Is.EqualTo(0));
    }

    [Test]
    public void Analyze_WithPunctuationOnly_ReturnsEmptyLongestWord()
    {
        var result = TextAnalyzer.Analyze("...");

        Assert.That(result.LongestWord, Is.EqualTo(string.Empty));
    }

    [Test]
    public void Analyze_WithMixedPunctuationOnly_ReturnsZeroWordCount()
    {
        var result = TextAnalyzer.Analyze("!!! ... ???");

        Assert.Multiple(() =>
        {
            Assert.That(result.WordCount, Is.EqualTo(0));
            Assert.That(result.AverageWordLength, Is.EqualTo(0));
            Assert.That(result.LongestWord, Is.EqualTo(string.Empty));
        });
    }

    [Test]
    public void ToFormattedString_ContainsAllValues()
    {
        var result = new TextAnalysisResult
        {
            WordCount = 5,
            SentenceCount = 2,
            AverageWordLength = 4.5,
            LongestWord = "hello"
        };

        var formatted = result.ToFormattedString();

        Assert.Multiple(() =>
        {
            Assert.That(formatted, Does.Contain("Word Count: 5"));
            Assert.That(formatted, Does.Contain("Sentence Count: 2"));
            Assert.That(formatted, Does.Match(@"Average Word Length: 4[.,]50"));
            Assert.That(formatted, Does.Contain("Longest Word: hello"));
        });
    }

    [Test]
    public void ToFormattedString_WithEmptyLongestWord_ShowsNone()
    {
        var result = TextAnalysisResult.Empty;

        var formatted = result.ToFormattedString();

        Assert.That(formatted, Does.Contain("Longest Word: (none)"));
    }

    [Test]
    public void ToFormattedString_ContainsHeaderAndFooter()
    {
        var result = TextAnalysisResult.Empty;

        var formatted = result.ToFormattedString();

        Assert.Multiple(() =>
        {
            Assert.That(formatted, Does.StartWith("=== Text Analysis Result ==="));
            Assert.That(formatted, Does.EndWith("============================"));
        });
    }
}