using System.Text;

namespace PrivateTests;

public record TextAnalysisResult
{
    public int WordCount { get; init; }
    public int SentenceCount { get; init; }
    public double AverageWordLength { get; init; }
    public string LongestWord { get; init; } = string.Empty;

    public static TextAnalysisResult Empty => new()
    {
        WordCount = 0,
        SentenceCount = 0,
        AverageWordLength = 0,
        LongestWord = string.Empty
    };

    public string ToFormattedString()
    {
        var builder = new StringBuilder();

        builder.AppendLine("=== Text Analysis Result ===");
        builder.AppendLine($"Word Count: {WordCount}");
        builder.AppendLine($"Average Word Length: {AverageWordLength:F2}");
        builder.AppendLine($"Sentence Count: {SentenceCount}");
        builder.AppendLine($"Longest Word: {(string.IsNullOrEmpty(LongestWord) ? "(none)" : LongestWord)}");
        builder.Append("============================");

        return builder.ToString();
    }
}