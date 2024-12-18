

var input = File.ReadAllLines("Input.txt");
var positions = new List<Position>();

foreach (var line in input)
{
    var parts = line.Split(',');

    positions.Add(new Position() { X = int.Parse(parts[0]), Y = int.Parse(parts[1]) });
}

var maxX = positions.Max(p => p.X);
var maxY = positions.Max(p => p.Y);

var grid = new char[maxY + 1][];

for (var i = 0; i < grid.Length; i++)
{
    grid[i] = Enumerable.Repeat('.', maxX + 1).ToArray();
}

Path? route = null;
var nextByte = 12;

foreach (var position in positions.Take(nextByte))
{
    grid[position.Y][position.X] = '#';
}

do
{
    var nextPosition = positions[nextByte++];

    grid[nextPosition.Y][nextPosition.X] = '#';

    route = FindPath();

    if (route != null)
    {
        for (var row = 0; row < grid.Length; row++)
        {
            for (var col = 0; col < grid[row].Length; col++)
            {
                if (route.Movements.Any(m => m.Position.Y == row && m.Position.X == col))
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
    }
}
while (route != null);

// var total = route.Cost - 1;

// Console.WriteLine($"Total = {total}");

var lastPosition = positions[nextByte - 1];

Console.WriteLine($"Blocker = {lastPosition.X},{lastPosition.Y}");

Path ? FindPath()
{
    var visited = new Path[maxY + 1][];

    for (var i = 0; i < visited.Length; i++)
    {
        visited[i] = new Path[maxX + 1];
    }

    var paths = new List<Path>();

    var startPosition = new Position();
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
        Console.WriteLine($"Paths to expand: {paths.Count}");

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

                if ((movement.Position.Y == maxY) && (movement.Position.X == maxX))
                {
                    Console.WriteLine($"Found end in: {path.Cost}");

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