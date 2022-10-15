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

class Vector3D
{
    public int X;
    public int Y;
    public int Z;

    public Vector3D(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }
    public static Vector3D operator +(Vector3D a, Vector3D b)
    {
        return new Vector3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    }
}

// class Moon:

//     def __init__(self, position):
//         self.position = position
//         self.velocity = Vector3D(0, 0, 0)

//     def attractedBy(self, other):

//         if self.position.x > other.position.x:
//             self.velocity.x -= 1
//         elif self.position.x < other.position.x:
//             self.velocity.x += 1

//         if self.position.y > other.position.y:
//             self.velocity.y -= 1
//         elif self.position.y < other.position.y:
//             self.velocity.y += 1

//         if self.position.z > other.position.z:
//             self.velocity.z -= 1
//         elif self.position.z < other.position.z:
//             self.velocity.z += 1

//     def move(self):
//         self.position += self.velocity

//     def energy(self):

//         potential = abs(self.position.x) + abs(self.position.y) + abs(self.position.z)
//         kinetic = abs(self.velocity.x) + abs(self.velocity.y) + abs(self.velocity.z)

//         return potential*kinetic

//     def __str__(self):
//         return f"Position: {self.position.x}, {self.position.y}, {self.position.z}\t Velocity: {self.velocity.x}, {self.velocity.y}, {self.velocity.z}"

class Moon
{
    public Vector3D position;
    public Vector3D velocity;

    public Moon(Vector3D position)
    {
        this.position = position;
        this.velocity = new Vector3D(0, 0, 0);
    }

    public void AttractedBy(Moon other)
    {
        if (this.position.X > other.position.X)
        {
            this.velocity.X -= 1;
        }
        else if (this.position.X < other.position.X)
        {
            this.velocity.X += 1;
        }

        if (this.position.Y > other.position.Y)
        {
            this.velocity.Y -= 1;
        }
        else if (this.position.Y < other.position.Y)
        {
            this.velocity.Y += 1;
        }

        if (this.position.Z > other.position.Z)
        {
            this.velocity.Z -= 1;
        }
        else if (this.position.Z < other.position.Z)
        {
            this.velocity.Z += 1;
        }
    }

    public void Move()
    {
        this.position += this.velocity;
    }

    public int Energy()
    {
        int potential = Math.Abs(this.position.X) + Math.Abs(this.position.Y) + Math.Abs(this.position.Z);
        int kinetic = Math.Abs(this.velocity.X) + Math.Abs(this.velocity.Y) + Math.Abs(this.velocity.Z);

        return potential * kinetic;
    }

    public override string ToString()
    {
        return $"Position: {this.position.X}, {this.position.Y}, {this.position.Z}\t Velocity: {this.velocity.X}, {this.velocity.Y}, {this.velocity.Z}";
    }
}

class Program
{
    static Moon[] parseLines(System.Collections.Generic.IEnumerable<String> lines)
    {
        Moon[] moons = new Moon[4];

        int i = 0;

        foreach (var line in lines)
        {
            string newLine = line;
            int x = int.Parse(newLine[(newLine.IndexOf('=') + 1)..newLine.IndexOf(',')]);
            newLine = newLine[(newLine.IndexOf(',') + 1)..];
            int y = int.Parse(newLine[(newLine.IndexOf('=') + 1)..newLine.IndexOf(',')]);
            newLine = newLine[(newLine.IndexOf(',') + 1)..];
            int z = int.Parse(newLine[(newLine.IndexOf('=') + 1)..newLine.IndexOf('>')]);

            moons[i] = new Moon(new Vector3D(x, y, z));
            i++;
        }
        return moons;
    }

    static string part1(System.Collections.Generic.IEnumerable<String> lines)
    {
        Moon[] moons = parseLines(lines);

        for (int i = 0; i < 1000; i++)
        {
            for (int j = 0; j < moons.Length; j++)
            {
                for (int k = 0; k < moons.Length; k++)
                {
                    if (j != k)
                    {
                        moons[j].AttractedBy(moons[k]);
                    }
                }
            }

            for (int j = 0; j < moons.Length; j++)
            {
                moons[j].Move();
            }
        }

        int totalEnergy = 0;

        for (int i = 0; i < moons.Length; i++)
        {
            totalEnergy += moons[i].Energy();
        }
        return $"After 1000 turns there is {totalEnergy} in the system.";
    }

    static long lcm(long a, long b)
    {
        long step = Math.Max(a, b);
        long temp = step;
        while (true)
        {
            if (temp % a == 0 && temp % b == 0)
            {
                return temp;
            }
            temp += step;
        }
    }

    static string part2(System.Collections.Generic.IEnumerable<String> lines)
    {
        Moon[] moons = parseLines(lines);

        string initial_x = "";
        string initial_y = "";
        string initial_z = "";

        for (int i = 0; i < moons.Length; i++)
        {
            initial_x += $"{moons[i].position.X}, {moons[i].velocity.X}, ";
            initial_y += $"{moons[i].position.Y}, {moons[i].velocity.Y}, ";
            initial_z += $"{moons[i].position.Z}, {moons[i].velocity.Z}, ";
        }

        int period_x = 0;
        int period_y = 0;
        int period_z = 0;

        int steps = 0;

        while (period_x * period_y * period_z == 0)
        {
            for (int i = 0; i < moons.Length; i++)
            {
                for (int j = 0; j < moons.Length; j++)
                {
                    if (i != j)
                    {
                        moons[i].AttractedBy(moons[j]);
                    }
                }
            }

            for (int i = 0; i < moons.Length; i++)
            {
                moons[i].Move();
            }

            steps++;

            string current_x = "";
            string current_y = "";
            string current_z = "";

            for (int i = 0; i < moons.Length; i++)
            {
                current_x += $"{moons[i].position.X}, {moons[i].velocity.X}, ";
                current_y += $"{moons[i].position.Y}, {moons[i].velocity.Y}, ";
                current_z += $"{moons[i].position.Z}, {moons[i].velocity.Z}, ";
            }

            if (initial_x == current_x && period_x == 0)
            {
                period_x = steps;
            }

            if (initial_y == current_y && period_y == 0)
            {
                period_y = steps;
            }

            if (initial_z == current_z && period_z == 0)
            {
                period_z = steps;
            }
        }

        long result = lcm(period_x, lcm(period_y, period_z));

        return $"The system repeats itself after {result} steps.";
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