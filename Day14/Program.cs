var input = File.ReadAllLines("Input.txt");
var robots = new List<Robot>();

foreach (var i in input)
{
    var parts = i.Split(new char[] { ' ', 'p', '=', ',', 'v' }, StringSplitOptions.RemoveEmptyEntries);

    var robot = new Robot()
    {
        Posiition = new Posiition()
        {
            X = int.Parse(parts[0]),
            Y = int.Parse(parts[1]),
        },
        Move = new Posiition()
        {
            X = int.Parse(parts[2]),
            Y = int.Parse(parts[3]),
        },
    };

    robots.Add(robot);
}

var maxRow = robots.Max(r => r.Posiition.Y) + 1;
var maxCol = robots.Max(r => r.Posiition.X) + 1;

foreach (var robot in robots)
{
    robot.MultipleMove(100, maxRow, maxCol);
}

for (var row = 0; row < maxRow; row++)
{
    for (var col = 0; col < maxCol; col++)
    {
        var count = robots.Count(r => r.Posiition.X == col && r.Posiition.Y == row);

        Console.Write(count == 0 ? "." : count.ToString());
    }

    Console.WriteLine();
}

Console.WriteLine();
Console.WriteLine();

var xDiv = Math.DivRem(maxCol, 2);
var yDiv = Math.DivRem(maxRow, 2);

var rightQuadrantStart = xDiv.Quotient;

if (xDiv.Remainder != 0)
{
    rightQuadrantStart += 1;
}

var bottomQuadrantStart = yDiv.Quotient;

if (yDiv.Remainder != 0)
{
    bottomQuadrantStart += 1;
}

var topLeft = robots.Count(r => r.Posiition.X < xDiv.Quotient && r.Posiition.Y < yDiv.Quotient);
var topRight = robots.Count(r => r.Posiition.X >= rightQuadrantStart && r.Posiition.Y < yDiv.Quotient);
var bottomLeft = robots.Count(r => r.Posiition.X < xDiv.Quotient && r.Posiition.Y >= bottomQuadrantStart);
var bottomRight = robots.Count(r => r.Posiition.X >= rightQuadrantStart && r.Posiition.Y >= bottomQuadrantStart);

Console.WriteLine($"TL={topLeft}, TR={topRight}, BL={bottomLeft}, BR={bottomRight}");

var total = topLeft * topRight * bottomLeft * bottomRight;

Console.WriteLine($"Total = {total}");

public class Robot
{
    public Posiition Posiition { get; set; }

    public Posiition Move { get; set; }

    internal void MultipleMove(int multiple, long maxRow, long maxCol)
    {
        var newX = (Posiition.X + (Move.X * multiple)) % maxCol;
        var newY = (Posiition.Y + (Move.Y * multiple)) % maxRow;

        if (newX < 0)
        {
            newX += maxCol;
        }

        if (newY < 0)
        {
            newY += maxRow;
        }

        Posiition = new Posiition() { X = newX, Y = newY };
    }
}

public class Posiition
{
    public long X { get; set; }

    public long Y { get; set; }
}