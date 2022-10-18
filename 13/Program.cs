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
    public long[] memory;
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

    public Queue<long> Run(Queue<long> inputs)
    {
        Queue<long> outputs = new Queue<long> {};
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
                        outputs.Enqueue(memory[addresses[0]]);
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

class Tile
{
    public int X { get; set; }
    public int Y { get; set; }
    public int TileId { get; set; }
    public Tile(int x, int y, int tileId)
    {
        X = x;
        Y = y;
        TileId = tileId;
    }

    public static implicit operator string(Tile c) => $"({c.X}, {c.Y}, {c.TileId})";
}

class Program
{
    static string part1(System.Collections.Generic.IEnumerable<String> lines)
    {
        ComputerState computer = new ComputerState(lines.First());
        Queue<long> outputs = computer.Run(new Queue<long>());
        List<Tile> tiles = new List<Tile>();
        while (outputs.Count > 0)
        {
            int x = (int)outputs.Dequeue();
            int y = (int)outputs.Dequeue();
            int tileId = (int)outputs.Dequeue();
            tiles.Add(new Tile(x, y, tileId));
        }
        return $"There are {tiles.Where(t => t.TileId == 2).Count().ToString()} blocks on the screen.";
    }

    static string part2(System.Collections.Generic.IEnumerable<String> lines)
    {
        ComputerState computer = new ComputerState(lines.First());
        computer.memory[0] = 2;
        int input = 0;
        const int SCREENWIDTH = 44;
        const int SCREENHEIGHT = 24;
        int score = 0;
        int paddleX = 0;
        int ballX = 0;
        int[,] screen = new int[SCREENWIDTH, SCREENHEIGHT];
        while (!computer.completed)
        {
            Queue<long> inputs = new Queue<long>();
            inputs.Enqueue((long)input);
            Queue<long> outputs = computer.Run(inputs);
            List<Tile> tiles = new List<Tile>();
            while (outputs.Count > 0)
            {
                int x = (int)outputs.Dequeue();
                int y = (int)outputs.Dequeue();
                int tileId = (int)outputs.Dequeue();
                tiles.Add(new Tile(x, y, tileId));
            }
            foreach (Tile tile in tiles)
            {
                if (tile.X != -1)
                {
                    screen[tile.X, tile.Y] = tile.TileId;
                    if (tile.TileId == 3)
                    {
                        paddleX = tile.X;
                    }
                    else if (tile.TileId == 4)
                    {
                        ballX = tile.X;
                    }
                }
                else
                {
                    score = tile.TileId;
                }
            }
            // // Uncomment to see the game
            // Console.Clear();
            // for (int y = 0; y < SCREENHEIGHT; y++)
            // {
            //     for (int x = 0; x < SCREENWIDTH; x++)
            //     {
            //         Console.Write(screen[x, y] switch
            //         {
            //             0 => " ",
            //             1 => "█",
            //             2 => "▒",
            //             3 => "▀",
            //             4 => "o",
            //             _ => throw new Exception("Invalid tile")
            //         });
            //     }
            //     Console.WriteLine("");
            // }

            // Console.WriteLine($"Score: {score}");

            if (paddleX < ballX)
            {
                input = 1;
            }
            else if (paddleX > ballX)
            {
                input = -1;
            }
            else
            {
                input = 0;
            }

            // // Uncomment to play the game manually - a goes left, d goes right
            // string inputRead = Console.ReadLine();

            // if (inputRead == "a")
            // {
            //     input = -1;
            // }
            // else if (inputRead == "d")
            // {
            //     input = 1;
            // }
            // else
            // {
            //     input = 0;
            // }

        }
        return $"The final score is {score}.";
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