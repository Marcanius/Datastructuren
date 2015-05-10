using System;

// Naam: Matthijs Platenburg.
// Studentnummer: 4260953.

class Program
{
    static IntPair[] Input;
    static Random Random;

    static void Main(string[] args)
    {
        // Creating the Random number generator for Quicksort.
        Random = new Random();

        // Creating the array.
        int size = int.Parse(Console.ReadLine());
        Input = new IntPair[size];

        // Filling the array.
        string[] line;
        for (int i = 0; i < size; i++)
        {
            line = Console.ReadLine().Split(' ');
            Input[i] = new IntPair(int.Parse(line[0]), int.Parse(line[1]));
        }

        // Sorting the array.
        if (size <= 20)
            InsertionSort(Input, 0, Input.Length - 1);
        else
            RndmQuickSort(Input, 0, Input.Length - 1);

        // Printing the sorted array.
        for (int i = 0; i < size; i++)
            Console.WriteLine(Input[i].Teller.ToString() + ' ' + Input[i].Noemer.ToString());
    }

    /// <summary>
    /// Divides the array in three parts: a pivot, all elements smaller than the pivot, and all elements larger than the pivot.
    /// </summary>
    /// <param name="A"> The array to partition. </param>
    /// <param name="start"> The first element of the array to include in the partitioning. </param>
    /// <param name="stop"> The last element of the array to include in the partitioning. </param>
    /// <returns> The new position of the pivot. </returns>
    public static int Partition(IntPair[] A, int start, int stop)
    {
        // Determine a random pivot.
        int pivot = Random.Next(start, stop + 1);
        // Buffer for switching elements.
        IntPair temp;
        // The position of the first element larger than the pivot.
        int i = start - 1;

        // Switch pivot and the last element in the range.
        temp = A[pivot];
        A[pivot] = A[stop];
        A[stop] = temp;

        // Compare each element to the pivot.
        for (int j = start; j < stop; j++)
        {
            // If the element is smaller than the pivot, switch it with the first element larger than the pivot.
            if (A[j].Breuk < A[pivot].Breuk)
            {
                // Increase i by 1 and Switch A[i] and A[j].
                temp = A[i++];
                A[i] = A[j];
                A[j] = temp;
            }

            // If they are equal, compare the numerator, and switch accordingly.
            else if (A[j].Breuk == A[i].Breuk)
                if (A[j].Teller <= A[i].Teller)
                {
                    // Increase i by 1 and Switch A[i] and A[j].
                    temp = A[i++];
                    A[i] = A[j];
                    A[j] = temp;
                }
        }

        // Switch the pivot and the first element larger than the pivot.
        temp = A[i++];
        A[i] = A[pivot];
        A[pivot] = temp;

        // Return the position of the pivot.
        return i;
    }

    public static void RndmQuickSort(IntPair[] Input, int start, int stop)
    {
        int pivot = 0;

        if (start < stop)
        {
            // Create a pivot by partitioning.
            pivot = Partition(Input, start, stop);

            // If the resulting first partition is larger than 20 elements, QuickSort them.
            if ((pivot - 1) - start > 20)
                RndmQuickSort(Input, start, pivot - 1);
            // Otherwise, InsertionSort them.
            else
                InsertionSort(Input, start, pivot - 1);

            // If the resulting second partition is larger than 20 elements, QuickSort them.
            if (stop - (pivot + 1) > 20)
                RndmQuickSort(Input, pivot + 1, stop);
            // Otherwise, InsertionSort them.
            else
                InsertionSort(Input, pivot + 1, stop);
        }
    }

    public static void InsertionSort(IntPair[] A, int start, int stop)
    {
        IntPair temp;
        int i;

        for (int j = start + 1; j <= stop; j++)
        {
            temp = A[j];

            // Place A[j] in the correct place in the array.
            i = j - 1;
            // Check if the previous element has a larger fraction, or, if the fractions are equal, a larger denominator than the current key.
            while (i >= start && (A[i].Breuk > temp.Breuk || (A[i].Breuk == temp.Breuk && A[i].Teller > temp.Teller)))
            {
                // Move the prevoious element up.
                A[i + 1] = A[i];
                // Decrease the element we are checking.
                i--;
            }

            // Place the key at the correct place.
            A[i + 1] = temp;
        }
        return;
    }
}

// 
/// <summary>
/// A struct containing a pair of two integers.
/// It also contains the fraction of these two numbers, so we don't have to keep calculating it.
/// </summary>
struct IntPair
{
    public readonly int Teller;
    public readonly int Noemer;
    public readonly float Breuk;

    public IntPair(int teller, int noemer)
    {
        Teller = teller;
        Noemer = noemer;

        Breuk = (float)teller / (float)noemer;
    }
}