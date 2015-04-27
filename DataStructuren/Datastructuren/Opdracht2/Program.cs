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
        {
            levelReqs = new long[gamma];
        }
        else
        {
            levelReqs = new long[2];
        }
        // Filling the array.
        levelReqs[0] = 0;
        levelReqs[1] = alfa;
        for (long i = 2; i < levelReqs.LongLength; i++)
        {
            levelReqs[i] = levelReqs[i - 1] + levelReqs[i - 1] / beta;
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
                Console.WriteLine("Oops");
            }

            line = Console.ReadLine();
        }
    }
    // Binary Search
    private static long SearchInArray(long Points)
    {
        long Onder, Midden, Boven;
        Boven = levelReqs.LongLength - 1;
        Midden = 0;
        Onder = 0;

        while (Onder < Boven)
        {
            // Calculate the middle of the levels.
            Midden = (Boven + Onder) / 2;

            // If we are right on the money, stop.
            if (levelReqs[Midden] == Points)
                return Midden + 1;
            // We are too low, raise the bottom.
            if (levelReqs[Midden] < Points)
                Onder = Midden + 1;
            // We are too high, lower the top.
            if (levelReqs[Midden] > Points)
                Boven = Midden + 1;
        }

        return Midden + 1;
    }
}

