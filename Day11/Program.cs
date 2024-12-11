var input = File.ReadAllLines("Input.txt");
var stones = input[0].Split(' ').ToList();

for (var i = 0; i < 25; i++)
{
    for (var j = 0; j < stones.Count; j++)
    {
        if (stones[j] == "0")
        {
            stones[j] = "1";
        }
        else if (stones[j].Length % 2 == 0)
        {
            var toSplit = stones[j];

            var left = toSplit.Substring(0, toSplit.Length / 2);
            var right = long.Parse(toSplit.Substring(toSplit.Length / 2)).ToString();

            stones[j] = left;
            
            stones.Insert(j + 1, right);

            // Move j along so not to process the stone just added.
            j += 1;
        }
        else
        {
            stones[j] = (long.Parse(stones[j]) * 2024).ToString();
        }
    }

    Console.WriteLine(string.Join(" ", stones));
}

var total = stones.Count;

Console.WriteLine($"Total = {total}");