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

    static int fuelCalc(int mass)
    {
        return Math.Max(0, (mass / 3) - 2);
    }
    static string part1(System.Collections.Generic.IEnumerable<String> lines)
    {
        int totalFuel = 0;
        foreach (string line in lines)
        {
            totalFuel += fuelCalc(int.Parse(line));
        }
        return $"You require a fuel amount of {totalFuel}.";
    }

    static string part2(System.Collections.Generic.IEnumerable<String> lines)
    {
        int totalFuel = 0;
        foreach (string line in lines)
        {
            int calcedFuel = fuelCalc(int.Parse(line));
            while (calcedFuel > 0)
            {
                totalFuel += calcedFuel;
                calcedFuel = fuelCalc(calcedFuel);
            }
        }

        return $"You require a fuel amount of {totalFuel}.";
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