using System.Text.RegularExpressions;

var input = File.ReadAllLines("Input.txt");

var matches = new List<string>();

foreach (var line in input)
{
    //matches.AddRange(Regex.Matches(line, "mul\\(\\d*,\\d*\\)").Select(m => m.Value));
    matches.AddRange(Regex.Matches(line, "mul\\(\\d*,\\d*\\)|do\\(\\)|don\'t\\(\\)").Select(m => m.Value));
}

var total = 0;
var ignore = false;

foreach (var match in matches)
{
    if (match == "don't()")
    {
        ignore = true;
    }

    if (match == "do()")
    {
        ignore = false;
    }

    if (!ignore && match.StartsWith("mul"))
    {
        var parts = match.Replace("mul(", string.Empty).Replace(")", string.Empty).Split(',').Select(s => int.Parse(s)).ToArray();
        var result = parts[0] * parts[1];

        Console.WriteLine($"{match} = {result}");

        total += result;
    }
}

Console.WriteLine($"Total = {total}");