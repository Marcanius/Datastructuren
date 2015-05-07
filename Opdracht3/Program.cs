using System;

// Naam: Matthijs Platenburg.
// Studentnummer: 4260953.

class Program
{
    int[,] Input, Output;

    static void Main(string[] args)
    {
        
        ArrayNumber[] Input, Output;

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

    }

    public int[,] RndmQuickSort()
    {
        return Output;
    }

    public int[,] InsertionSort()
    {
        int[,] result = new int[1, 1];


        return Output;
    }


}

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