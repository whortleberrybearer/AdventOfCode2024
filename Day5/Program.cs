
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

    if (invalid)
    {
        total += Reorder(order);
    }

    //if (!invalid)
    //{
    //    total += order[(order.Length / 2)];
    //}
}

Console.WriteLine($"Total = {total}");

int Reorder(int[] order)
{
    var orderRules = new List<int[]>();

    for (var i = 0; i < order.Length; i++)
    {
        orderRules.AddRange(rules.Where(r => r[0] == order[i]));
    }

    foreach (var orderRule in orderRules.ToArray())
    {
        if (!order.Contains(orderRule[1]))
        {
            orderRules.Remove(orderRule);
        }
    }

    var orderGroups = orderRules.GroupBy(or => or[1], or => or[0]).OrderBy(g => g.Count());

    // This is a bit of a cheat, as the first item on the list will not be present in the group.
    return orderGroups.ElementAt((orderGroups.Count() / 2) - 1).Key;
}
