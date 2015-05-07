using System;

// Naam: Matthijs Platenburg.
// Studentnummer: 4260953.

class Program
{
    static ArrayNumber[] Input, Output;

    static void Main(string[] args)
    {

        // Creating the unsorted array.
        int size = int.Parse(Console.ReadLine());
        Input = new ArrayNumber[size];
        Output = new ArrayNumber[size];

        string[] line;

        // Filling the unsorted array.
        for (int i = 0; i < size; i++)
        {
            line = Console.ReadLine().Split(' ');
            Input[i] = new ArrayNumber(int.Parse(line[0]), int.Parse(line[1]));
        }

        // Sorting the array.
        if (size <= 20)
        {

        }
    }

    public ArrayNumber[] RndmQuickSort(ArrayNumber[] Input)
    {
        ArrayNumber[] Output = new ArrayNumber[Input.Length];

        // Select a random 

        // Partition it.

        return Output;
    }

    public ArrayNumber[] InsertionSort(ArrayNumber[] Input)
    {
        ArrayNumber[] Output = new ArrayNumber[Input.Length];

        // Sort it.

        return Output;
    }
}

// 
/// <summary>
/// A struct containing a pair of two integers.
/// It also contains the fraction of these two numbers, so we don't have to keep calculating it.
/// </summary>
struct ArrayNumber
{
    public readonly int Teller;
    public readonly int Noemer;
    public readonly float Breuk;

    public ArrayNumber(int teller, int noemer)
    {
        Teller = teller;
        Noemer = noemer;

        Breuk = teller / noemer;
    }
}