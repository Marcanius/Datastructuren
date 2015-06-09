using System;
using System.IO;

// Naam: Matthijs Platenburg
// Studentnummer: 4260953

// Soort Boom: TODO

class Program
{
    static void Main(string[] args)
    {
        // Setup.


        string input;
        string[] inputSplit;
        
        // Loop through instructions.
        input = Console.ReadLine();
        while (input != null && input != "")
        {
            inputSplit = input.Split(' ');


            // Recieve next input.
            input = Console.ReadLine();
        }
    }
}

class Speler
{
    int PlayerNumber;
    int Booms;

    public Speler(int PlayerNumber, int Booms)
    {
        this.PlayerNumber = PlayerNumber;
        this.Booms = PlayerNumber;
    }
}

class Tree
{

}