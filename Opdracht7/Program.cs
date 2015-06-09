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
            string instruction = inputSplit[0];
            int playerNumber = int.Parse(inputSplit[1]);

            // Add new Player.
            if (instruction == "T")
                AddNewPlayer(playerNumber, int.Parse(inputSplit[2]));

            // Print part of the tree.
            if (instruction == "G")
                PrintRange(playerNumber);

            // Player's rank.
            if (instruction == "R")
                PlayerRank(playerNumber);

            // Recieve next input.
            input = Console.ReadLine();
        }
    }

    static void AddNewPlayer(int PlayerNumber, int PlayerScore)
    {

    }

    static void PrintRange(int PlayerNumber)
    {

    }

    static void PlayerRank(int PlayerNumber)
    {

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