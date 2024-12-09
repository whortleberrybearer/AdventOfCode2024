using System.Collections.Generic;

var input = File.ReadAllLines("Input.txt")[0];

var fullString = new List<string>();
var fileId = 0;

for (var i = 0; i < input.Length; i++)
{
    if (i % 2 == 0)
    {
        fullString.AddRange(Enumerable.Repeat(fileId.ToString(), int.Parse(input[i].ToString())));

        fileId += 1;
    }
    else
    {
        fullString.AddRange(Enumerable.Repeat(".", int.Parse(input[i].ToString())));
    }
}

foreach (var s in fullString)
{
    Console.Write(s);
}

Console.WriteLine();

var replaceAt = fullString.IndexOf(".");

while (replaceAt != -1)
{
    var toMove = fullString.Last();
    fullString.RemoveAt(fullString.Count - 1);

    if (toMove != ".")
    {
        fullString[replaceAt] = toMove;

        replaceAt = fullString.IndexOf(".");
    }
}

foreach (var s in fullString)
{
    Console.Write(s);
}

Console.WriteLine();

var total = 0l;

for (var i = 0; i < fullString.Count; i++)
{
    total += (i * int.Parse(fullString[i]));
}

Console.WriteLine();
Console.WriteLine($"Total = {total}");

// 2132861615 too low