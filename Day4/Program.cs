
using System.Text.RegularExpressions;

var input = File.ReadAllLines("Input.txt");
var grid = input.Select(x => x.ToCharArray()).ToArray();

var total = 0;

for (int row = 0; row < grid.Length; row++)
{
    for (int col = 0; col < grid[row].Length; col++)
    {
        if (grid[row][col] == 'X')
        {
            total += FindXmas(row, col);
        }
    }
}

Console.WriteLine($"Total = {total}");

int FindXmas(int row, int col)
{
    var count = 0;
    var forward = col < grid[row].Length - 3;
    var backward = col >= 3;
    var up = row >= 3;
    var down = row < grid.Length - 3;

    if (forward && (new string(grid[row].Skip(col).Take(4).ToArray()) == "XMAS"))
    {
        count++;
    }

    if (backward && (new string(grid[row].Skip(col - 3).Take(4).ToArray()) == "SAMX"))
    {
        count++;
    }

    if (down && (new string(grid.Skip(row).Take(4).Select(r => r[col]).ToArray()) == "XMAS"))
    {
        count++;
    }

    if (up && (new string(grid.Skip(row - 3).Take(4).Select(r => r[col]).ToArray()) == "SAMX"))
    {
        count++;
    }

    if (forward && up)
    {
        var line = string.Empty;

        for (var i = 0; i < 4; i++)
        {
            line += grid[row - i][col + i];
        }

        if (line == "XMAS")
        {
            count++;
        }
    }

    if (backward && up)
    {
        var line = string.Empty;

        for (var i = 0; i < 4; i++)
        {
            line += grid[row - i][col - i];
        }

        if (line == "XMAS")
        {
            count++;
        }
    }

    if (forward && down)
    {
        var line = string.Empty;

        for (var i = 0; i < 4; i++)
        {
            line += grid[row + i][col + i];
        }

        if (line == "XMAS")
        {
            count++;
        }
    }

    if (backward && down)
    {
        var line = string.Empty;

        for (var i = 0; i < 4; i++)
        {
            line += grid[row + i][col - i];
        }

        if (line == "XMAS")
        {
            count++;
        }
    }

    return count;
}


/*
Console.WriteLine("Rows");

for (var row = 0; row < grid.Length; row++)
{
    var line = new string(grid[row]);
    var count = FindXmas(line);

    Console.WriteLine($"{line} = {count}");

    total += count;
}

Console.WriteLine("Columns");

for (var col = 0; col < grid[0].Length; col++)
{
    var line = new string(grid.Select(r => r[col]).ToArray());
    var count = FindXmas(line);

    Console.WriteLine($"{line} = {count}");

    total += count;
}

int FindXmas(string line)
{
    return Regex.Count(line, "XMAS") + Regex.Count(line, "SAMX");
}
*/