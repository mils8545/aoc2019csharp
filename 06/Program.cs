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

class Node
{
    public string Name { get; set; }
    public Node[] Children { get; set; }
    public int DistanceFromRoot { get; set; }

    public Node(string name, int distanceFromRoot)
    {
        Name = name;
        DistanceFromRoot = distanceFromRoot;
        Children = new Node[0];
    }

    public void AddChild(string name)
    {
        Node child = new Node(name, DistanceFromRoot + 1);
        Node[] temp = this.Children;
        Array.Resize(ref temp, Children.Length + 1);
        this.Children = temp;
        Children[Children.Length - 1] = child;
    }

    public Node GetNode (string name)
    {
        if (Name == name)
        {
            return this;
        }
        else
        {
            foreach (Node child in Children)
            {
                Node result = child.GetNode(name);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }
    }

    public bool ContainsNode(string name)
    {
        if (Name == name)
        {
            return true;
        }
        else
        {
            foreach (Node child in Children)
            {
                if (child.ContainsNode(name))
                {
                    return true;
                }
            }
            return false;
        }
    }

    public int OrbitCount(int length = 0)
    {
        int count = 0;
        foreach (Node child in Children)
        {
            count += child.OrbitCount(length + 1);
        }
        return count + length;
    }

    public Node FindBranch(){
        foreach (Node child in Children)
        {
            if (child.ContainsNode("YOU") && child.ContainsNode("SAN"))
            {
                return child.FindBranch();
            }
            if (child.ContainsNode("YOU"))
            {
                return this;
            }
        }
        return null;
    }
}

class Orbit{
    public string Orbitee { get; set; }
    public string Orbiter { get; set; }
    public Orbit(string orbitee, string orbiter)
    {
        Orbitee = orbitee;
        Orbiter = orbiter;
    }
}

class Program
{

    static string part1(System.Collections.Generic.IEnumerable<String> lines)
    {
        Node root = new Node("COM", 0);

        Queue<Orbit> orbits = new Queue<Orbit>();
        foreach (string line in lines)
        {
            string[] parts = line.Split(')');
            orbits.Enqueue(new Orbit(parts[0], parts[1]));
        }
        while (orbits.Count > 0)
        {
            Orbit orbit = orbits.Dequeue();
            if (root.ContainsNode(orbit.Orbitee))
            {
                root.GetNode(orbit.Orbitee).AddChild(orbit.Orbiter);
            }
            else
            {
                orbits.Enqueue(orbit);
            }
        }
        return $"There are a total of {root.OrbitCount()} direct and indirect orbits in the system.";
    }

    static string part2(System.Collections.Generic.IEnumerable<String> lines)
    {
        Node root = new Node("COM", 0);

        Queue<Orbit> orbits = new Queue<Orbit>();
        foreach (string line in lines)
        {
            string[] parts = line.Split(')');
            orbits.Enqueue(new Orbit(parts[0], parts[1]));
        }

                while (orbits.Count > 0)
        {
            Orbit orbit = orbits.Dequeue();
            if (root.ContainsNode(orbit.Orbitee))
            {
                root.GetNode(orbit.Orbitee).AddChild(orbit.Orbiter);
            }
            else
            {
                orbits.Enqueue(orbit);
            }
        }
        Node branch = root.FindBranch();
        Node you = branch.GetNode("YOU");
        Node san = branch.GetNode("SAN");
        int you_san_distance = you.DistanceFromRoot + san.DistanceFromRoot - 2 - 2 * branch.DistanceFromRoot;
        return $"It takes {you_san_distance} orbital transfers to get to Santa.";
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