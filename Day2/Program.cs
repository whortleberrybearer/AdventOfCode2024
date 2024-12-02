
var input = File.ReadAllLines("Input.txt");

var safeLevels = 0;

foreach (var report in input)
{
    var levels = report.Split(' ').Select(l => int.Parse(l)).ToArray();

    var isSafe = CheckIsSafe(levels);

    if (!isSafe)
    {
        isSafe = CheckIsSafe(levels.Reverse().ToArray());
    }

    Console.WriteLine($"{report} = {isSafe}");

    if (isSafe)
    {
        safeLevels += 1;
    }
}

Console.WriteLine($"Safe levels = {safeLevels}");

bool CheckIsSafe(int[] levels)
{
    for (int i = 0; i < levels.Count() - 1; i++)
    {
        var diff = levels[i + 1] - levels[i];

        if ((diff <= 0) || (diff > 3))
        {
            return false;
        }
    }

    return true;
}