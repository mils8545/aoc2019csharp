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
        int DEPTH = lines.First().Length / (WIDTH * HEIGHT);
        
        int minZeroes = int.MaxValue;
        int minZeroesLayer = 0;
        int minZeroesLayerOnes = 0;
        int minZeroesLayerTwos = 0;

        for (int layer = 0; layer < DEPTH; layer++)
        {
            int[] layerImage = lines.First().Substring(layer * WIDTH * HEIGHT, WIDTH * HEIGHT).Select(c => int.Parse(c.ToString())).ToArray();
            int zeroes = layerImage.Where(x => x == 0).Count();
            if (zeroes < minZeroes)
            {
                minZeroes = zeroes;
                minZeroesLayer = layer;
                minZeroesLayerOnes = layerImage.Where(x => x == 1).Count();
                minZeroesLayerTwos = layerImage.Where(x => x == 2).Count();
            }
        }

        int result = minZeroesLayerOnes * minZeroesLayerTwos;
        return ($"The product of 1s and 2s in the layer with the least 0s is {result}");
    }

    static string part2(System.Collections.Generic.IEnumerable<String> lines)
    {
        const int WIDTH = 25;
        const int HEIGHT = 6;
        int DEPTH = lines.First().Length / (WIDTH * HEIGHT);
        
        int minZeroes = int.MaxValue;
        int minZeroesLayer = 0;
        int minZeroesLayerOnes = 0;
        int minZeroesLayerTwos = 0;

        int[] finalImage = new int[WIDTH * HEIGHT];

        for (int pixel = 0; pixel < WIDTH * HEIGHT; pixel++)
        {
            finalImage[pixel] = 2;
        }

        for (int layer = 0; layer < DEPTH; layer++)
        {
            int[] layerImage = lines.First().Substring(layer * WIDTH * HEIGHT, WIDTH * HEIGHT).Select(c => int.Parse(c.ToString())).ToArray();
            for (int pixel = 0; pixel < WIDTH * HEIGHT; pixel++)
            {
                if (finalImage[pixel] == 2)
                {
                    finalImage[pixel] = layerImage[pixel];
                }
            }
        }

        for (int row = 0; row < HEIGHT; row++)
        {
            for (int col = 0; col < WIDTH; col++)
            {
                if (finalImage[row * WIDTH + col] == 0)
                {
                    Console.Write("  ");
                }
                else
                {
                    Console.Write("██");
                }
            }
            Console.WriteLine("");
        }

        return ("Read the code above");
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