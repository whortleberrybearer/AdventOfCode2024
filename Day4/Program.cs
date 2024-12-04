
using System.Text.RegularExpressions;

var input = File.ReadAllLines("Input.txt");
var grid = input.Select(x => x.ToCharArray()).ToArray();

var total = 0;

for (int row = 1; row < grid.Length - 1; row++)
{
    for (int col = 1; col < grid[row].Length - 1; col++)
    {
        //if (grid[row][col] == 'X')
        //{
        //    total += FindXmas(row, col);
        //}
        if (grid[row][col] == 'A')
        {
            if (FindMas(row, col))
            {
                total += 1;
            };
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

bool FindMas(int row, int col)
{
    var count = 0;

    if ((grid[row - 1][col - 1] == 'M') && (grid[row + 1][col + 1] == 'S'))
    {
        count += 1;
    }

    if ((grid[row - 1][col + 1] == 'M') && (grid[row + 1][col - 1] == 'S'))
    {
        count += 1;
    }

    if ((grid[row + 1][col - 1] == 'M') && (grid[row - 1][col + 1] == 'S'))
    {
        count += 1;
    }

    if ((grid[row + 1][col + 1] == 'M') && (grid[row - 1][col - 1] == 'S'))
    {
        count += 1;
    }

    return count == 2;
}