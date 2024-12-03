using System.Text.RegularExpressions;

var input = File.ReadAllLines("Input.txt");

var matches = new List<string>();

foreach (var line in input)
{
    // MAy need to check > 4 digit numbers.
    matches.AddRange(Regex.Matches(line, "mul\\(\\d*,\\d*\\)").Select(m => m.Value));
}

var total = 0;

foreach (var match in matches)
{
    var parts = match.Replace("mul(", string.Empty).Replace(")", string.Empty).Split(',').Select(s => int.Parse(s)).ToArray();
    var result = parts[0] * parts[1];

    Console.WriteLine($"{match} = {result}");

    total += result;
}

Console.WriteLine($"Total = {total}");