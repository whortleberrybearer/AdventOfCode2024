var input = File.ReadAllLines("Input.txt");
var stones = input[0].Split(' ').ToList();
var stoneLookups = new Dictionary<string, Stone>();

foreach (var stone in stones)
{
    stoneLookups.Add(stone, new Stone() { Value = stone });
}

var blinks = 6;

for (var i = 0; i < blinks; i++)
{
    foreach (var stone in stoneLookups.Values.Where(s => !s.Next.Any()).ToArray())
    {
        if (stone.Value == "0")
        {
            stone.Next.Add(GetAndAddStone("1"));
        }
        else if (stone.Value.Length % 2 == 0)
        {
            var left = stone.Value.Substring(0, stone.Value.Length / 2);
            var right = long.Parse(stone.Value.Substring(stone.Value.Length / 2)).ToString();

            stone.Next.Add(GetAndAddStone(left));
            stone.Next.Add(GetAndAddStone(right));
        }
        else
        {
            stone.Next.Add(GetAndAddStone((long.Parse(stone.Value) * 2024).ToString()));
        }
    }
}

var newStones = new List<Stone>();

foreach (var stone in stones)
{
    newStones.Add(stoneLookups[stone]);
}

for (var i = 0; i < blinks; i++)
{
    Console.WriteLine($"Blink {i + 1}");

    var existingStones = newStones.ToArray();
    newStones = new List<Stone>();

    foreach (var stone in existingStones)
    {
        newStones.AddRange(stone.Next);
    }

    Console.WriteLine(string.Join(" ", newStones.Select(ns => ns.Value)));
}

var total = newStones.Count;

Console.WriteLine($"Total = {total}");

Stone GetAndAddStone(string value)
{
    if (!stoneLookups.TryGetValue(value, out var nextStone))
    {
        nextStone = new Stone() { Value = value };
        stoneLookups.Add(nextStone.Value, nextStone);
    }

    return nextStone;
}

class Stone
{
    public string Value { get; init; }

    public List<Stone> Next { get; } = new List<Stone>();
}