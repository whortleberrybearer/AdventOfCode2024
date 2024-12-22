var input = File.ReadAllLines("Input.txt");

var total = 0L;

foreach (var line in input)
{
    var secretNumber = long.Parse(line);

    for (var i = 0; i < 2000; i++)
    {
        secretNumber = Part1(secretNumber);
        secretNumber = Part2(secretNumber);
        secretNumber = Part3(secretNumber);

        Console.WriteLine($"Calc {i + 1} = {secretNumber}");
    }

    total += secretNumber;
}

Console.WriteLine($"Total = {total}");

long Part1(long secretNumber)
{
    var mixNumber = secretNumber * 64;

    return Prune(Mix(secretNumber, mixNumber));
}

long Part2(long secretNumber)
{
    var mixNumber = secretNumber / 32;

    return Prune(Mix(secretNumber, mixNumber));
}

long Part3(long secretNumber)
{
    var mixNumber = secretNumber * 2048;

    return Prune(Mix(secretNumber, mixNumber));
}

long Mix(long secretNumber, long mixNumber)
{
    return secretNumber ^ mixNumber;
}

long Prune(long secretNumber)
{
    return secretNumber % 16777216;
}