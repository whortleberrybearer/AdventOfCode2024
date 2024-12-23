var input = File.ReadAllLines("Input.txt");
var clientConnections = new Dictionary<string, List<string>>();

foreach (var line in input)
{
    var clients = line.Split('-');

    foreach (var client in clients)
    {
        var otherClients = clients.Except(new string[] { client });

        if (clientConnections.TryGetValue(client, out var connections))
        {
            connections.AddRange(otherClients);
        }
        else
        {
            clientConnections.Add(client, new List<string>(otherClients));
        }
    }
}

var options = new List<string>();

foreach (var connection in clientConnections)
{
    foreach (var connection1 in connection.Value.Where(c => c != connection.Key))
    {
        foreach (var connection2 in clientConnections[connection1].Where(c => c != connection.Key && c != connection1))
        {
            if (clientConnections[connection2].Contains(connection.Key))
            {
                var group = string.Join(",", new string[] { connection.Key, connection1, connection2 }.Order());

                if (!options.Contains(group))
                {
                    options.Add(group);
                }
            }
        }
    }
}

var total = options.Count(o => o.StartsWith("t") || o.Contains(",t"));

Console.WriteLine($"Total = {total}");
