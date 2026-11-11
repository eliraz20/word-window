static class CorpusReport
{
    public static void Run(string[] args)
    {
        if (args.Length != 4)
            throw new FormatException("Usage: corpus texts bank 3");
        int window = checked((int)Integer(args[3]));
        var word = new System.Text.RegularExpressions.Regex(@"[\p{L}\p{M}]+(?:['’][\p{L}\p{M}]+)*");
        Require(word.Match(args[2]).Value == args[2] && args[2].Length > 0, "Enter one keyword.");
        var paths = Directory.GetFiles(args[1], "*.txt", SearchOption.AllDirectories).Order(StringComparer.Ordinal).ToArray();
        Require(paths.Length > 0, "Folder contains no .txt files.");
        static string Csv(string s) => "\"" + s.Replace("\"", "\"\"") + "\"";
        Console.WriteLine("document,line,column,left,keyword,right");
        foreach (var file in paths)
        {
            int row = 0;
            foreach (var line in File.ReadLines(file))
            {
                row++;
                var tokens = word.Matches(line).ToArray();
                for (int i = 0; i < tokens.Length; i++)
                {
                    var hit = tokens[i];
                    if (!string.Equals(hit.Value, args[2], StringComparison.OrdinalIgnoreCase))
                        continue;
                    int lo = window == 0 ? hit.Index : i <= window ? 0 : tokens[i - window].Index;
                    int hi = window == 0 ? hit.Index + hit.Length : window >= tokens.Length - 1 - i ? line.Length : tokens[i + window + 1].Index;
                    Console.WriteLine($"{Csv(Path.GetRelativePath(args[1], file))},{row},{hit.Index + 1},{Csv(line[lo..hit.Index])},{Csv(hit.Value)},{Csv(line[(hit.Index + hit.Length)..hi])}");
                }
            }
        }
    }

    static void Require(bool condition, string message)
    {
        if (!condition)
            throw new FormatException(message);
    }

    static long Integer(string text)
    {
        var value = long.Parse(text);
        Require(value >= 0, "Count must be nonnegative.");
        return value;
    }
}
