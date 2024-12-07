
var input = File.ReadAllLines("Input.txt");
var collabarations = input.Select(i => i.Split(new char[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(i => long.Parse(i)).ToArray()).ToArray();

var total = 0l;

foreach (var collabaration in collabarations)
{
    if (CheckCollaberation(collabaration))
    {
        Console.WriteLine(collabaration);

        total += collabaration[0];
    }
}

Console.WriteLine($"Total = {total}");

bool CheckCollaberation(long[] collabaration)
{
    var results = new List<long>();

    results.AddRange(AddNext(collabaration[1], collabaration.Skip(2)));
    results.AddRange(MultiplyNext(collabaration[1], collabaration.Skip(2)));

    return results.Contains(collabaration[0]);
}

IEnumerable<long> AddNext(long value, IEnumerable<long> remainingValues)
{
    value += remainingValues.ElementAt(0);

    if (remainingValues.Count() == 1)
    {
        return [value];
    }

    return AddNext(value, remainingValues.Skip(1)).Concat(MultiplyNext(value, remainingValues.Skip(1)));
}

IEnumerable<long> MultiplyNext(long value, IEnumerable<long> remainingValues)
{
    value *= remainingValues.ElementAt(0);

    if (remainingValues.Count() == 1)
    {
        return [value];
    }

    return AddNext(value, remainingValues.Skip(1)).Concat(MultiplyNext(value, remainingValues.Skip(1)));
}