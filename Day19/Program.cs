var input = File.ReadAllLines("Input.txt");
var patterns = input[0].Split(", ");
var towels = input.Skip(2).ToArray();
var invalid = 0;
var expansions = new Dictionary<string, List<Expansion>>();

foreach (var towel in towels)
{
    var expandedOptions = ExpandOptions(towel);

    Console.WriteLine($"{towel} has {expandedOptions.Count} expansions");

    if (!expandedOptions.Any())
    {
        Console.WriteLine($"{towel} not valid");

        invalid += 1;
    }
}

var total = towels.Length - invalid;

Console.WriteLine($"Total = {total}");

List<Expansion> ExpandOptions(string towel)
{
    Console.WriteLine($"Explanding {towel}");

    var options = new List<Expansion>();

    for (var length = 1; length <= towel.Length; length++)
    {
        var start = towel.Substring(0, length);

        // Only continue expanding is already got a valid start pattern.
        if (patterns.Contains(start))
        {
            if (length < towel.Length)
            {
                var toExpand = towel.Substring(length);

                if (!expansions.TryGetValue(toExpand, out var endOptions))
                {
                    endOptions = ExpandOptions(toExpand);

                    expansions[toExpand] = endOptions;
                }

                if (endOptions.Any())
                {
                    options.Add(new Expansion() { Start = start, End = endOptions });
                }
            }
            else
            {
                options.Add(new Expansion() { Start = start, End = new List<Expansion>() });
            }
        }
    }

    return options;
}

class Expansion
{
    public string Start { get; init; }

    public List<Expansion> End { get; init; }
}