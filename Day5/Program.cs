var input = File.ReadAllLines("Input.txt");

var rules = input.TakeWhile(i => i != string.Empty).Select(i => i.Split('|').Select(r => int.Parse(r)).ToArray()).ToArray();
var orders = input.Skip(rules.Length + 1).Select(i => i.Split(',').Select(o => int.Parse(o)).ToArray()).ToArray();

var total = 0;
var validOrders = new List<int[]>();

foreach (var order in orders)
{
    var invalid = false;

    for (var i = 0; i < order.Length; i++)
    {
        var orderRules = rules.Where(r => r[0] == order[i]).ToArray();
        var invalidPreviousPages = orderRules.Select(or => or[1]).ToArray();

        if (order.Take(i).Intersect(invalidPreviousPages).Any())
        {
            invalid = true;
            break;
        }
    }

    if (!invalid)
    {
        total += order[(order.Length / 2)];
    }
}

Console.WriteLine($"Total = {total}");
