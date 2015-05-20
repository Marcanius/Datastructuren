using System;
using System.IO;

// Naam: Matthijs Platenburg
// Studentnummer: 4260953

class Program
{
    public static long p, q, s, m, x;
    public static long i, previous, current, currentBucket;
    public static long[] array;

    static void Main(string[] args)
    {
        // Parsing input.
        p = long.Parse(Console.ReadLine());
        q = long.Parse(Console.ReadLine());
        s = long.Parse(Console.ReadLine());
        m = long.Parse(Console.ReadLine());
        x = long.Parse(Console.ReadLine());

        // Creating the array of buckets.
        array = new long[(long)Math.Ceiling((double)m / (double)x)];

        // Setting the default values.
        for (long j = 0; j < array.LongLength; j++)
            array[j] = -1;

        // Setting the first value.
        array[s / x] = s;
        current = s;

        // Calculating the row a[i].
        for (i = 2; i < m; i++)
        {
            // Calculate the next value, and its bucket.
            previous = current;
            current = (p * previous + q) % m;
            currentBucket = current / x;

            #region Special Cases

            if (currentBucket < 2 || currentBucket > array.LongLength - 3)
            {
                // First bucket.
                if (currentBucket == 0)
                {

                    // Check the bucket, and the adjacent ones.
                    if (
                        CheckBucket(0, current) ||
                        CheckBucket(1, current) ||
                        CheckBucketAlt(array.LongLength - 1, current) ||
                        CheckBucketAlt(array.LongLength - 2, current)
                        )
                        break;

                    // It does not differ at most x from any previously calculated value; write it to its bucket.
                    array[currentBucket] = current;
                    continue;
                }

                // Second to last bucket.
                else if (currentBucket == array.LongLength - 2)
                {
                    if (
                        CheckBucket(currentBucket, current) ||
                        CheckBucket(currentBucket + 1, current) ||
                        CheckBucketAlt(0, current) ||
                        CheckBucket(currentBucket - 1, current)
                        )
                        break;

                    array[currentBucket] = current;
                    continue;
                }

                // Last bucket.
                else if (currentBucket == array.LongLength - 1)
                {
                    // If the bucket is already taken, we stop, since two values in the same bucket can not differ more than x from one another.
                    if (
                        CheckBucket(currentBucket, current) ||
                        CheckBucketAlt(0, current) ||
                        CheckBucket(currentBucket - 1, current)
                        )
                        break;

                    array[currentBucket] = current;
                    continue;
                }
            }

            #endregion

            else
            {
                // The regular testing of the buckets:
                // Check if the current bucket is empty, then check the adjacent buckets as well.
                if (
                    CheckBucket(currentBucket, current) ||
                    CheckBucket(currentBucket + 1, current) ||
                    CheckBucket(currentBucket - 1, current)
                    )
                    break;

                // It does not differ at most x from any previously calculated value; write current to its bucket.
                array[currentBucket] = current;
                continue;
            }
        }

        // Printing the result.
        Console.WriteLine(i);
    }

    /// <summary>
    /// We check if the bucket specified already contains a value, and if so, we check if it differs at most x from the given value.
    /// </summary>
    /// <param name="Bucket"> The bucket to check. </param>
    /// <param name="Value"> The value to check against. </param>
    /// <returns> true if we can not write to the bucket, and stop our loop, false if we can write to our bucket, and continue the loop. </returns>
    static bool CheckBucket(long Bucket, long Value)
    {
        if (array[Bucket] != -1 && Math.Abs(Value - array[Bucket]) <= x)
            return true;
        return false;
    }

    /// <summary>
    /// An alternate version of CheckBucket(), which is used by the last and first bucket, to check across the 0 line.
    /// </summary>
    /// <param name="Bucket"></param>
    /// <param name="Value"></param>
    /// <returns></returns>
    static bool CheckBucketAlt(long Bucket, long Value)
    {
        if (array[Bucket] != -1 && Math.Abs(Value - array[Bucket]) >= m - x)
            return true;
        return false;
    }
}
