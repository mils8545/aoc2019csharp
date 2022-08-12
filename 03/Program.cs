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

class Step
{
    public char Direction;
    public int Distance;
    public Step(string toParse)
    {
        Direction = toParse[0];
        Distance = int.Parse(toParse.Substring(1));
    }
}

class Vector2Int{
    public int X;
    public int Y;
    public Vector2Int(int x, int y)
    {
        X = x;
        Y = y;
    }
    public int ManhattanDistance()
    {
        return Math.Abs(X) + Math.Abs(Y);
    }
    
}

class Program
{

    static int getSize(Step[] wire1Steps, Step[] wire2Steps){
        Dictionary<char, Vector2Int> stepDirections =
            new Dictionary<char, Vector2Int>();
        stepDirections.Add('R', new Vector2Int(1, 0));
        stepDirections.Add('L', new Vector2Int(-1, 0));
        stepDirections.Add('U', new Vector2Int(0, 1));
        stepDirections.Add('D', new Vector2Int(0, -1));

        Vector2Int position = new Vector2Int(0, 0);
        int maxDistance = 0;

        foreach (Step step in wire1Steps)
        {
            position.X = position.X + (stepDirections[step.Direction].X * step.Distance);
            position.Y = position.Y + (stepDirections[step.Direction].Y * step.Distance);
            maxDistance = Math.Max(Math.Max(maxDistance, Math.Abs(position.X)), Math.Abs(position.Y));
        }

        position = new Vector2Int(0, 0);
        foreach (Step step in wire2Steps)
        {
            position.X = position.X + (stepDirections[step.Direction].X * step.Distance);
            position.Y = position.Y + (stepDirections[step.Direction].Y * step.Distance);
            maxDistance = Math.Max(Math.Max(maxDistance, Math.Abs(position.X)), Math.Abs(position.Y));
        }
        return maxDistance;
    }

    static int stringManhattanDistance(string stringPosition)
    {
        string xString = stringPosition.Substring(0, stringPosition.IndexOf(','));
        string yString = stringPosition.Substring(stringPosition.IndexOf(',') + 1);
        int x = int.Parse(xString);
        int y = int.Parse(yString);
        return Math.Abs(x) + Math.Abs(y);
    }

    static string part1(System.Collections.Generic.IEnumerable<String> lines)
    {
        Step[] wire1Steps = lines.ElementAt(0).Split(",").Select(x => new Step(x)).ToArray();
        Step[] wire2Steps = lines.ElementAt(1).Split(",").Select(x => new Step(x)).ToArray();

        Dictionary<char, Vector2Int> stepDirections =
            new Dictionary<char, Vector2Int>();
        stepDirections.Add('R', new Vector2Int(1, 0));
        stepDirections.Add('L', new Vector2Int(-1, 0));
        stepDirections.Add('U', new Vector2Int(0, 1));
        stepDirections.Add('D', new Vector2Int(0, -1));

        int size = getSize(wire1Steps, wire2Steps);

        var wire1Grid = new bool[size*2+1, size*2+1];

        Vector2Int position = new Vector2Int(size, size);

        foreach (Step step in wire1Steps)
        {
            for (int i = 0; i < step.Distance; i++)
            {
                position.X = position.X + (stepDirections[step.Direction].X);
                position.Y = position.Y + (stepDirections[step.Direction].Y);
                wire1Grid[position.X, position.Y] = true;
            }
        }

        position = new Vector2Int(size, size);

        List<string> instersections = new List<string> {};

        foreach (Step step in wire2Steps)
        {
            for (int i = 0; i < step.Distance; i++)
            {
                position.X = position.X + (stepDirections[step.Direction].X);
                position.Y = position.Y + (stepDirections[step.Direction].Y);
                if (wire1Grid[position.X, position.Y])
                {
                    string positionString = $"{position.X-size},{position.Y-size}";
                    if (!instersections.Contains(positionString))
                    {
                        instersections.Add(positionString);
                    }
                }
            }
        }

        instersections.Sort((x, y) => stringManhattanDistance(x).CompareTo(stringManhattanDistance(y)));

        int shortestDistance = stringManhattanDistance(instersections.First());
        return ($"The closest intersection is {shortestDistance} away from the start.");
    }

    static string part2(System.Collections.Generic.IEnumerable<String> lines)
    {
        Step[] wire1Steps = lines.ElementAt(0).Split(",").Select(x => new Step(x)).ToArray();
        Step[] wire2Steps = lines.ElementAt(1).Split(",").Select(x => new Step(x)).ToArray();

        Dictionary<char, Vector2Int> stepDirections =
            new Dictionary<char, Vector2Int>();
        stepDirections.Add('R', new Vector2Int(1, 0));
        stepDirections.Add('L', new Vector2Int(-1, 0));
        stepDirections.Add('U', new Vector2Int(0, 1));
        stepDirections.Add('D', new Vector2Int(0, -1));

        int size = getSize(wire1Steps, wire2Steps);

        var wire1Grid = new bool[size*2+1, size*2+1];

        Vector2Int position = new Vector2Int(size, size);

        foreach (Step step in wire1Steps)
        {
            for (int i = 0; i < step.Distance; i++)
            {
                position.X = position.X + (stepDirections[step.Direction].X);
                position.Y = position.Y + (stepDirections[step.Direction].Y);
                wire1Grid[position.X, position.Y] = true;
            }
        }

        position = new Vector2Int(size, size);

        List<string> instersections = new List<string> {};

        foreach (Step step in wire2Steps)
        {
            for (int i = 0; i < step.Distance; i++)
            {
                position.X = position.X + (stepDirections[step.Direction].X);
                position.Y = position.Y + (stepDirections[step.Direction].Y);
                if (wire1Grid[position.X, position.Y])
                {
                    string positionString = $"{position.X-size},{position.Y-size}";
                    if (!instersections.Contains(positionString))
                    {
                        instersections.Add(positionString);
                    }
                }
            }
        }

        instersections.Sort((x, y) => stringManhattanDistance(x).CompareTo(stringManhattanDistance(y)));

        Dictionary<string, int> wire1Intersections =
            new Dictionary<string, int>();

        position = new Vector2Int(size, size);
        int steps = 0;
        foreach (Step step in wire1Steps)
        {
            for (int i = 0; i < step.Distance; i++)
            {
                steps++;
                position.X = position.X + (stepDirections[step.Direction].X);
                position.Y = position.Y + (stepDirections[step.Direction].Y);
                if (instersections.Contains($"{position.X-size},{position.Y-size}"))
                {
                    if (!wire1Intersections.ContainsKey($"{position.X-size},{position.Y-size}"))
                    {
                        wire1Intersections.Add($"{position.X-size},{position.Y-size}", steps);
                    }
                }
            }
        }

        Dictionary<string, int> wire2Intersections =
            new Dictionary<string, int>();

        position = new Vector2Int(size, size);
        steps = 0;
        foreach (Step step in wire2Steps)
        {
            for (int i = 0; i < step.Distance; i++)
            {
                steps++;
                position.X = position.X + (stepDirections[step.Direction].X);
                position.Y = position.Y + (stepDirections[step.Direction].Y);
                if (instersections.Contains($"{position.X-size},{position.Y-size}"))
                {
                    if (!wire2Intersections.ContainsKey($"{position.X-size},{position.Y-size}"))
                    {
                        wire2Intersections.Add($"{position.X-size},{position.Y-size}", steps);
                    }
                }
            }
        }

        instersections.Sort((x, y) => (wire1Intersections[x] + wire2Intersections[x]).CompareTo(wire1Intersections[y] + wire2Intersections[y]));
        int shortestDistance = wire1Intersections[instersections.First()] + wire2Intersections[instersections.First()];
        return ($"The closest intersection is {shortestDistance} away from the start.");
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