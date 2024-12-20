var input = File.ReadAllLines("Input.txt");
var grid = input.Select(i => i.ToCharArray()).ToArray();

Position startPosition = new Position();
Position endPositiom = new Position();

for (var row = 0; row < grid.Length; row++)
{
    for (var col = 0; col < grid[row].Length; col++)
    {
        if (grid[row][col] == 'S')
        {
            startPosition = new Position()
            {
                X = col,
                Y = row,
            };
        }
        else if (grid[row][col] == 'E')
        {
            endPositiom = new Position()
            {
                X = col,
                Y = row,
            };
        }
    }
}

var path = FindPath();

for (var row = 0; row < grid.Length; row++)
{
    for (var col = 0; col < grid[row].Length; col++)
    {
        if (path.Movements.Any(m => m.Position.Y == row && m.Position.X == col))
        {
            Console.Write('O');
        }
        else
        {
            Console.Write(grid[row][col]);
        }
    }

    Console.WriteLine();
}

Console.WriteLine();
Console.WriteLine($"Found end in: {path.Cost}");

var total = 0;

Console.WriteLine($"Total = {total}");

Path? FindPath()
{
    var visited = new Path[grid.Length][];

    for (var i = 0; i < visited.Length; i++)
    {
        visited[i] = new Path[grid[i].Length];
    }

    var paths = new List<Path>();

    foreach (var movement in GetPossibleMovements(startPosition))
    {
        var path = new Path();
        path.Movements.Add(new Movement() { Position = startPosition });
        path.Movements.Add(movement);

        paths.Add(path);
    }

    Path? bestPath = null;

    while (paths.Count > 0 && (bestPath is null))
    {
        var pathToExpand = paths.First();
        paths.RemoveAt(0);

        foreach (var movement in GetPossibleMovements(pathToExpand.Movements.Last().Position))
        {
            var path = new Path();
            path.Movements.AddRange(pathToExpand.Movements);
            path.Movements.Add(movement);

            if (path.Cost < (visited[movement.Position.Y][movement.Position.X]?.Cost ?? int.MaxValue))
            {
                visited[movement.Position.Y][movement.Position.X] = path;

                if ((movement.Position.Y == endPositiom.Y) && (movement.Position.X == endPositiom.X))
                {
                    bestPath = path;
                }
                else if (path.Cost < (bestPath?.Cost ?? int.MaxValue))
                {
                    paths.Add(path);

                    paths = paths.OrderBy(p => p.Cost).ToList();
                }
            }
        }
    }

    return bestPath;
}

IEnumerable<Movement> GetPossibleMovements(Position position)
{
    var movements = new List<Movement>();

    if (position.X > 0)
    {
        var nextPosition = grid[position.Y][position.X - 1];

        if (nextPosition != '#')
        {
            movements.Add(new Movement()
            {
                Position = new Position() { Y = position.Y, X = position.X - 1 },
            });
        }
    }

    if (position.X < (grid[position.Y].Length - 1))
    {
        var nextPosition = grid[position.Y][position.X + 1];

        if (nextPosition != '#')
        {
            movements.Add(new Movement()
            {
                Position = new Position() { Y = position.Y, X = position.X + 1 },
            });
        }
    }

    if (position.Y < (grid[position.Y].Length - 1))
    {
        var nextPosition = grid[position.Y + 1][position.X];

        if (nextPosition != '#')
        {
            movements.Add(new Movement()
            {
                Position = new Position() { Y = position.Y + 1, X = position.X },
            });
        }
    }

    if (position.Y > 0)
    {
        var nextPosition = grid[position.Y - 1][position.X];

        if (nextPosition != '#')
        {
            movements.Add(new Movement()
            {
                Position = new Position() { Y = position.Y - 1, X = position.X },
            });
        }
    }

    return movements;
}

struct Position
{
    public int X { get; init; }

    public int Y { get; init; }
}

class Movement
{
    public Position Position { get; init; }
}

class Path
{
    public List<Movement> Movements { get; } = new List<Movement>();

    public int Cost => Movements.Count;
}