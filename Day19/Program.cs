
var input = File.ReadAllLines("Input.txt");
var patterns = input[0].Split(", ");
var towels = input.Skip(2).ToArray();
var invalid = 0;
var expansions = new Dictionary<string, List<Expansion>>();

var total = 0L;

foreach (var towel in towels)
{
    var expandedOptions = ExpandOptions(towel);
    var possibleExpansions = expandedOptions.Sum(eo => eo.PossibleExpansionsCount);

    Console.WriteLine($"{towel} has {possibleExpansions} expansions");

    total += possibleExpansions;

    //if (!expandedOptions.Any())
    //{
    //    Console.WriteLine($"{towel} not valid");

    //    invalid += 1;
    //}
}

//var total = towels.Length - invalid;

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
                    options.Add(new Expansion() { Start = start, End = endOptions, PossibleExpansionsCount = endOptions.Sum(eo => eo.PossibleExpansionsCount) });
                }
            }
            else
            {
                options.Add(new Expansion() { Start = start });
            }
        }
    }

    return options;
}

class Expansion
{
    public string Start { get; init; }

    public List<Expansion> End { get; init; }

    public long PossibleExpansionsCount { get; init; } = 1;
}