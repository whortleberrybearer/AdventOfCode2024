


var input = File.ReadAllLines("Input.txt");

//var total = 0L;
var buyerOptions = new List<BuyerOption>();

foreach (var line in input)
{
    var secretNumber = long.Parse(line);
    var buyerOption = new BuyerOption();

    buyerOptions.Add(buyerOption);

    var item = CalcaulateItem(secretNumber, null);

    buyerOption.Items.Add(item);

    var previosBananas = item.Bananas;
    
    for (var i = 0; i < 2000; i++)
    {
        secretNumber = Part1(secretNumber);
        secretNumber = Part2(secretNumber);
        secretNumber = Part3(secretNumber);

        //Console.WriteLine($"Calc {i + 1} = {secretNumber}");

        item = CalcaulateItem(secretNumber, previosBananas);

        buyerOption.Items.Add(item);

        previosBananas = item.Bananas;
    }

    //total += secretNumber;
}

var total = FindBestSequence();

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

Item CalcaulateItem(long secretNumber, long? previosBananas)
{
    var bananas = secretNumber % 10;
    var item = new Item() { Bananas = bananas, Change = bananas - previosBananas };

    Console.WriteLine($"{secretNumber} = {item.Bananas} ({item.Change})");

    return item;
}

long FindBestSequence()
{
    var bestTotal = 0L;
    var possibleSequences = new List<string>();

    foreach (var buyerOption in buyerOptions)
    {
        possibleSequences.AddRange(buyerOption.FindPossibleSequences());
    }

    foreach (var group in possibleSequences.GroupBy(ps => ps).OrderBy(g => g.Count()))
    {
        var total = 0L;

        foreach (var buyerOption in buyerOptions)
        {
            total += buyerOption.FindSequenceBananas(group.Key.Split(",").Select(c => int.Parse(c)).ToArray());
        }

        if (total > bestTotal)
        {
            bestTotal = total;
        }
    }

    return bestTotal;
}

class Item
{
    public long Bananas { get; init; }

    public long? Change { get; init; }
}

class BuyerOption
{
    public List<Item> Items { get; } = new List<Item>();

    public long FindSequenceBananas(int[] sequence)
    {
        for (var i = 0; i < Items.Count - 3; i++)
        {
            if (Items[i].Change == sequence[0] &&
                Items[i + 1].Change == sequence[1] &&
                Items[i + 2].Change == sequence[2] &&
                Items[i + 3].Change == sequence[3])
            {
                return Items[i + 3].Bananas;
            }
        }

        return 0;
    }

    internal List<string> FindPossibleSequences()
    {
        var sequences = new List<string>();

        for (var i = 1; i < Items.Count - 4; i++)
        {
            sequences.Add(string.Join(',', Items.Skip(i).Take(4).Select(i => i.Change)));
        }

        return sequences.Distinct().ToList();
    }
}