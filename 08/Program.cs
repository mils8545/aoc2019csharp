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

class Program
{
    static string part1(System.Collections.Generic.IEnumerable<String> lines)
    {
        const int WIDTH = 25;
        const int HEIGHT = 6;
        const int LAYER_SIZE = WIDTH * HEIGHT;
        int DEPTH = lines.First().Length / LAYER_SIZE;

        int[,,]pixels = new int[DEPTH, HEIGHT, WIDTH];
        for (int i = 0; i < lines.First().Length; i++)
        {
            int depth = i / LAYER_SIZE;
            int height = (i % LAYER_SIZE) / WIDTH;
            int width = (i % LAYER_SIZE) % WIDTH;
            pixels[depth, height, width] = int.Parse(lines.First()[i].ToString());
        }

        int minZeroes = int.MaxValue;
        int minZeroesLayer = 0;
        int minZeroesLayerOnes = 0;
        int minZeroesLayerTwos = 0;

        for (int i = 0; i < DEPTH; i++)
        {
            int zeroes = 0;
            int ones = 0;
            int twos = 0;
            for (int j = 0; j < HEIGHT; j++)
            {
                for (int k = 0; k < WIDTH; k++)
                {
                    if (pixels[i, j, k] == 0)
                    {
                        zeroes++;
                    }
                    else if (pixels[i, j, k] == 1)
                    {
                        ones++;
                    }
                    else if (pixels[i, j, k] == 2)
                    {
                        twos++;
                    }
                }
            }
            if (zeroes < minZeroes)
            {
                minZeroes = zeroes;
                minZeroesLayer = i;
                minZeroesLayerOnes = ones;
                minZeroesLayerTwos = twos;
            }
        }

        return $"Layer {minZeroesLayer} has {minZeroesLayerOnes} ones and {minZeroesLayerTwos} twos. Answer: {minZeroesLayerOnes * minZeroesLayerTwos}";
    }

    static string part2(System.Collections.Generic.IEnumerable<String> lines)
    {
        const int WIDTH = 25;
        const int HEIGHT = 6;
        const int LAYER_SIZE = WIDTH * HEIGHT;
        int DEPTH = lines.First().Length / LAYER_SIZE;

        int[,,]pixels = new int[DEPTH, HEIGHT, WIDTH];
        for (int i = 0; i < lines.First().Length; i++)
        {
            int depth = i / LAYER_SIZE;
            int height = (i % LAYER_SIZE) / WIDTH;
            int width = (i % LAYER_SIZE) % WIDTH;
            pixels[depth, height, width] = int.Parse(lines.First()[i].ToString());
        }

        int[,] finalPixels = new int[HEIGHT, WIDTH];
        for (int y = 0; y < HEIGHT; y++)
        {
            for (int x = 0; x < WIDTH; x++)
            {
                int layer = 0;
                while (pixels[layer, y, x] == 2)
                {
                    layer++;
                }
                finalPixels[y, x] = pixels[layer, y, x];
            }
        }

        for (int y = 0; y < HEIGHT; y++)
        {
            for (int x = 0; x < WIDTH; x++)
            {
                if (finalPixels[y, x] == 0)
                {
                    Console.Write(" ");
                }
                else
                {
                    Console.Write("@");
                }
            }
            Console.WriteLine("");
        }

        return $"Enter the Code on the Screen";
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