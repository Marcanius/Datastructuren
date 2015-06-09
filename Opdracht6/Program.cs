using System;
using System.IO;

// Naam: Matthijs Platenburg
// Studentnummer: 4260953

class Program
{
    static Speler[] Table;

    static void Main(string[] args)
    {
        // Setup.
        Table = new Speler[33343];

        string input;
        string[] inputSplit;

        // Loop through all input.
        input = Console.ReadLine();
        while (input != null && input != "")
        {
            inputSplit = input.Split(' ');
            string name = inputSplit[1];
            long hash = CalculateHash(name);

            // Calculate correct output.
            // New Player.
            if (inputSplit[0] == "N")
                OutputN(hash, name);

            // Delete a Player
            else if (inputSplit[0] == "D")
                OutputD(hash, name);

            // Raise a Score.
            else if (inputSplit[0] == "X")
                OutputX(hash, name, int.Parse(inputSplit[2]));

            // Print a Score.
            else if (inputSplit[0] == "Q")
                OutputQ(hash, name);

            // Recieve next input.
            input = Console.ReadLine();
        }
    }

    static long CalculateHash(string Input)
    {
        long result = 0;

        // Calculate the hash.
        for (int i = 0; i < Input.Length; i++)
            result += i * Input[i] * Input[i];

        // Limit the result to a large prime number.
        result %= 33343;

        // Return the calculated hash.
        return result;
    }

    static Speler SearchPlayer(long Hash, string Name)
    {
        Speler current;

        // Get the first link in the chain
        current = Table[Hash];

        // Go along the chain, until you have found the player with the given name, or until you have reached the end.
        while (current != null)
        {
            if (current.name == Name)
                return current;
            current = current.Next;
        }

        // Player could not be found in the chain, return null.
        return null;
    }

    // Input a new Player.
    static void OutputN(long Hash, string Name)
    {
        // If the player already exists, print -1 and stop.
        if (SearchPlayer(Hash, Name) != null)
        {
            Console.WriteLine("-1");
            return;
        }

        // Player does not already exist.
        else
        {
            // If there is already a player in that chain, search the last link, and add a new player to that link.
            if (Table[Hash] != null)
            {
                Speler current = Table[Hash];

                // Go along the chain until you have reached the last link.
                while (current.Next != null)
                    current = current.Next;

                // Create a new link in the chain.
                Speler temp = new Speler(Name, current);
            }

            // If there is no player in that chain, add a new player at the start.
            else
            {
                Table[Hash] = new Speler(Name);
            }
        }
    }

    // Delete an existing player.
    static void OutputD(long Hash, string Name)
    {
        // Check if the player exists in the chain.
        Speler current = SearchPlayer(Hash, Name);

        // If the player could not be found, print -1, and stop.
        if (current == null)
        {
            Console.WriteLine("-1");
            return;
        }
        else
        {
            // Change the references to and from the found player, the rest should be taken care of by garbage collection.

            // Neighbor's references.
            // Not the first link in the chain.
            if (current.Previous != null)
                // Not the last link in the chain.
                if (current.Next != null)
                {
                    current.Next.Previous = current.Previous;
                    current.Previous.Next = current.Next;
                }
                // The last link in the chain.
                else
                    current.Previous.Next = null;
            // The first link in the chain.
            else
                // Not the only link in the chain.
                if (current.Next != null)
                {
                    Table[Hash] = current.Next;
                    current.Next.Previous = null;
                }
                // The only link in the chain.
                else
                    Table[Hash] = null;

            // Own references.
            current.Previous = null;
            current.Next = null;
        }
    }

    // Raise the score of an existing player.
    static void OutputX(long Hash, string Name, int Amount)
    {
        // Check if the player exists in the chain.
        Speler current = SearchPlayer(Hash, Name);

        // If the player could not be found, print -1, and stop.
        if (current == null)
        {
            Console.WriteLine("-1");
            return;
        }

        // Add the provided amount to the score of the found player.
        current.score += Amount;
    }

    // Print the score of an existing player.
    static void OutputQ(long Hash, string Name)
    {
        // Check if the player exists in the chain.
        Speler current = SearchPlayer(Hash, Name);

        // If the player could not be found, print -1, and stop.
        if (current == null)
        {
            Console.WriteLine("-1");
            return;
        }

        // Print the score of the found player.
        Console.WriteLine(current.score);
    }
}

class Speler
{
    public Speler Previous, Next;
    public readonly string name;
    public long score;

    public Speler(string Name, Speler Previous = null)
    {
        this.name = Name;
        this.score = 0;
        this.Previous = Previous;
        if (Previous != null)
            this.Previous.Next = this;
    }
}