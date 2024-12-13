var input = File.ReadAllLines("Input.txt");
var games = new List<Game>();

var game = new Game();
games.Add(game);

foreach (var line in input)
{
    if (line == string.Empty)
    {
        game = new Game();
        games.Add(game);
    }
    else
    {
        if (line.StartsWith("Button A"))
        {
            game.ButtonA = ExtractPosition(line.Replace("Button A: ", string.Empty));
        }
        else if (line.StartsWith("Button B"))
        {
            game.ButtonB = ExtractPosition(line.Replace("Button B: ", string.Empty));
        }
        else if (line.StartsWith("Prize"))
        {
            game.Prize = ExtractPosition(line.Replace("Prize: ", string.Empty));

            //game.Prize.X += 10000000000000L;
            //game.Prize.Y += 10000000000000L;
        }
    }
}

var total = 0L;

for (var i = 0; i < games.Count; i++)
{
    game = games[i];

    var tokens = game.FindTokens();

    Console.WriteLine($"Game {i} = {tokens}");

    total += tokens;
}

Console.WriteLine($"Total = {total}");

Poisition ExtractPosition(string line)
{
    var parts = line.Split(new char[] { ' ', ',', '+', '=' }, StringSplitOptions.RemoveEmptyEntries);

    return new Poisition() { X = long.Parse(parts[1]), Y = long.Parse(parts[3]) };
}

public class Game
{
    public Poisition ButtonA { get; set; }

    public Poisition ButtonB { get; set; }

    public Poisition Prize { get; set; }

    public long FindTokens()
    {
        var delta = ButtonA.X * ButtonB.Y - ButtonA.Y * ButtonB.X;

        if (delta == 0)
            throw new ArgumentException("Lines are parallel");

        var x = Math.DivRem(ButtonB.Y * Prize.X - ButtonB.X * Prize.Y, delta);
        var y = Math.DivRem(ButtonA.X * Prize.Y - ButtonA.Y * Prize.X, delta);

        // A 0 remained means they intersect on a valid move.
        if (x.Remainder == 0 && y.Remainder == 0)
        {
            return (x.Quotient * 3) + y.Quotient;
        }

        return 0;
    }
}

public class Poisition
{
    public long X { get; set; }

    public long Y { get; set; }

    public static Poisition operator *(Poisition pos, long i) => new Poisition() { X = pos.X * i, Y = pos.Y * i };
}