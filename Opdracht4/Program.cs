using System;

// Naam: Matthijs Platenburg
// Studentnummer: 4260953

class Program
{
    static long p, q, s, m, x;
    static long previous, current, currentBucket;
    static long j;
    static long[] array;
    static bool zeroTaken;

    static void Main(string[] args)
    {
        // Parsing input.
        p = long.Parse(Console.ReadLine());
        q = long.Parse(Console.ReadLine());
        s = long.Parse(Console.ReadLine());
        m = long.Parse(Console.ReadLine());
        x = long.Parse(Console.ReadLine());

        // Creating the array of buckets.
        array = new long[m / x];

        // Setting the first value.
        array[s / x] = s;
        previous = s;

        int i;
        // Calculating the row a[i].
        for (i = 0; i < m; i++)
        {
            // Calculate the next value, and its bucket.
            current = (p * previous + q) % m;
            currentBucket = current / x;

            #region Special Cases

            // Current = 0.
            if (current == 0)
            {
                // If zero has already been calculated, we can stop; the difference between 0 and 0 is always smaller than x.
                if (zeroTaken)
                    break;

                // If zero has not been calculated previously.
                else
                {
                    // Zero has now been calculated, so we record that.
                    zeroTaken = true;
                    // Check the adjacent buckets.
                    // The last bucket.
                    if (array[array.Length - 1] != 0)
                        if (array[array.Length - 1] >= m - x)
                            break;

                    // The second bucket.
                    if (array[1] != 0)
                        if (array[1] <= x)
                            break;

                    // It does not differ at most x from any previously calculated value; write it to its bucket.
                    array[0] = 0;
                    continue;
                }
            }

            // Current / x is in the first bucket.
            if (currentBucket == 0)
            {
                // If zero was already a result, we are done, since two values in the same bucket cannot differ more than x from eachother.
                if (zeroTaken)
                    break;
                // If 0 wasn't taken, check the adjacent buckets.
                else
                {
                    // The last bucket.
                    if (array[array.Length - 1] != 0)
                        if (Math.Abs(current - array[array.Length - 1]) >= m - x)
                            break;

                    // The second bucket.
                    if (array[1] != 0)
                        if (Math.Abs(current - array[1]) <= x)
                            break;

                    // It does not differ at most x from any previously calculated value; write it to its bucket.
                    array[current / x] = current;
                    continue;
                }
            }

            // Current / x is in the second bucket.
            if (currentBucket == 1)
            {
                // Check if there has already been a value in the current / x bucket.
                if (array[1] != 0)
                    break;
            }

            // Current / x is in the last bucket.
            if (current / x == array.Length - 1)
            {

            }
            #endregion

            // The regular testing of the buckets:
            // Check if the bucket of current is empty, if it isn't, its contents cannot differ more than x from 
            if (array[current / x] != 0)
                break;

            // If the adjacent buckets are not empty, check if they really differ x from those values.
            // Bucket above.
            if (array[current / x + 1] != 0)
                if (Math.Abs(array[current / x + 1] - current) <= x)
                    break;
                else if (Math.Abs(array[current / x + 1] - current) >= m - x)
                    break;

            // Bucket below.
            if (array[current / x - 1] != 0)
                if (Math.Abs(array[current / x - 1] - current) <= x)
                    break;
                else if (Math.Abs(array[current / x - 1] - current) >= m - x)
                    break;

            // It does differ <= x from any previous values, write current to its bucket.
            array[current / x] = current;

            previous = current;
        }

        // Printing the result.
        Console.WriteLine(i + 1);
    }
}
