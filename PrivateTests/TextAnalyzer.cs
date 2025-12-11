namespace PrivateTests;

public static class TextAnalyzer
{
    private static readonly char[] SentenceDelimiters = ['.', '!', '?'];
    private static readonly char[] WordDelimiters = [' ', '\t', '\n', '\r'];

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

    private static string[] ExtractWords(string text)
    {
        return text
            .Split(WordDelimiters, StringSplitOptions.RemoveEmptyEntries)
            .Select(CleanWord)
            .Where(w => !string.IsNullOrEmpty(w))
            .ToArray();
    }

    private static string CleanWord(string word)
    {
        return new string(word.Where(char.IsLetterOrDigit).ToArray());
    }

    private static int CountWords(string[] words)
    {
        return words.Length;
    }

    private static int CountSentences(string text)
    {
        return text.Split(SentenceDelimiters, StringSplitOptions.RemoveEmptyEntries).Length;
    }

    private static double CalculateAverageWordLength(string[] words)
    {
        return words.Length == 0 ? 0 : words.Average(w => w.Length);
    }

    private static string FindLongestWord(string[] words)
    {
        return words.Length == 0 ? string.Empty : words.OrderByDescending(w => w.Length).First();
    }
}