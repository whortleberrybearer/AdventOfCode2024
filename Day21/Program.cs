var input = File.ReadAllLines("Input.txt");
var numpad = new Numpad();
var directionPad = new DirectionPad();

var total = 0;

foreach (var code in input)
{
    var sequence1 = numpad.FindSequence(code);
    var buttonPresses1 = string.Join("A", sequence1) + "A";

    Console.WriteLine($"{code} = {buttonPresses1}");

    var sequence2 = directionPad.FindSequence(buttonPresses1);
    var buttonPresses2 = string.Join("A", sequence2) + "A";

    Console.WriteLine($"{buttonPresses1} = {buttonPresses2}");

    var sequence3 = directionPad.FindSequence(buttonPresses2);
    var buttonPresses3 = string.Join("A", sequence3) + "A";

    Console.WriteLine($"{buttonPresses2} = {buttonPresses3}");

    var calc = int.Parse(code.Replace("A", string.Empty)) * buttonPresses3.Length;

    Console.WriteLine($"{buttonPresses3.Length} * {code.Replace("A", string.Empty).TrimStart('0')} = {calc}");

    total += calc;

    Console.WriteLine();
}

Console.WriteLine($"Total = {total}");

class Numpad
{
    private char[][] keys = new char[][] {
            new char[] { '7', '8', '9' },
            new char[] { '4', '5', '6' },
            new char[] { '1', '2', '3' },
            new char[] { ' ', '0', 'A' }
        };

    private Dictionary<string, string> keyExpansions = new Dictionary<string, string>();

    public Numpad()
    {
        for (var y = keys.Length - 1; y >= 0; y--)
        {
            for (var x = keys[y].Length - 1; x >= 0; x--)
            {
                var from = keys[y][x];

                if (from == ' ')
                {
                    break;
                }

                for (var i = keys.Length - 1; i >= 0; i--)
                {
                    for (var j = keys[i].Length - 1; j >= 0; j--)
                    {
                        var to = keys[i][j];

                        if ((to == ' ') || (from == to))
                        {
                            continue;
                        }

                        var key = $"{from}-{to}";

                        if (!keyExpansions.ContainsKey(key))
                        {
                            var accross = x - j;
                            var path = "";
                            var reversePath = "";

                            if (accross > 0)
                            {
                                path = new string(Enumerable.Repeat('<', x - j).ToArray());
                                reversePath = new string(Enumerable.Repeat('>', x - j).ToArray());
                            }
                            else if (accross < 0)
                            {
                                path = new string(Enumerable.Repeat('>', accross * -1).ToArray());
                                reversePath = new string(Enumerable.Repeat('<', accross * -1).ToArray());
                            }

                            keyExpansions.Add(key, $"{new string(Enumerable.Repeat('^', y - i).ToArray())}{path}");
                            keyExpansions.Add($"{to}-{from}", $"{reversePath}{new string(Enumerable.Repeat('v', y - i).ToArray())}");
                        }
                    }
                }
            }
        }
    }

    public IEnumerable<string> FindSequence(string code)
    {
        var sequence = new List<string>();
        var currentPosition = 'A';

        for (var i = 0; i < code.Length; i++)
        {
            sequence.Add(keyExpansions[$"{currentPosition}-{code[i]}"]);

            currentPosition = code[i];
        }

        return sequence;
    }
}

class DirectionPad
{
    private char[][] keys = new char[][] {
            new char[] { ' ', '^', 'A' },
            new char[] { '<', 'v', '>' },
        };

    private Dictionary<string, string> keyExpansions = new Dictionary<string, string>();

    public DirectionPad()
    {
        for (var y = 0; y < keys.Length; y++)
        {
            for (var x = keys[y].Length - 1; x >= 0; x--)
            {
                var from = keys[y][x];

                if (from == ' ')
                {
                    break;
                }

                for (var i = 0; i < keys.Length; i++)
                {
                    for (var j = keys[i].Length - 1; j >= 0; j--)
                    {
                        var to = keys[i][j];

                        if ((to == ' ') || (from == to))
                        {
                            continue;
                        }

                        var key = $"{from}-{to}";

                        if (!keyExpansions.ContainsKey(key))
                        {
                            var accross = x - j;
                            var path = "";
                            var reversePath = "";

                            if (accross > 0)
                            {
                                path = new string(Enumerable.Repeat('<', x - j).ToArray());
                                reversePath = new string(Enumerable.Repeat('>', x - j).ToArray());
                            }
                            else if (accross < 0)
                            {
                                path = new string(Enumerable.Repeat('>', accross * -1).ToArray());
                                reversePath = new string(Enumerable.Repeat('<', accross * -1).ToArray());
                            }

                            keyExpansions.Add(key, $"{new string(Enumerable.Repeat('v', i - y).ToArray())}{path}");
                            keyExpansions.Add($"{to}-{from}", $"{reversePath}{new string(Enumerable.Repeat('^', i - y).ToArray())}");
                        }
                    }
                }
            }
        }
    }

    public IEnumerable<string> FindSequence(string code)
    {
        var sequence = new List<string>();
        var currentPosition = 'A';

        for (var i = 0; i < code.Length; i++)
        {
            if (currentPosition != code[i])
            {
                sequence.Add(keyExpansions[$"{currentPosition}-{code[i]}"]);

                currentPosition = code[i];
            }
            else
            {
                sequence.Add("");
            }
        }

        return sequence;
    }
}

