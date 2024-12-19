

var input = File.ReadAllLines("Input.txt");
var patterns = input[0].Split(", ");
var towels = input.Skip(2).ToArray();
var invalid = 0;

foreach (var towel in towels)
{
    var expandedOptions = ExpandOptions(towel);

    if (!CanMake(expandedOptions))
    {
        Console.WriteLine($"{towel} not valid");

        invalid += 1;
    }
}

var total = towels.Length - invalid;

Console.WriteLine($"Total = {total}");

List<string[]> ExpandOptions(string towel)
{
    var options = new List<string[]>();

    for (var length = 1; length <= towel.Length; length++)
    {
        var start = towel.Substring(0, length);

        if (length < towel.Length)
        {
            var endOptions = ExpandOptions(towel.Substring(length));

            foreach (var option in endOptions)
            {
                options.Add(option.Prepend(start).ToArray());
            }
        }
        else
        {
            options.Add(new string[] { start });
        }
    }

    return options;
}

bool CanMake(List<string[]> expandedOptions)
{
    foreach (var option in expandedOptions)
    {
        var valid = true;

        foreach (var towel in option)
        {
            if (!patterns.Contains(towel))
            {
                valid = false;
                break;
            }
        }

        if (valid)
        {
            return true;
        }
    }

    return false;
}


public class StringPermuter
{
    private string _word;

    public StringPermuter(string word)
    {
        _word = word;
    }

    public void Permute()
    {
        Console.WriteLine($"Permutations of {_word}");
        Process(_word.Length);
    }

    private void Process(int c)
    {
        if (c == 1)
        {
            Console.WriteLine(_word);
            return;
        }

        for (var i = 0; i < c; i++)
        {
            Process(c - 1);
            Rotate(c);
        };
    }

    private void Rotate(int c)
    {
        var target = _word.Length - c;
        _word = _word.Substring(0, target)
            + _word.Substring(target + 1)
            + _word.Substring(target, 1);
    }
}