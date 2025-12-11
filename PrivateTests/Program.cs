using PrivateTests;

var result = TextAnalyzer.Analyze("Hello, World!\r\nGood morning, everyone!");

Console.WriteLine(result.ToFormattedString());