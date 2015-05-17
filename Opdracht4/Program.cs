using System;

// Naam: Matthijs Platenburg
// Studentnummer: 4260953

class Program
{
    static long p, q, s, m, x;
    static long result;
    static long[] array;

    static void Main(string[] args)
    {
        // Parsing input.
        p = long.Parse(Console.ReadLine());
        q = long.Parse(Console.ReadLine());
        s = long.Parse(Console.ReadLine());
        m = long.Parse(Console.ReadLine());
        x = long.Parse(Console.ReadLine());

        // Creating the array.
        array = new long[m / x];

        array[0] = s;

        for (int i = 1; i < m; i++)
        {
            array[i] = (p * array[i - 1] + q) % m;

            // if the difference with another value in the array is smaller than x, or larger than m-x, retrun the result;
            if ()
            result = i + 1;
            break;
        }
        // Printing the result.
        Console.WriteLine(result);
    }
}
