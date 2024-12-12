var input = File.ReadAllLines("Input.txt");
var grid = input.Select(i => i.ToCharArray()).ToArray();
var plots = new Plot[grid.Length][];

for (var row = 0; row < grid.Length; row++)
{
    plots[row] = new Plot[grid[row].Length];

    for (var col = 0; col < grid[row].Length; col++)
    {
        plots[row][col] = new Plot()
        {
            Plant = grid[row][col],
            Row = row,
            Col = col,
        };
    }
}

for (var row = 0; row < plots.Length; row++)
{
    for (var col = 0; col < plots[row].Length; col++)
    {
        var plot = plots[row][col];

        if (row > 0)
        {
            CheckNextPlot(plot, plots[row - 1][col]);
        }

        if (row < (grid.Length - 1))
        {
            CheckNextPlot(plot, plots[row + 1][col]);
        }

        if (col > 0)
        {
            CheckNextPlot(plot, plots[row][col - 1]);
        }

        if (col < (grid[row].Length - 1))
        {
            CheckNextPlot(plot, plots[row][col + 1]);
        }
    }
}

var groups = new List<IEnumerable<Plot>>();

for (var row = 0; row < plots.Length; row++)
{
    for (var col = 0; col < plots[row].Length; col++)
    {
        var plot = plots[row][col];

        if (!groups.SelectMany(g => g).Contains(plot))
        {
            var group = new List<Plot>();

            groups.Add(group);
            plot.BuildGroup(group);
        }
    }
}

var total = 0;

foreach (var group in groups)
{
    //var fences = group.Sum(g => g.Fences.Count());
    //var cost = group.Count() * fences;

    //Console.WriteLine($"Group {group.First().Plant} = Area:{group.Count()} * Parameter:{fences} = {cost}");

    var edges = FindEdges(group.SelectMany(g => g.Fences).ToArray());
    var cost = group.Count() * edges;

    Console.WriteLine($"Group {group.First().Plant} = Area:{group.Count()} * Edges:{edges} = {cost}");

    total += cost;
}

Console.WriteLine($"Total = {total}");

void CheckNextPlot(Plot currentPlot, Plot nextPlot)
{
    if (currentPlot.Plant == nextPlot.Plant)
    {
        currentPlot.AddNext(nextPlot);
        nextPlot.AddNext(currentPlot);
    }
}

int FindEdges(IEnumerable<Fence> fences)
{
    var remainingEdges = new List<Fence>(fences);

    for (var i = 0; i < fences.Count(); i++)
    {
        var fence = fences.ElementAt(i);
        var nextRow = fence.Row;
        var nextCol = fence.Col;

        if (fence.Position == 'T')
        {
            nextCol += 1;
        }
        else if (fence.Position == 'R')
        {
            nextRow += 1;
        }
        else if (fence.Position == 'B')
        {
            nextCol -= 1;
        }
        else if (fence.Position == 'L')
        {
            nextRow -= 1;
        }

        var nextFence = fences.FirstOrDefault(f => f.Position == fence.Position && f.Row == nextRow && f.Col == nextCol);

        if (nextFence is not null)
        {
            remainingEdges.Remove(fence);
        }
    }

    return remainingEdges.Count();
}

class Plot
{
    public char Plant { get; init; }

    public int Row { get; init; }

    public int Col { get; init; }

    public List<Plot> Related { get; } = new List<Plot>();

    public IEnumerable<Fence> Fences 
    {
        get
        {
            var fences = new List<Fence>();

            if (!Related.Any(r => r.Row == Row - 1 && r.Col == Col))
            {
                fences.Add(new Fence() { Row = Row, Col = Col, Position = 'T' });
            }

            if (!Related.Any(r => r.Row == Row && r.Col == Col + 1))
            {
                fences.Add(new Fence() { Row = Row, Col = Col, Position = 'R' });
            }

            if (!Related.Any(r => r.Row == Row + 1 && r.Col == Col))
            {
                fences.Add(new Fence() { Row = Row, Col = Col, Position = 'B' });
            }

            if (!Related.Any(r => r.Row == Row && r.Col == Col - 1))
            {
                fences.Add(new Fence() { Row = Row, Col = Col, Position = 'L' });
            }

            return fences;
        }
    }

    public void AddNext(Plot plot)
    {
        if (!Related.Contains(plot))
        {
            Related.Add(plot);
        }
    }

    public void BuildGroup(List<Plot> inGroup)
    {
        if (!inGroup.Contains(this))
        {
            inGroup.Add(this);

            foreach (var plot in Related)
            {
                plot.BuildGroup(inGroup);
            }
        }
    }
}

class Fence
{
    public int Row { get; init; }

    public int Col { get; init; }

    public char Position { get; init; }
}