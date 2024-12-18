

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

foreach (var position in positions.Take(1024))
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
Console.WriteLine();

var route = FindPath();

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

var total = route.Cost - 1;

Console.WriteLine($"Total = {total}");

Path FindPath()
{
    var visited = new Path[maxY + 1][];

    for (var i = 0; i < visited.Length; i++)
    {
        visited[i] = new Path[maxX + 1];
    }

    var paths = new List<Path>();

    var startPosition = new Position();
    foreach (var movement in GetPossibleMovements(startPosition, ' '))
    {
        var path = new Path();
        path.Movements.Add(new Movement() { Position = startPosition, Direction = ' ' });
        path.Movements.Add(movement);

        paths.Add(path);
    }

    Path bestPath = null;

    while (paths.Count > 0 && (bestPath is null))
    {
        Console.WriteLine($"Paths to expand: {paths.Count}");

        var pathToExpand = paths.First();
        paths.RemoveAt(0);


        Console.WriteLine($"Expand: X:{pathToExpand.Movements.Last().Position.X}, Y:{pathToExpand.Movements.Last().Position.Y}");

        foreach (var movement in GetPossibleMovements(pathToExpand.Movements.Last().Position, pathToExpand.Movements.Last().Direction))
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

IEnumerable<Movement> GetPossibleMovements(Position position, char direction)
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
                Direction = 'W',
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
                Direction = 'E',
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
                Direction = 'S',
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
                Direction = 'N',
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

    public char Direction { get; init; }
}

class Path
{
    public List<Movement> Movements { get; } = new List<Movement>();

    public int Cost => Movements.Count;
}