var input = File.ReadAllLines("Input.txt");
var splitInput = input.Select(i => i.Split(' ', StringSplitOptions.RemoveEmptyEntries));

var list1 = splitInput.Select(si => int.Parse(si[0])).OrderBy(i => i).ToArray();
var list2 = splitInput.Select(si => int.Parse(si[1])).OrderBy(i => i).ToArray();

var total = 0;

/*for (var i = 0; i < list1.Length; i++)
{
    var diff = list2[i] - list1[i];

    Console.WriteLine($"{list2[i]} - {list1[i]} = {diff}");

    total += Math.Abs(diff);
}*/

foreach (var item in list1)
{
    var occurances = list2.Count(i => i == item);
    var simularityScore = item * occurances;

    Console.WriteLine($"{item} * {occurances} = {simularityScore}");

    total += simularityScore;
}

Console.WriteLine($"Total = {total}");