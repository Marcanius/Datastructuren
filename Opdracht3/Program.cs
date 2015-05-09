using System;

// Naam: Matthijs Platenburg.
// Studentnummer: 4260953.

class Program
{
    static IntPair[] Input, Output;
    static Random Random;

    static void Main(string[] args)
    {
        // Creating the Random number generator for Quicksort.
        Random = new Random();

        // Creating the unsorted array.
        int size = int.Parse(Console.ReadLine());
        Input = new IntPair[size];
        Output = new IntPair[size];

        string[] line;

        // Filling the unsorted array.
        for (int i = 0; i < size; i++)
        {
            line = Console.ReadLine().Split(' ');
            Input[i] = new IntPair(int.Parse(line[0]), int.Parse(line[1]));
        }

        // Sorting the array.
        if (size <= 20)
        {
            Output = InsertionSort(Input);
        }
        else
        {
            Output = RndmQuickSort(Input);
        }

        for (int i = 0; i < Output.Length; i++)
        {
            Console.WriteLine(Output[i].Teller + ' ' + Output[i].Noemer);
        }
    }

    public static int Partition(IntPair[] A, int start, int stop)
    {
        // Determine a random pivot.
        int pivot = Random.Next(A.Length);
        // Buffer for switching elements.
        IntPair temp;
        // The position of the first element larger than the pivot.
        int i = start - 1;

        // Switch pivot and the last element in the range.
        temp = A[pivot];
        A[pivot] = A[stop];
        A[stop] = temp;

        // Compare each element to the pivot.
        for (int j = start; j < stop - 1; j++)
        {
            // If the element is smaller than the pivot, switch it with the smallest element larger than the pivot.
            if (A[j].Breuk < A[pivot].Breuk)
            {
                // Increase i by 1 and Switch A[i] and A[j].
                temp = A[i++];
                A[i] = A[j];
                A[j] = temp;
            }

            // If they are equal, compare the numerator.
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

        // Check if we are large enough to partition, or we should InsertionSort.
        if (start < stop)
            if (stop - start > 20)
                pivot = Partition(Input, start, stop);

        // If the resulting partition are larger than 20 elements, QuickSort them.
        if ((pivot - 1) - start > 20)
            RndmQuickSort(Input, start, pivot - 1);
            // Otherwise, InsertionSort them.
        else
            InsertionSort(Input, start, pivot - 1);

        // Same deal as last four lines.
        if (stop - (pivot + 1) > 20)
            RndmQuickSort(Input, pivot + 1, stop);
        else
            InsertionSort(Input, pivot + 1, stop);
    }

    public static IntPair[] InsertionSort(IntPair[] Input, int start, int stop)
    {
        IntPair temp;
        int i;

        for (int j = start + 1; j < stop; j++)
        {
            temp = Input[j];
            i = j - 1;

            while (i > 0 && Input[i].Breuk >= temp.Breuk && Input[i].Teller > temp.Teller)
            {
                Input[i + 1] = Input[i];
                i--;
            }
            Input[i + 1] = temp;
        }

            // Sort it.

            return Output;
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

        Breuk = teller / noemer;
    }
}