
var input = File.ReadAllLines("Input.txt");
var numpad = new Numpad();
var directionPad = new DirectionPad();

var total = 0;

foreach (var code in input)
{
    var sequences1 = numpad.FindSequence(code);

    foreach (var sequence in sequences1)
    {
        var buttonPresses1 = string.Join("A", sequence) + "A";

        Console.WriteLine($"{code} = {buttonPresses1}");
    }

    //var sequence2 = directionPad.FindSequence(buttonPresses1);
    //var buttonPresses2 = string.Join("A", sequence2) + "A";

    //Console.WriteLine($"{buttonPresses1} = {buttonPresses2}");

    //var sequence3 = directionPad.FindSequence(buttonPresses2);
    //var buttonPresses3 = string.Join("A", sequence3) + "A";

    //Console.WriteLine($"{buttonPresses2} = {buttonPresses3}");

    //var calc = int.Parse(code.Replace("A", string.Empty)) * buttonPresses3.Length;

    //Console.WriteLine($"{buttonPresses3.Length} * {code.Replace("A", string.Empty).TrimStart('0')} = {calc}");

    //total += calc;

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

    private Dictionary<string, IEnumerable<string>> keyExpansions = new Dictionary<string, IEnumerable<string>>();

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

                        if (to == ' ')
                        {
                            continue;
                        }

                        keyExpansions.Add($"{from}-{to}", GetPaths(x, y, j, i));
                    }
                }
            }
        }
    }

    private IEnumerable<string> GetPaths(int fromX, int fromY, int toX, int toY)
    {
        var horizontal = fromX - toX;
        var horizontalPath = "";

        if (horizontal > 0)
        {
            horizontalPath = new string(Enumerable.Repeat('<', horizontal).ToArray());
        }
        else if (horizontal < 0)
        {
            horizontalPath = new string(Enumerable.Repeat('>', horizontal * -1).ToArray());
        }

        var vertical = fromY - toY;
        var verticalPath = "";

        if (vertical > 0)
        {
            verticalPath = new string(Enumerable.Repeat('^', vertical).ToArray());
        }
        else if (vertical < 0)
        {
            verticalPath = new string(Enumerable.Repeat('v', vertical * -1).ToArray());
        }

        var permuter = new StringPermuter(verticalPath + horizontalPath);
        var options = permuter.Permute().Distinct().ToList();

        // This is a hack to remove any path through the ' '.
        if (fromY == 3 || fromX == 0)
        {
            foreach (var option in options.ToArray())
            {
                if ((fromX == 2 && option.StartsWith("<<")) ||
                    (fromX == 1 && option.StartsWith("<")) ||
                    (fromY == 0 && option.StartsWith("vvv")) ||
                    (fromY == 1 && option.StartsWith("vv")) ||
                    (fromY == 2 && option.StartsWith("v")))
                {
                    options.Remove(option);
                }
            }
        }

        return options;
    }

    public IEnumerable<List<string>> FindSequence(string code)
    {
        var sequences = new List<List<string>>();
        var currentPosition = 'A';

        for (var i = 0; i < code.Length; i++)
        {
            var newSequence = new List<List<string>>();

            foreach (var expansion in keyExpansions[$"{currentPosition}-{code[i]}"])
            {
                if (!sequences.Any())
                {
                    newSequence.Add(new List<string> { expansion });
                }
                else
                {
                    foreach (var s in sequences)
                    {
                        newSequence.Add(s.Append(expansion).ToList());
                    }
                }
            }

            sequences = newSequence;
            currentPosition = code[i];
        }

        return sequences;
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

// Taken from https://gist.github.com/iterativo/8c8f58da086a9edf58f3
public class StringPermuter
{
    private string _word;

    public StringPermuter(string word)
    {
        _word = word;
    }

    public IEnumerable<string> Permute()
    {
        //Console.WriteLine($"Permutations of {_word}");
        return Process(_word.Length);
    }

    private IEnumerable<string> Process(int c)
    {
        var words = new List<string>();

        if (c == 1)
        {
            //Console.WriteLine(_word);
            return new string[] { _word };
        }

        for (var i = 0; i < c; i++)
        {
            words.AddRange(Process(c - 1));
            Rotate(c);
        };

        return words;
    }

    private void Rotate(int c)
    {
        var target = _word.Length - c;
        _word = _word.Substring(0, target)
            + _word.Substring(target + 1)
            + _word.Substring(target, 1);
    }
}