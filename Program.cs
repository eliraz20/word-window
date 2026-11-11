using System.Globalization;

CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
if (args.Length > 0 && args[0] == "corpus")
{
    try { CorpusReport.Run(args); return 0; }
    catch (Exception e) when (e is FormatException or IOException or ArgumentException or OverflowException or UnauthorizedAccessException)
    { Console.Error.WriteLine(e.Message); return 1; }
}

if (args.Length != 1 || args[0] == "--help")
{
    Console.WriteLine("Usage: dotnet run -- corpus texts bank 3  |  original format: <input-file|->");
    return args.Length == 1 ? 0 : 2;
}

try
{
    string[] lines = args[0] == "-" ? Console.In.ReadToEnd().ReplaceLineEndings("\n").TrimEnd('\n').Split('\n') : File.ReadAllLines(args[0]);
    Check(lines.Length >= 2 && lines.Any(line => !string.IsNullOrWhiteSpace(line)), "Input is empty or missing records.");
    var term = lines[0].Trim();
    var word = new System.Text.RegularExpressions.Regex(@"[\p{L}\p{M}]+(?:['’][\p{L}\p{M}]+)*");
    Check(word.Match(term).Value == term && term.Length > 0, "Enter one word.");
    int hits = 0;
    for (int row = 1; row < lines.Length; row++)
    {
        var words = word.Matches(lines[row]).Select(m => m.Value).ToArray();
        for (int i = 0; i < words.Length; i++)
        {
            if (!string.Equals(words[i], term, StringComparison.OrdinalIgnoreCase))
                continue;
            var left = words.Skip(Math.Max(i - 4, 0)).Take(Math.Min(i, 4));
            Console.WriteLine($"{row}: {string.Join(" ", left)} [{words[i]}] {string.Join(" ", words.Skip(i + 1).Take(4))}");
            hits++;
        }
    }

    Console.WriteLine($"{hits} matches");
    return 0;
}
catch (Exception e)when (e is IOException or FormatException or OverflowException or ArgumentException or UnauthorizedAccessException or DivideByZeroException)
{
    Console.Error.WriteLine(e.Message);
    return 1;
}

static void Check(bool valid, string message)
{
    if (!valid)
        throw new FormatException(message);
}
