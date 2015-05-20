using System;
using System.IO;

// Naam: Matthijs Platenburg
// Studentnummer: 4260953

class Program
{
    public static long p, q, s, m, x;
    public static long i, previous, current, currentBucket;
    public static long[] array;
    public static bool zeroTaken = false;

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
                // Current / x is in the first bucket.
                if (currentBucket == 0)
                {
                    // If the current bucket has already been filled, we stop, since two values in the same bucket cannot differ more than x from one another.
                    if (zeroTaken || array[currentBucket] != 0)
                        break;

                    // If the bucket has not been filled, check the adjacent buckets.
                    else
                    {
                        // If we are actually looking at zero, we need to record that, since 0 is the default value of the array. 
                        if (current == 0)
                            zeroTaken = true;

                        // Check the adjacent buckets.
                        // The last bucket.
                        if (array[array.LongLength - 1] != 0)
                            if (Math.Abs(current - array[array.LongLength - 1]) >= m - x)
                                break;

                        // The second to last bucket.
                        if (array[array.LongLength - 2] != 0)
                            if (Math.Abs(current - array[array.LongLength - 2]) >= m - x)
                                break;

                        // The second bucket.
                        if (CheckNextBucket(currentBucket, current))
                            break;

                        // It does not differ at most x from any previously calculated value; write it to its bucket.
                        array[currentBucket] = current;
                        continue;
                    }
                }

                // Current / x is in the second bucket.
                else if (currentBucket == 1)
                {
                    // Check if there has already been a value in the current bucket.
                    if (array[1] != 0)
                        break;

                    // Check the adjacent buckets.
                    // The first bucket.
                    if (array[0] != 0 || zeroTaken)
                        if (Math.Abs(current - array[0]) <= x)
                            break;
                    // The third bucket.
                    if (CheckNextBucket(currentBucket, current))
                        break;

                    array[currentBucket] = current;
                    continue;
                }

                // Second to last bucket.
                else if (currentBucket == array.LongLength - 2)
                {
                    if (array[currentBucket] != 0)
                        break;

                    // Check the adjacent buckets.
                    // The third from last bucket.
                    if (CheckPrevBucket(currentBucket, current))
                        break;

                    // The last bucket.
                    if (CheckNextBucket(currentBucket, current))
                        break;

                    // The first bucket.
                    if (zeroTaken || array[0] != 0)
                        if (Math.Abs(current - array[0]) >= m - x)
                            break;

                    array[currentBucket] = current;
                    continue;
                }

                // Current / x is in the last bucket.
                else if (currentBucket == array.LongLength - 1)
                {
                    // If the bucket is already taken, we stop, since two values in the same bucket can not differ more than x from one another.
                    if (array[currentBucket] != 0)
                        break;

                    // Check the adjacent buckets.
                    // The second from last bucket.
                    if (CheckPrevBucket(currentBucket, current))
                        break;

                    // The third from last bucket.
                    if (CheckPrevBucket(currentBucket - 1, current))
                        break;

                    // The first bucket.
                    if (array[0] != 0 || zeroTaken)
                        if (Math.Abs(current - array[0]) >= m - x)
                            break;

                    // The second bucket.
                    if (CheckNextBucket(0, current))
                        break;

                    array[currentBucket] = current;
                    continue;
                }
            }

            #endregion

            else
            {
                // The regular testing of the buckets:
                // Check if the bucket of current is empty.
                // If it is not, its contents cannot differ more than x from an element in the same bucket, and we have our result.
                if (array[currentBucket] != 0)
                    break;

                // Check the adjacent buckets.
                if (CheckNextBucket(currentBucket, current) || CheckPrevBucket(currentBucket, current))
                    break;

                // It does not differ at most x from any previously calculated value; write current to its bucket.
                array[currentBucket] = current;
                continue;
            }
        }

        // Printing the result.
        Console.WriteLine(i);
    }

    static bool CheckNextBucket(long Bucket, long Value)
    {
        if (array[Bucket + 1] != 0 && Math.Abs(Value - array[Bucket + 1]) <= x)
            return true;
        return false;
    }

    static bool CheckPrevBucket(long Bucket, long Value)
    {
        if (array[Bucket - 1] != 0 && Math.Abs(Value - array[Bucket - 1]) <= x)
            return true;
        return false;
    }
}
