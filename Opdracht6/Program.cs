using System;
using System.IO;

// Naam: Matthijs Platenburg
// Studentnummer: 4260953

// TODO:
/* 
 * Input
 * Output
 * Dynamic Array for output
 * Objects for Players
 * Hash Function
 * Query Calculation
 *  
 */



class Program
{
    static void Main(string[] args)
    {
        // Setup the hash table.

        // Loop Start
        string input;
        string[] inputSplit;
        input = Console.ReadLine();
        while (input != null)
        {
            inputSplit = input.Split(' ');

            // Calculate correct output.
            // New Player.
            if (inputSplit[0] == "N")
                OutputN(CalculateHash(inputSplit[1]), inputSplit[1]);

            // Delete Player
            else if (inputSplit[0] == "D")
                OutputD(CalculateHash(inputSplit[1]), inputSplit[1]);

            // Output

            // Recieve next input.
            input = Console.ReadLine();
        }
    }

    static uint CalculateHash(string Input)
    {
        uint result = 0;

        // Calculate the hash.
        for (int i = 0; i < Input.Length; i++)
            result += (uint)i * Input[i] * Input[i];

        // Return the calculated hash.
        return result;
    }

    // Input a new Player.
    static void OutputN(uint Hash, string Name)
    {

    }

    // Delete an existing player.
    static void OutputD()
    {

    }

    // Raise the score of an existing player.
    static void OutputX()
    {

    }

    // Print the score of an existing player.
    static void OutputQ()
    {

    }
}

class Speler
{
    public Speler previous, next;
    public readonly string name;
    public uint score;

    public Speler(string Name, uint Score, Speler Previous = null)
    {
        this.name = Name;
        this.score = Score;
        this.previous = Previous;
    }
}