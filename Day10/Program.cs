
using System.Collections.Generic;

var input = File.ReadAllLines("Input.txt");
var grid = input.Select(i => i.Select(c => c == '.' ? -1 : int.Parse(c.ToString())).ToArray()).ToArray();

var total = 0;

for (var row = 0; row < grid.Length; row++)
{
    for (var col = 0; col < grid[row].Length; col++)
    {
        if (grid[row][col] == 0)
        {
            var trailHeads = FindTrailHeads(0, row, col).Distinct();

            Console.WriteLine($"{row},{col} = {trailHeads.Count()}");

            total += trailHeads.Count();
        }
    }
}

Console.WriteLine($"Total = {total}");

IEnumerable<Position> FindTrailHeads(int height, int row, int col)
{
    if (grid[row][col] != height)
    {
        return Enumerable.Empty<Position>();
    }

    if (grid[row][col] == 9)
    {
        return Enumerable.Repeat(new Position() {  Row = row, Col = col }, 1);
    }

    var trailHeads = new List<Position>();

    if (row > 0)
    {
        trailHeads.AddRange(FindTrailHeads(height + 1, row - 1, col));    
    }

    if (row < (grid.Length - 1))
    {
        trailHeads.AddRange(FindTrailHeads(height + 1, row + 1, col));
    }

    if (col > 0)
    {
        trailHeads.AddRange(FindTrailHeads(height + 1, row, col - 1));
    }

    if (col < (grid[row].Length - 1))
    {
        trailHeads.AddRange(FindTrailHeads(height + 1, row, col + 1));
    }

    return trailHeads;
}

struct Position
{
    public int Row { get; init; }

    public int Col { get; init; }
}