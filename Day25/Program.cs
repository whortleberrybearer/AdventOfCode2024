
var input = File.ReadAllLines("Input.txt");
var locks = new List<Lock>();
var keys = new List<Key>();

for (var i = 0; i < input.Length; i += 8)
{
    var grid = input.Skip(i).Take(6).Select(l => l.ToCharArray()).ToArray();

    if (grid[0][0] == '#')
    {
        locks.Add(new Lock() { Bits = ExtractBits(grid) });
    }
    else if (grid[0][0] == '.')
    {
        keys.Add(new Key() { Bits = ExtractBits(grid) });
    }
}

var total = 0;

Console.WriteLine($"Total = {total}");

int[] ExtractBits(char[][] grid)
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

class Lock
{
    public int[] Bits { get; init; }
}

class Key
{
    public int[] Bits { get; init; }
}