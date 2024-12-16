


var input = File.ReadAllLines("Input.txt");
var grid = input.Select(i => i.ToCharArray()).ToArray();

Position startPositiom = new Position();
Position endPositiom = new Position();

for (var row = 0; row < grid.Length; row++)
{
    for (var col = 0; col < grid[row].Length; col++)
    {
        if (grid[row][col] == 'S')
        {
            startPositiom = new Position()
            {
                Row = row,
                Col = col,
            };
        }
        else if (grid[row][col] == 'E')
        {
            endPositiom = new Position()
            {
                Row = row,
                Col = col,
            };
        }
    }
}

var expansions = new PathPoint[grid.Length][];

for (var i = 0; i < grid.Length; i++)
{
    expansions[i] = new PathPoint[grid[i].Length];
}

var pathsToExpand = new List<Path>();

foreach (var movement in GetMovements(startPositiom, 'E'))
{
    var path = new Path();
    path.Movements.Add(movement);

    pathsToExpand.Add(path);
}

do
{
    Console.WriteLine($"Paths to expand: {pathsToExpand.Count()}");

    foreach (var path in pathsToExpand.ToArray())
    {
        var lastMove = path.Movements.Last();
        var nextMovements = GetMovements(lastMove.Position, lastMove.Direction);

        foreach (var nextMove in nextMovements)
        {
            var newPath = new Path();
            newPath.Movements.AddRange(path.Movements);
            newPath.Movements.Add(nextMove);

            if (nextMove.Position.Row == 8 && nextMove.Position.Col == 3)
            {
                int i = 0;
            }

            var existingExpansion = expansions[nextMove.Position.Row][nextMove.Position.Col];

            if ((existingExpansion is null) || (newPath.Distance <= existingExpansion.Distance))
            {
                if ((existingExpansion is null) || (newPath.Distance < existingExpansion.Distance))
                {
                    existingExpansion = new PathPoint();
                    expansions[nextMove.Position.Row][nextMove.Position.Col] = existingExpansion;

                    if (grid[nextMove.Position.Row][nextMove.Position.Col] != 'E')
                    {
                        pathsToExpand.Add(newPath);
                    }
                }

                existingExpansion.ShortestPaths.Add(newPath);
                existingExpansion.Distance = newPath.Distance;
            }
            else if (newPath.Distance <= existingExpansion.Distance + 1000)
            {
                if (grid[nextMove.Position.Row][nextMove.Position.Col] != 'E')
                {
                    pathsToExpand.Add(newPath);
                }
            }
        }

        pathsToExpand.Remove(path);
    }
}
while (pathsToExpand.Any());

var bestPositions = ExpandPossibleSteps(
    expansions[endPositiom.Row][endPositiom.Col].ShortestPaths.SelectMany(sp => sp.Movements).Select(m => m.Position).Distinct().ToArray(),
    Enumerable.Empty<Position>());

IEnumerable<Position> ExpandPossibleSteps(IEnumerable<Position> positions, IEnumerable<Position> knownPositions)
{
    var possiblePositions = new List<Position>(knownPositions);

    foreach (var position in positions)
    {
        possiblePositions.Add(position);

        if (expansions[position.Row][position.Col] is not null)
        {
            var additionalPositions = expansions[position.Row][position.Col].ShortestPaths.SelectMany(sp => sp.Movements).Select(m => m.Position).Distinct().Except(knownPositions).ToArray();

            possiblePositions.AddRange(ExpandPossibleSteps(additionalPositions, possiblePositions));
        }
    }

    return possiblePositions.Distinct().ToArray();
}

Console.WriteLine();

for (var row = 0; row < grid.Length; row++)
{
    for (var col = 0; col < grid[row].Length; col++)
    {
        if (bestPositions.Any(t => t.Row == row && t.Col == col))
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

//var total = expansions[endPositiom.Row][endPositiom.Col].Distance;
var total = bestPositions.Count() + 1;  // +1 for the start position.

Console.WriteLine($"Total = {total}");

List<Movement> GetMovements(Position position, char direction)
{
    var movements = new List<Movement>();

    if ((direction != 'W') && (position.Col > 0))
    {
        var nextPosition = grid[position.Row][position.Col + 1];

        if (nextPosition != '#')
        {
            movements.Add(new Movement()
            {
                Position = new Position() { Row = position.Row, Col = position.Col + 1 },
                Direction = 'E',
                Rotated = direction != 'E',
            });
        }
    }

    if ((direction != 'E') && (position.Col < (grid[position.Row].Length - 1)))
    {
        var nextPosition = grid[position.Row][position.Col - 1];

        if (nextPosition != '#')
        {
            movements.Add(new Movement()
            {
                Position = new Position() { Row = position.Row, Col = position.Col - 1 },
                Direction = 'W',
                Rotated = direction != 'W',
            });
        }
    }

    if ((direction != 'N') && (position.Row < (grid[position.Row].Length - 1)))
    {
        var nextPosition = grid[position.Row + 1][position.Col];

        if (nextPosition != '#')
        {
            movements.Add(new Movement()
            {
                Position = new Position() { Row = position.Row + 1, Col = position.Col },
                Direction = 'S',
                Rotated = direction != 'S',
            });
        }
    }

    if ((direction != 'S') && (position.Row > 0))
    {
        var nextPosition = grid[position.Row - 1][position.Col];

        if (nextPosition != '#')
        {
            movements.Add(new Movement()
            {
                Position = new Position() { Row = position.Row - 1, Col = position.Col },
                Direction = 'N',
                Rotated = direction != 'N',
            });
        }
    }

    return movements;
}

struct Position
{
    public int Row { get; init; }

    public int Col { get; init; }
}

struct Movement
{
    public Position Position { get; init; }

    public char Direction { get; init; }

    public bool Rotated { get; init; }
}

class Path
{
    public List<Movement> Movements { get; } = new List<Movement>();

    public int Distance
    {
        get
        {
            return (Movements.Count(m => m.Rotated) * 1000) + Movements.Count;
        }
    }
}

class PathPoint
{
    public List<Path> ShortestPaths { get; set; } = new List<Path>();

    public int Distance { get; set; }
} 