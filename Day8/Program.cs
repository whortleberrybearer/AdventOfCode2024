
var input = File.ReadAllLines("Input.txt");
var grid = input.Select(x => x.ToCharArray()).ToArray();
var antennas = new List<Antenna>();
var antinodes = new List<Antenna>();

for (var row = 0; row < grid.Length; row++)
{
    for (var col = 0; col < grid[row].Length; col++)
    {
        if (grid[row][col] != '.')
        {
            antennas.Add(new Antenna()
            {
                Row = row,
                Col = col,
                Frequency = grid[row][col],
            });
        }
    }
}

var antennaGroups = antennas.GroupBy(a => a.Frequency);

foreach (var antennaGroup in antennaGroups)
{
    for (var i = 0; i < antennaGroup.Count(); i++)
    {
        for (var j = i + 1; j < antennaGroup.Count(); j++)
        {
            AddAntinodes(antennaGroup.ElementAt(i), antennaGroup.ElementAt(j));
        }
    }
}

for (var row = 0; row < grid.Length; row++)
{
    for (var col = 0; col < grid[row].Length; col++)
    {
        var antinode = antinodes.FirstOrDefault(a => a.Row == row && a.Col == col);
        if (antinodes.Any(a => a.Row == row && a.Col == col))
        {
            Console.Write(antinode.Frequency);
        }
        else
        {
            Console.Write(grid[row][col]);
        }
    }

    Console.WriteLine();
}

var total = antinodes.Distinct().Count() + antennas.Count();

Console.WriteLine();
Console.WriteLine($"Total = {total}");

void AddAntinodes(Antenna antennaA, Antenna antennaB)
{
    var diffRow = antennaB.Row - antennaA.Row;
    var diffCol = antennaB.Col - antennaA.Col;

    AddAntinode(antennaA, -diffRow, -diffCol);
    AddAntinode(antennaB, diffRow, diffCol);
}

void AddAntinode(Antenna antenna, int diffRow, int diffCol)
{
    var antinode = new Antenna()
    {
        Row = antenna.Row + diffRow,
        Col = antenna.Col + diffCol,
        Frequency = '#',
    };

    if ((antinode.Row >= 0) && (antinode.Row < grid.Length) && (antinode.Col >= 0) && (antinode.Col < grid[antinode.Row].Length))
    {
        if (grid[antinode.Row][antinode.Col] == '.')
        {
            antinodes.Add(antinode);
        }

        AddAntinode(antinode, diffRow, diffCol);
    }
}

struct Antenna
{
    public int Row { get; init; }

    public int Col { get; init; }

    public char Frequency { get; init; }
}