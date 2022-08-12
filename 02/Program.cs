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

class Computer
{
    private int[] Program;

    public Computer(string programString)
    {
        Program = programString.Split(',').Select(int.Parse).ToArray();
    }

    public int Run(int noun = -1, int verb = -1)
    {
        int programCounter = 0;
        int[] memory;
        if (Program.Length > 0)
        {
            memory = Program.Clone() as int[];
        } else
        {
            memory = new int[0];
            return 0;
        }
        if (noun >= 0)
        {
            memory[1] = noun;
        }
        if (verb >= 0)
        {
            memory[2] = verb;
        }
    
        while (memory[programCounter] != 99)
        {
            int opcode = memory[programCounter];
            int param1 = memory[programCounter + 1];
            int param2 = memory[programCounter + 2];
            int param3 = memory[programCounter + 3];
            switch (opcode)
            {
                case 1:
                    memory[param3] = memory[param1] + memory[param2];
                    break;
                case 2:
                    memory[param3] = memory[param1] * memory[param2];
                    break;
                default:
                    throw new Exception($"Invalid opcode {opcode}");
            }
            programCounter += 4;
        }

        return memory[0];
    }
}

class Program
{

    static string part1(System.Collections.Generic.IEnumerable<String> lines)
    {
        Computer computer = new Computer(lines.ElementAt(0));
        
        int result = computer.Run(12, 2);

        return $"The program result is {result}.";
    }

    static string part2(System.Collections.Generic.IEnumerable<String> lines)
    {
        const int TARGET = 19690720;
        Computer computer = new Computer(lines.ElementAt(0));
        for (int noun = 0; noun < 100; noun++)
        {
            for (int verb = 0; verb < 100; verb++)
            {
                int result = computer.Run(noun, verb);
                if (result == TARGET)
                {
                    return $"The program that produces the result is {100 * noun + verb}.";
                }
            }
        }

        return "No program found";
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