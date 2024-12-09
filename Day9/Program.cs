using System.Collections.Generic;

var input = File.ReadAllLines("Input.txt")[0];

var blocks = new List<Block>();
var fullString = new List<string>();
var fileId = 0;

for (var i = 0; i < input.Length; i++)
{
    if (i % 2 == 0)
    {
        fullString.AddRange(Enumerable.Repeat(fileId.ToString(), int.Parse(input[i].ToString())));
        blocks.Add(new Block() { FileId = fileId, Length = int.Parse(input[i].ToString()) });

        fileId += 1;
    }
    else
    {
        fullString.AddRange(Enumerable.Repeat(".", int.Parse(input[i].ToString())));
        blocks.Add(new Block() { FileId = -1, Length = int.Parse(input[i].ToString()) });
    }
}

foreach (var s in fullString)
{
    Console.Write(s);
}

Console.WriteLine();

//var replaceAt = fullString.IndexOf(".");

//while (replaceAt != -1)
//{
//    var toMove = fullString.Last();
//    fullString.RemoveAt(fullString.Count - 1);

//    if (toMove != ".")
//    {
//        fullString[replaceAt] = toMove;

//        replaceAt = fullString.IndexOf(".");
//    }
//}

for (var i = blocks.Count - 1; i >= 0; i--)
{
    var blockToMove = blocks[i];

    if (blockToMove.Moved)
    {
        continue;
    }

    if (blockToMove.FileId != -1)
    {
        var spaceToInsert = blocks.FirstOrDefault(b => b.FileId == -1 && b.Length >= blockToMove.Length);

        if (spaceToInsert is not null)
        {
            var positionToInsert = blocks.IndexOf(spaceToInsert);

            if (positionToInsert > i)
            {
                continue;
            }

            blocks.Insert(positionToInsert, new Block() { FileId = blockToMove.FileId, Length = blockToMove.Length, Moved = true });

            blockToMove.FileId = -1;

            spaceToInsert.Length -= blockToMove.Length;
        }
    }
}

fullString = new List<string>();

foreach (var block in blocks)
{
    fullString.AddRange(Enumerable.Repeat(block.FileId == -1 ? "." : block.FileId.ToString(), block.Length));
}

foreach (var s in fullString)
{
    Console.Write(s);
}

Console.WriteLine();

var total = 0L;

for (var i = 0; i < fullString.Count; i++)
{
    if (fullString[i] != ".")
    {
        total += (i * int.Parse(fullString[i]));
    }
}

Console.WriteLine();
Console.WriteLine($"Total = {total}");

class Block
{
    public int FileId { get; set; }

    public int Length { get; set; }

    public bool Moved { get; set; } = false;
}