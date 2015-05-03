using System;

// Naam: Matthijs Platenburg
// Studentnummer: 4260953
class Program
{
    static long alfa, beta, gamma;
    static string line;
    static string[] lineSplit;
    static long[] levelReqs;

    static void Main(string[] args)
    {
        alfa = 0;
        beta = 0;
        gamma = 0;

        // Splitting the first line to get the alfa, beta and gamma.
        line = Console.ReadLine();
        lineSplit = line.Split(' ');

        // Try parsing the split string to the variables.
        try
        {
            // Alfa: The amount of pounts required for level 2.
            alfa = long.Parse(lineSplit[0]);
            // Beta: The integer used for the fraction for determining the level after a given one.
            beta = long.Parse(lineSplit[1]);
            // Gamma: The maximum level.
            gamma = long.Parse(lineSplit[2]);
        }
        catch (Exception) { ; }

        // Try creating the array.
        if (gamma > 1)
            levelReqs = new long[gamma];
        else
            levelReqs = new long[2];

        // Filling the array.
        levelReqs[0] = 0;
        levelReqs[1] = alfa;
        ulong longCheck = long.MaxValue;

        for (long i = 2; i < levelReqs.LongLength; i++)
        {
            levelReqs[i] = levelReqs[i - 1] + levelReqs[i - 1] / beta;
            if (levelReqs[i - 1] % 2 == 1)
                levelReqs[i] += 1;
            if ((ulong)levelReqs[i] > longCheck)
                levelReqs[i] = long.MaxValue;
        }

        // Recieving Points.
        line = Console.ReadLine();

        while (line != null)
        {
            try
            {
                long points = long.Parse(line);
                long result = SearchInArray(points);
                Console.WriteLine(result);
            }
            catch (Exception)
            {
            }

            line = Console.ReadLine();
        }
    }

    // Searching the library.
    private static long SearchInArray(long Points)
    {
        long Onder, Midden, Boven;
        Boven = levelReqs.LongLength;
        Midden = 0;
        Onder = 1;

        // Check if it is larger than the top of the array.
        if (Points >= levelReqs[Boven - 1])
            return Boven;

        // Binary search within the array.
        while (Onder < Boven)
        {
            // Calculate the middle of the levels.
            Midden = (Boven + Onder) / 2;

            // We are too high, lower the top.
            if (levelReqs[Midden] > Points)
                Boven = Midden;
            // We must be too low, raise the bottom.
            else
                Onder = Midden + 1;
        }

        return Onder;
    }
}

