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
        // If we can't parse, set everything to the default values of 0.
        catch (Exception)
        {
            alfa = 0;
            beta = 0;
            gamma = 0;
        }

        // Try creating the array.
        if (gamma > 1)
            levelReqs = new long[gamma];
        else
            levelReqs = new long[2];

        // Filling first two values of the array.
        levelReqs[0] = 0;
        levelReqs[1] = alfa;

        // Filling in the rest of the array.
        for (long i = 2; i < levelReqs.LongLength; i++)
        {
            double nextValue = (double)levelReqs[i - 1] + (double)levelReqs[i - 1] / (double)beta;
            levelReqs[i] = (long)Math.Ceiling(nextValue);

            // We don't want the values in the array to become negative if they are larger than the size of a long.
            if ((ulong)levelReqs[i] > long.MaxValue)
                levelReqs[i] = long.MaxValue;
        }

        // Recieving Input.
        line = Console.ReadLine();

        // Calculating Output.
        while (line != null)
        {
            try
            {
                long points = long.Parse(line);
                long result = SearchInArray(points);
                Console.WriteLine(result);
            }
            catch (Exception) { break; }

            //Recieving next Input.
            line = Console.ReadLine();
        }
    }

    // Searching the library.
    private static long SearchInArray(long Input)
    {
        long Onder, Midden, Boven;
        Boven = levelReqs.LongLength;
        Onder = 0;

        // Check if it is larger than the highest value in the array; if so, return that highest value.
        if (Input >= levelReqs[Boven - 1])
            return Boven;

        // Binary search within the array.
        while (Onder < Boven)
        {
            // Calculate the middle of the levels.
            Midden = (Boven + Onder) / 2;

            // If we are too high, lower the top.
            if (levelReqs[Midden] > Input)
                Boven = Midden;
            // Otherwise we must be too low, raise the bottom.
            else
                Onder = Midden + 1;
        }

        return Onder;
    }
}

