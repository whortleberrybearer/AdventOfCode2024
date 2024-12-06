
var input = File.ReadAllLines("Input.txt");

var grid = input.Select(x => x.ToCharArray()).ToArray();
var visited = input.Select(x => x.ToCharArray()).ToArray();
var positionRow = -1;
var positionCol = -1;

for (var row = 0; row < grid.Length; row++)
{
    for (var col = 0; col < grid[row].Length; col++)
    {
        if (grid[row][col] == '^')
        {
            positionCol = col;
            positionRow = row;

            break;
        }
    }
}

Console.WriteLine($"Start position = {positionRow},{positionCol}");

var moveRow = -1;
var moveCol = 0;

var total = 0;

while ((positionRow > -1) && 
    (positionRow < grid.Length) && 
    (positionCol > -1) && 
    (positionCol < grid[positionRow].Length))
{
    if (visited[positionRow][positionCol] != 'X')
    {
        visited[positionRow][positionCol] = 'X';

        total += 1;
    }

    var nextRow = positionRow + moveRow;
    var nextCol = positionCol + moveCol;

    if ((nextRow < 0) ||
        (nextRow >= grid.Length) ||
        (nextCol < 0) ||
        (nextCol >= grid[positionRow].Length))
    {
        break;
    }

    if (grid[nextRow][nextCol] == '#')
    {
        // Rotate 90 degrees.
        if ((moveRow == 1) && (moveCol == 0))
        {
            moveRow = 0;
            moveCol = -1;
        }
        else if ((moveRow == 0) && (moveCol == -1))
        {
            moveRow = -1;
            moveCol = 0;
        }
        else if ((moveRow == -1) && (moveCol == 0))
        {
            moveRow = 0;
            moveCol = 1;
        }
        else if ((moveRow == 0) && (moveCol == 1))
        {
            moveRow = 1;
            moveCol = 0;
        }

        positionRow += moveRow;
        positionCol += moveCol;
    }
    else 
    {
        positionRow = nextRow;
        positionCol = nextCol;
    }
}

Console.WriteLine();

for (var row = 0; row < grid.Length; row++)
{
    for (var col = 0; col < grid[row].Length; col++)
    {
        Console.Write(visited[row][col]);
    }

    Console.WriteLine();
}

Console.WriteLine();
Console.WriteLine($"Total = {total}");