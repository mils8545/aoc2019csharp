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

    static bool part1Valid(int num)
    {
        int[] digits = num.ToString().Select(c => int.Parse(c.ToString())).ToArray();
        bool adjacent = false;
        for (int i = 0; i < digits.Length - 1; i++)
        {
            if (digits[i] == digits[i+1])
            {
                adjacent = true;
            }
        }
        bool increasing = true;
        for (int i = 0; i < digits.Length - 1; i++)
        {
            if (digits[i] > digits[i+1])
            {
                increasing = false;
            }
        }
        return adjacent && increasing;
    }

    static bool part2Valid(int num)
    {
        int[] digits = num.ToString().Select(c => int.Parse(c.ToString())).ToArray();
        bool adjacent = false;
        for (int i = 1; i < digits.Length - 2; i++)
        {
            int digit = digits[i];
            if ((digit == digits[i+1]) && (digit != digits[i-1]) && (digit != digits[i+2]))
            {
                adjacent = true;
            }
        }
        if ((digits[0] == digits[1]) && (digits[0] != digits[2]))
        {
            adjacent = true;
        }
        if ((digits[digits.Length - 1] == digits[digits.Length - 2]) && (digits[digits.Length - 1] != digits[digits.Length - 3]))
        {
            adjacent = true;
        }

        bool increasing = true;
        for (int i = 0; i < digits.Length - 1; i++)
        {
            if (digits[i] > digits[i+1])
            {
                increasing = false;
            }
        }
        return adjacent && increasing;
    }

    static string part1(System.Collections.Generic.IEnumerable<String> lines)
    {
        int startingNumber = int.Parse(lines.First().Split('-')[0]);
        int endingNumber = int.Parse(lines.First().Split('-')[1]);

        int count = 0;
        for (int i = startingNumber; i <= endingNumber; i++)
        {
            if (part1Valid(i))
            {
                count++;
            }
        }

        return $"There are {count} valid codes.";
    }

    static string part2(System.Collections.Generic.IEnumerable<String> lines)
    {
    int startingNumber = int.Parse(lines.First().Split('-')[0]);
        int endingNumber = int.Parse(lines.First().Split('-')[1]);

        int count = 0;
        for (int i = startingNumber; i <= endingNumber; i++)
        {
            if (part2Valid(i))
            {
                count++;
            }
        }

        return $"There are {count} valid codes.";
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