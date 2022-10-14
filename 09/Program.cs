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
    private long[] memory;
    private int programCounter;
    private int offset;
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
            { 9, 2 },
            { 99, 0 }
        };
    public bool completed = false;

    public ComputerState(string programString)
    {
        memory = programString.Split(',').Select(long.Parse).ToArray();
        programCounter = 0;
    }

    public bool MemoryCheck(int address)
    {
        if (address >= memory.Length)
        {
            long[] newMemory = new long[address + 1];
            memory.CopyTo(newMemory, 0);
            memory = newMemory;
            return true;
        }
        return false;
    }

    public Stack<long> Run(Queue<long> inputs)
    {
        Stack<long> outputs = new Stack<long> {};
        if (memory != null) {
            while (memory[programCounter] != 99)
            {
                int opcode = (int)(memory[programCounter] % 100);
                int currentOpLength = (int)opLength[opcode];
                int[] parameters = new int[currentOpLength - 1];
                int[] modes = new int[currentOpLength - 1];
                int[] addresses = new int[currentOpLength - 1];

                for (int i = 0; i < currentOpLength - 1; i++)
                {
                    modes[i] = (int)(memory[programCounter] / (int)Math.Pow(10, i + 2)) % 10;
                    parameters[i] = (int)memory[programCounter + i + 1];
                    addresses[i] = modes[i] switch
                    {
                        0 => parameters[i],
                        1 => programCounter + i + 1,
                        2 => parameters[i] + offset,
                        _ => throw new Exception("Invalid mode")
                    };
                    MemoryCheck(addresses[i]);
                }

                switch (opcode)
                {
                    case 1:
                        memory[addresses[2]] = memory[addresses[0]] + memory[addresses[1]];
                        break;
                    case 2:
                        memory[addresses[2]] = memory[addresses[0]] * memory[addresses[1]];
                        break;
                    case 3:
                        if (inputs.Count == 0)
                        {
                            return outputs;
                        }
                        else
                        {
                            memory[addresses[0]] = inputs.Dequeue();
                        }
                        break;
                    case 4:
                        outputs.Push(memory[addresses[0]]);
                        break;
                    case 5:
                        if (memory[addresses[0]] != 0)
                        {
                            programCounter = (int)memory[addresses[1]];
                            continue;
                        }
                        break;
                    case 6:
                        if (memory[addresses[0]] == 0)
                        {
                            programCounter = (int)memory[addresses[1]];
                            continue;
                        }
                        break;
                    case 7:
                        memory[addresses[2]] = memory[addresses[0]] < memory[addresses[1]] ? 1 : 0;
                        break;
                    case 8:
                        memory[addresses[2]] = memory[addresses[0]] == memory[addresses[1]] ? 1 : 0;
                        break;
                    case 9:
                        offset += (int)memory[addresses[0]];
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
        ComputerState computer = new ComputerState(lines.First());
        Queue<long> inputs = new Queue<long>();
        inputs.Enqueue(1);
        long output = computer.Run(inputs).Pop();

        return $"The BOOST keycode of the validation program is {output}.";
    }

    static string part2(System.Collections.Generic.IEnumerable<String> lines)
    {
        ComputerState computer = new ComputerState(lines.First());
        Queue<long> inputs = new Queue<long>();
        inputs.Enqueue(2);
        long output = computer.Run(inputs).Pop();

        return $"The coordinates of the distress signal are {output}.";
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