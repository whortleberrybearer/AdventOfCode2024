var input = File.ReadAllLines("Input.txt");
var mapEnd = input.Count(i => i.Contains("#"));
var map = input.Take(mapEnd).Select(i => i.Replace("#", "##").Replace(".", "..").Replace("O","[]").Replace("@", "@.").ToCharArray()).ToArray();
var movements = string.Join(string.Empty, input.Skip(mapEnd + 1));

var robotRow = 0;
var robotCol = 0;

for (var row = 0; row < map.Length; row++)
{
    for (var col = 0; col < map[row].Length; col++)
    {
        if (map[row][col] == '@')
        {
            robotRow = row;
            robotCol = col;

            break;
        }
    }
}

foreach (var move in movements)
{
    if (move == '^')
    {
        if (MoveRobot(robotRow, robotCol, -1, 0))
        {
            robotRow -= 1;
        }
    }
    else if (move == 'v')
    {
        if (MoveRobot(robotRow, robotCol, 1, 0))
        {
            robotRow += 1;
        }
    }
    else if (move == '<')
    {
        if (MoveRobot(robotRow, robotCol, 0, -1))
        {
            robotCol -= 1;
        }
    }
    else if (move == '>')
    {
        if (MoveRobot(robotRow, robotCol, 0, 1))
        {
            robotCol += 1;
        }
    }
}

for (var row = 0; row < map.Length; row++)
{
    for (var col = 0; col < map[row].Length; col++)
    {
        Console.Write(map[row][col]);
    }

    Console.WriteLine();
}

Console.WriteLine();
Console.WriteLine();

var total = 0;

for (var row = 0; row < map.Length; row++)
{
    for (var col = 0; col < map[row].Length; col++)
    {
        //if (map[row][col] == 'O')
        if (map[row][col] == '[')
        {
            var coordinate = (row * 100) + col;

            Console.WriteLine($"{row},{col} = {coordinate}");

            total += coordinate;
        }
    }
}

Console.WriteLine($"Total = {total}");

bool MoveRobot(int row, int col, int rowMove, int colMove)
{
    var nextRow = row + rowMove;
    var nextCol = col + colMove;

    if (map[nextRow][nextCol] == '.')
    {
        map[nextRow][nextCol] = map[row][col];
        map[row][col] = '.';

        return true;
    }
    else if ((rowMove == 0) && ((map[nextRow][nextCol] == '[') || (map[nextRow][nextCol] == ']')))
    {
        // We are pushing the box to the side, so can just move like normal.
        if (MoveRobot(nextRow, nextCol, rowMove, colMove))
        {
            map[nextRow][nextCol] = map[row][col];
            map[row][col] = '.';

            return true;
        }
    }
    else if (map[nextRow][nextCol] != '#')
    {
        // Moving the box up or down, so need to check that the whole bounds of the box can move.
        if (CanMoveVertical(row, col, rowMove, colMove))
        {
            var otherNextCol = map[nextRow][nextCol] == '[' ? nextCol + 1 : nextCol - 1;

            MoveRobot(nextRow, nextCol, rowMove, colMove);
            MoveRobot(nextRow, otherNextCol, rowMove, colMove);

            map[nextRow][nextCol] = map[row][col];
            map[row][col] = '.';

            return true;
        }
    }

    return false;
}

bool CanMoveVertical(int row, int col, int rowMove, int colMove)
{
    var nextRow = row + rowMove;
    var nextCol = col + colMove;

    if (map[nextRow][nextCol] == '.')
    {
        return true;
    }
    else if (map[nextRow][nextCol] == '#')
    {
        return false;
    }

    var otherNextCol = map[nextRow][nextCol] == '[' ? nextCol + 1 : nextCol - 1;

    return CanMoveVertical(nextRow, nextCol, rowMove, colMove) && CanMoveVertical(nextRow, otherNextCol, rowMove, colMove);
}