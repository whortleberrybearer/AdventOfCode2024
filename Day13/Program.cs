
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
        }
    }
}

var total = 0;

for (var i = 0; i < games.Count; i++)
{
    game = games[i];

    var tokens = game.FindTokens();

    Console.WriteLine($"Game {i} = {tokens}");

    total += tokens;
}

// 48670 too high

Console.WriteLine($"Total = {total}");

Poisition ExtractPosition(string line)
{
    var parts = line.Split(new char[] { ' ', ',', '+', '=' }, StringSplitOptions.RemoveEmptyEntries);

    return new Poisition() { X = int.Parse(parts[1]), Y = int.Parse(parts[3]) };
}

public class Game
{
    public Poisition ButtonA { get; set; }

    public Poisition ButtonB { get; set; }

    public Poisition Prize { get; set; }

    public int FindTokens()
    {
        var minTokens = 0;

        for (var i = 1; i < 100; i++)
        {
            var aPos = ButtonA * i;

            if ((aPos.X > Prize.X) || (aPos.Y > Prize.Y))
            {
                // Beyond the prize.
                break;
            }

            var xDiv = Math.DivRem(Prize.X - aPos.X, ButtonB.X);

            if (xDiv.Remainder == 0)
            {
                var yDiv = Math.DivRem(Prize.Y - aPos.Y, ButtonB.Y);

                if (yDiv.Remainder == 0)
                {
                    var tokens = (i * 3) + yDiv.Quotient;

                    if ((minTokens == 0) || (tokens < minTokens))
                    {
                        minTokens = tokens;
                    }
                }
            }
        }

        return minTokens;
    }
}

public class Poisition
{
    public int X { get; set; }

    public int Y { get; set; }

    public static Poisition operator *(Poisition pos, int i) => new Poisition() { X = pos.X * i, Y = pos.Y * i };
}