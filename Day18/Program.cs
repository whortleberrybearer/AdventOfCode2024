var input = File.ReadAllLines("Input.txt");
var positions = new List<Position>();

foreach (var line in input)
{
    var parts = line.Split(',');

    positions.Add(new Position() { X = int.Parse(parts[0]), Y = int.Parse(parts[1]) });
}

var maxX = positions.Max(p => p.X) + 1;
var maxY = positions.Max(p => p.Y) + 1;

var grid = new char[maxY][];

for (var i = 0; i < maxY; i++)
{
    grid[i] = Enumerable.Repeat('.', maxX).ToArray();
}

foreach (var position in positions.Take(12))
{
    grid[position.Y][position.X] = '#';
}

for (var row = 0; row < grid.Length; row++)
{
    for (var col = 0; col < grid[row].Length; col++)
    {
        Console.Write(grid[row][col]);
    }

    Console.WriteLine();
}

Console.WriteLine();

var total = 0;

Console.WriteLine($"Total = {total}");

struct Position
{
    public int X { get; init; }

    public int Y { get; init; }
}