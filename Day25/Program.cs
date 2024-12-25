var input = File.ReadAllLines("Input.txt");
var locks = new List<Lock>();
var keys = new List<Key>();

for (var i = 0; i < input.Length; i += 8)
{
    var grid = input.Skip(i).Take(6).Select(l => l.ToCharArray()).ToArray();

    if (grid[0][0] == '#')
    {
        var lck = new Lock() { Bits = ExtractLockBits(grid) };

        Console.WriteLine($"Lock: {string.Join(",", lck.Bits)}");

        locks.Add(lck);
    }
    else if (grid[0][0] == '.')
    {
        var key = new Key() { Bits = ExtractKeyBits(grid) };

        Console.WriteLine($"Key: {string.Join(",", key.Bits)}");

        keys.Add(key);
    }
}

var total = 0;

foreach (var lck in locks)
{
    foreach (var key in keys)
    {
        if (lck.KeyFits(key))
        {
            Console.WriteLine($"Lock:{string.Join(",", lck.Bits)} and Key:{string.Join(",", key.Bits)} fit");

            total += 1;
        }
    }
}

Console.WriteLine($"Total = {total}");

int[] ExtractLockBits(char[][] grid)
{
    var bits = new List<int>();
    
    for (var i = 0; i < grid[0].Length; i++)
    {
        var count = 0;

        for (var j = 1; j < grid.Length; j++)
        {
            if (grid[j][i] == '#')
            {
                count += 1;
            }
            else
            {
                break;
            }
        }

        bits.Add(count);
    }
     
    return bits.ToArray();
}

int[] ExtractKeyBits(char[][] grid)
{
    var bits = new List<int>();

    for (var i = 0; i < grid[0].Length; i++)
    {
        var count = 0;

        for (var j = grid.Length - 1; j > 0; j--)
        {
            if (grid[j][i] == '#')
            {
                count += 1;
            }
            else
            {
                break;
            }
        }

        bits.Add(count);
    }

    return bits.ToArray();
}

class Lock
{
    public int[] Bits { get; init; }

    internal bool KeyFits(Key key)
    {
        for (var i = 0; i < Bits.Length; i++)
        {
            if (Bits[i] + key.Bits[i] > 5)
            {
                return false;
            }
        }

        return true;
    }
}

class Key
{
    public int[] Bits { get; init; }
}