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
    public readonly int PlayerNumber;
    public int Booms;

    // Tree
    public Speler Parent;
    public Speler Left, Right;

    public Speler(int PlayerNumber, int Booms)
    {
        this.PlayerNumber = PlayerNumber;
        this.Booms = PlayerNumber;
    }
}

class TreeNode
{
    public TreeNode Parent, Left, Right;
    public readonly Speler Player;
    public int Depth;

    public TreeNode(Speler Speler, TreeNode Parent = null)
    {
        this.Player = Speler;
    }

    // Gets the maximum number of steps from this node to the bottom of the tree.
    public int Height
    {
        get
        {
            if (this.Left != null)
                if (this.Right != null)
                    return Math.Max(Left.Height, Right.Height) + 1;
                else
                    return Left.Height + 1;
            else
                if (this.Right != null)
                    return Right.Height + 1;
                else
                    return 1;
        }
    }

    public TreeNode Predecessor
    {
        get
        {
            // If the successor is under the node, go left once, then keep going right until we have reached the bottom.
            if (this.Left != null)
            {
                TreeNode result = this.Left;
                while (result != null && result.Right != null)
                    result = result.Right;
                return result;
            }
                // If the successor is above the node, go up the tree, until we are no longer the left child
            else
            {
                TreeNode current = this;
                TreeNode parent = current.Parent;
                while (parent != null && current == parent.Left)
                {
                    current = parent;
                    parent = current.Parent;
                }
                return parent;
            }
        }
    }


    // Gets the first node that is bigger than the current. 
    public TreeNode Successor
    {
        get
        {
            // If the successor is under the node, go right once, and then keep going left until we have reached the bottom.
            if (this.Right != null)
            {
                TreeNode result = this.Right;
                while (result != null && result.Left != null)
                    result = result.Left;
                return result;
            }
            // If the successor is above the node, go up the tree, until we are no longer a right child.
            else
            {
                TreeNode current = this;
                TreeNode parent = current.Parent;
                while (parent != null && current == parent.Right)
                {
                    current = parent;
                    parent = current.Parent;
                }
                return parent;
            }
        }
    }
}