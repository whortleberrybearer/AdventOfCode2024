var input = File.ReadAllLines("Input.txt");
var wires = new Dictionary<string, bool>();

foreach (var line in input.TakeWhile(i => i != string.Empty))
{
    var parts = line.Split(": ");

    wires.Add(parts[0], parts[1] == "1");
}

var linesToProcess = input.Skip(wires.Count + 1).ToList();

while (linesToProcess.Any())
{
    foreach (var line in linesToProcess.ToArray())
    {
        var parts = line.Split(" ");

        if (wires.ContainsKey(parts[0]) && wires.ContainsKey(parts[2]))
        {
            var left = wires[parts[0]];
            var right = wires[parts[2]];
            var value = parts[1] switch
            {
                "AND" => left & right,
                "XOR" => left ^ right,
                "OR" => left | right
            };

            wires.Add(parts[4], value);

            linesToProcess.Remove(line);
        }
    }
}

var binary = "";

foreach (var wire in wires.OrderBy(w => w.Key))
{
    Console.WriteLine($"{wire.Key}: {wire.Value}");
}

foreach (var zWire in wires.Where(w => w.Key.StartsWith("z")).OrderByDescending(w => w.Key))
{
    binary += zWire.Value == true ? "1" : "0";
}

Console.WriteLine($"zWires = {binary}");

var total = Convert.ToInt64(binary, 2);

Console.WriteLine($"Total = {total}");