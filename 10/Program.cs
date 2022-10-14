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

class Meteor
{
    public int X;
    public int Y;

    public Meteor(int x, int y)
    {
        X = x;
        Y = y;
    }

    public double GetAngle(Meteor other)
    {
        double angle = Math.Atan2(other.Y - Y, other.X - X) + Math.PI / 2;
        if (angle < 0)
        {
            angle += 2 * Math.PI;
        }
        // angle = angle * 180 / Math.PI + 90;
        return angle;

        // angle = math.atan2(other.y - self.y, other.x - self.x)
        // angle = math.degrees(angle) + 90
    }

    public double GetDistance(Meteor other)
    {
        return Math.Abs(other.X - X) + Math.Abs(other.Y - Y);
    }

    public static implicit operator string(Meteor m) => $"({m.X}, {m.Y})";
}

class Program
{

    static string part1(System.Collections.Generic.IEnumerable<String> lines)
    {
        List<Meteor> meteors = new List<Meteor>();
        for (int y = 0; y < lines.Count(); y++)
        {
            for (int x = 0; x < lines.First().Length; x++)
            {
                if (lines.ElementAt(y)[x] == '#')
                {
                    meteors.Add(new Meteor(x, y));
                }
            }
        }

        int max = 0;
        int bestIndex = 0;

        for (int i = 0; i < meteors.Count; i++)
        {
            HashSet<double> angles = new HashSet<double>();
            for (int j = 0; j < meteors.Count; j++)
            {
                if (i != j)
                {
                    angles.Add(meteors[i].GetAngle(meteors[j]));
                }
            }
            if (angles.Count > max)
            {
                max = angles.Count;
                bestIndex = i;
            }
        }

        return $"The best station position sees {max} meteors at {(string)meteors[bestIndex]}.";
    }

    static string part2(System.Collections.Generic.IEnumerable<String> lines)
    {
        List<Meteor> meteors = new List<Meteor>();
        for (int y = 0; y < lines.Count(); y++)
        {
            for (int x = 0; x < lines.First().Length; x++)
            {
                if (lines.ElementAt(y)[x] == '#')
                {
                    meteors.Add(new Meteor(x, y));
                }
            }
        }

        int max = 0;
        int bestIndex = 0;

        for (int i = 0; i < meteors.Count; i++)
        {
            HashSet<double> angles = new HashSet<double>();
            for (int j = 0; j < meteors.Count; j++)
            {
                if (i != j)
                {
                    angles.Add(meteors[i].GetAngle(meteors[j]));
                }
            }
            if (angles.Count > max)
            {
                max = angles.Count;
                bestIndex = i;
            }
        }

        List<double> bestAngles = new List<double>();

        for (int i = 0; i < meteors.Count; i++)
        {
            if (i != bestIndex)
            {
                if (!bestAngles.Contains(meteors[bestIndex].GetAngle(meteors[i])))
                {
                    bestAngles.Add(meteors[bestIndex].GetAngle(meteors[i]));
                }
            }
        }

        bestAngles.Sort();

        Dictionary<double, List<Meteor>> laserTargets = new Dictionary<double, List<Meteor>>();
        for (int i = 0; i < meteors.Count; i++)
        {
            if (i != bestIndex)
            {
                double angle = meteors[bestIndex].GetAngle(meteors[i]);
                if (!laserTargets.ContainsKey(angle))
                {
                    laserTargets.Add(angle, new List<Meteor>());
                }
                laserTargets[angle].Add(meteors[i]);
            }
        }

        for (int i = 0; i < bestAngles.Count; i++)
        {
            laserTargets[bestAngles[i]].Sort((a, b) => a.GetDistance(meteors[bestIndex]).CompareTo(b.GetDistance(meteors[bestIndex])));
        }

        int count = 0;
        int index = 0;

        while (count < 200)
        {
            if (laserTargets[bestAngles[index]].Count > 0)
            {
                count++;
                if (count == 200)
                {
                    Meteor targetMeteor = laserTargets[bestAngles[index]].First();
                    return $"The 200th asteroid to be destroyed is at {(string)targetMeteor} or {targetMeteor.X*100 + targetMeteor.Y}.";
                }
                laserTargets[bestAngles[index]].RemoveAt(0);
            }
            index = (index + 1) % bestAngles.Count;
        }
        return $"Didn't get an answer something went wrong.";
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