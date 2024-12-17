
var input = File.ReadAllLines("Input.txt");

var registerA = 0;
var registerB = 0;
var registerC = 0;
var program = new int[0];
var output = new List<int>();

foreach (var line in input)
{
    if (line.StartsWith("Register A"))
    {
        registerA = int.Parse(line.Replace("Register A: ", string.Empty));
    }
    else if (line.StartsWith("Register B"))
    {
        registerB = int.Parse(line.Replace("Register B: ", string.Empty));
    }
    else if(line.StartsWith("Register C"))
    {
        registerC = int.Parse(line.Replace("Register C: ", string.Empty));
    }
    else if (line.StartsWith("Program"))
    {
        program = line.Replace("Program: ", string.Empty).Split(',').Select(s => int.Parse(s)).ToArray();
    }
}

for (var i = 0; i < program.Length; i += 2)
{
    var opcode = program[i];
    var operand = program[i + 1];

    if (opcode == 0)
    {
        registerA /= (int)Math.Pow(2, GetComboOperand(operand));
    }
    else if (opcode == 1)
    {
        registerB ^= operand;
    }
    else if (opcode == 2)
    {
        registerB = GetComboOperand(operand) % 8;
    }
    else if (opcode == 3)
    {
        if (registerA != 0)
        {
            // Jump to position, and -2 so the next iteration of the loop hits it.
            i = operand - 2;
        }
    }
    else if (opcode == 4)
    {
        registerB ^= registerC;
    }
    else if (opcode == 5)
    {
        output.Add(GetComboOperand(operand) % 8);
    }
    else if (opcode == 6)
    {
        registerB = registerA / (int)Math.Pow(2, GetComboOperand(operand));

    }
    else if (opcode == 7)
    {
        registerC = registerA / (int)Math.Pow(2, GetComboOperand(operand));
    }
}

Console.WriteLine(string.Join(",", output));

//var total = 0;

//Console.WriteLine($"Total = {total}");

int GetComboOperand(int operand)
{
    if (operand == 4)
    {
        return registerA;
    }
    else if (operand == 5)
    {
        return registerB;
    }
    else if (operand == 6)
    {
        return registerC;
    }

    return operand;
}