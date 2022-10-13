class Helpers
{
    static public System.Collections.Generic.IEnumerable<String> ReadFromFile(string fileName)
    {
        var lines = System.IO.File.ReadAllLines(fileName);

        return lines;
    }

    static public string GetFileName()
    {
        string fileName = "input.txt";
        try
        {
            string[] files = Directory.GetFiles(@".\", "*.txt");
            for (int i = 0; i < files.Length; i++)
            {
                Console.WriteLine($"{i} : {Path.GetFileName(files[i])}");
            }
            Console.Write("Enter file number: ");
            int fileNumber = int.Parse(Console.ReadLine()!);
            fileName = files[fileNumber];
            Console.WriteLine("");
        }
        catch (Exception e)
        {
            Console.WriteLine("The process failed: {0}", e.ToString());
        }
        return fileName;
    }
}

class ComputerState
{
    private int[] memory;
    private int programCounter;
    private Dictionary<int, int> opLength =
        new Dictionary<int, int>()
        {
            { 1, 4 },
            { 2, 4 },
            { 3, 2 },
            { 4, 2 },
            { 5, 3 },
            { 6, 3 },
            { 7, 4 },
            { 8, 4 },
            { 99, 0 }
        };
    public bool completed = false;

    public ComputerState(string programString)
    {
        memory = programString.Split(',').Select(int.Parse).ToArray();
        programCounter = 0;
    }

    public Stack<int> Run(Queue<int> inputs)
    {
        Stack<int> outputs = new Stack<int> {};
        if (memory != null) {
            while (memory[programCounter] != 99)
            {
                int opcode = memory[programCounter] % 100;
                int currentOpLength = opLength[opcode];
                int[] parameters = new int[currentOpLength - 1];
                int[] modes = new int[currentOpLength - 1];

                for (int i = 0; i < currentOpLength - 1; i++)
                {
                    modes[i] = (memory[programCounter] / (int)Math.Pow(10, i + 2)) % 10;
                    parameters[i] = memory[programCounter + i + 1];
                }

                int num1;
                int num2;

                switch (opcode)
                {
                    case 1:
                        num1 = (modes[0] == 0) ? memory[parameters[0]] : parameters[0];
                        num2 = (modes[1] == 0) ? memory[parameters[1]] : parameters[1];
                        memory[parameters[2]] = num1 + num2;
                        break;
                    case 2:
                        num1 = (modes[0] == 0) ? memory[parameters[0]] : parameters[0];
                        num2 = (modes[1] == 0) ? memory[parameters[1]] : parameters[1];
                        memory[parameters[2]] = num1 * num2;
                        break;
                    case 3:
                        if (inputs.Count == 0)
                        {
                            return outputs;
                        }
                        else
                        {
                            memory[parameters[0]] = inputs.Dequeue();
                        }
                        break;
                    case 4:
                        num1 = (modes[0] == 0) ? memory[parameters[0]] : parameters[0];
                        outputs.Push(num1);
                        break;
                    case 5:
                        num1 = (modes[0] == 0) ? memory[parameters[0]] : parameters[0];
                        num2 = (modes[1] == 0) ? memory[parameters[1]] : parameters[1];
                        if (num1 != 0)
                        {
                            programCounter = num2 - currentOpLength;
                        }
                        break;
                    case 6:
                        num1 = (modes[0] == 0) ? memory[parameters[0]] : parameters[0];
                        num2 = (modes[1] == 0) ? memory[parameters[1]] : parameters[1];
                        if (num1 == 0)
                        {
                            programCounter = num2 - currentOpLength;
                        }
                        break;
                    case 7:
                        num1 = (modes[0] == 0) ? memory[parameters[0]] : parameters[0];
                        num2 = (modes[1] == 0) ? memory[parameters[1]] : parameters[1];
                        if (num1 < num2)
                        {
                            memory[parameters[2]] = 1;
                        } else
                        {
                            memory[parameters[2]] = 0;
                        }
                        break;
                    case 8:
                        num1 = (modes[0] == 0) ? memory[parameters[0]] : parameters[0];
                        num2 = (modes[1] == 0) ? memory[parameters[1]] : parameters[1];
                        if (num1 == num2)
                        {
                            memory[parameters[2]] = 1;
                        } else
                        {
                            memory[parameters[2]] = 0;
                        }
                        break;
                    default:
                        throw new Exception($"Invalid opcode {opcode}");
                }
                programCounter += currentOpLength;
            }

        }
        completed = true;
        return outputs;
    }
}

class Program
{
    static string part1(System.Collections.Generic.IEnumerable<String> lines)
    {
        int maxOutput = 0;
        int[] phases = new int[] { 0, 1, 2, 3, 4 };

        foreach (IList<int> phaseSettings in new Combinatorics.Collections.Permutations<int>(phases))
        {
            ComputerState[] amplifiers = new ComputerState[] {
                new ComputerState(lines.First()),
                new ComputerState(lines.First()),
                new ComputerState(lines.First()),
                new ComputerState(lines.First()),
                new ComputerState(lines.First())
            };

            int output = 0;

            for (int i = 0; i < 5; i++)
            {
                Queue<int> inputs = new Queue<int>();
                inputs.Enqueue(phaseSettings[i]);
                inputs.Enqueue(output);
                output = amplifiers[i].Run(inputs).Pop();
            }
            while (amplifiers[4].completed == false)
            {
                for (int i = 0; i < 5; i++)
                {
                    Queue<int> inputs = new Queue<int>();
                    inputs.Enqueue(output);
                    output = amplifiers[i].Run(inputs).Pop();
                }
            }
            if (output > maxOutput)
            {
                maxOutput = output;
            }
        }

        return $"The maximum output is {maxOutput}.";
    }

    static string part2(System.Collections.Generic.IEnumerable<String> lines)
    {
        int maxOutput = 0;
        int[] phases = new int[] { 5, 6, 7, 8, 9 };

        foreach (IList<int> phaseSettings in new Combinatorics.Collections.Permutations<int>(phases))
        {
            ComputerState[] amplifiers = new ComputerState[] {
                new ComputerState(lines.First()),
                new ComputerState(lines.First()),
                new ComputerState(lines.First()),
                new ComputerState(lines.First()),
                new ComputerState(lines.First())
            };

            int output = 0;

            for (int i = 0; i < 5; i++)
            {
                Queue<int> inputs = new Queue<int>();
                inputs.Enqueue(phaseSettings[i]);
                inputs.Enqueue(output);
                output = amplifiers[i].Run(inputs).Pop();
            }
            while (amplifiers[4].completed == false)
            {
                for (int i = 0; i < 5; i++)
                {
                    Queue<int> inputs = new Queue<int>();
                    inputs.Enqueue(output);
                    output = amplifiers[i].Run(inputs).Pop();
                }
            }
            if (output > maxOutput)
            {
                maxOutput = output;
            }
        }

        return $"The maximum output is {maxOutput}.";
    }

    static void Main(string[] args)
    {
        string fileName = Helpers.GetFileName();
        var lines = Helpers.ReadFromFile(fileName);

        var watch1 = System.Diagnostics.Stopwatch.StartNew();
        string part1Answer = part1(lines);
        watch1.Stop();

        Console.WriteLine($"Part 1 took {watch1.ElapsedMilliseconds} ms");
        Console.WriteLine($"Part 1: {part1Answer}");

        var watch2 = System.Diagnostics.Stopwatch.StartNew();
        string part2Answer = part2(lines);
        watch2.Stop();

        Console.WriteLine($"Part 2 took {watch2.ElapsedMilliseconds} ms");
        Console.WriteLine($"Part 2: {part2Answer}");
    }
}