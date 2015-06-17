using System;
using System.IO;

// Naam: Matthijs Platenburg
// Studentnummer: 4260953

// Soort Boom: TODO

class Program
{
    static TreeNode[] lookup;
    static Tree Tree;

    static void Main(string[] args)
    {
        // Setup.
        lookup = new TreeNode[1000000];

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
            else if (instruction == "G")
                PrintRange(playerNumber);

            // Player's rank.
            else if (instruction == "R")
                PlayerRank(playerNumber);

            // Recieve next input.
            input = Console.ReadLine();
        }
    }

    static void AddNewPlayer(int PlayerNumber, int PlayerScore)
    {
        TreeNode newPlayer = new TreeNode(PlayerNumber, PlayerScore, Tree);

        // Add the new player to the lookup table.
        lookup[PlayerNumber] = newPlayer;

        // Add the new player to the tree.
        // If this is the first player.
        if (Tree.Root == null)
            Tree.Root = newPlayer;
        // This is not the first player.
        else
            Tree.Root.Add(newPlayer);
    }

    static void PrintRange(int PlayerNumber)
    {
        TreeNode[] Result = new TreeNode[10];
        int i = 4;

        // Add the current player to the table.
        Result[i] = lookup[PlayerNumber];
        // Get the players with a smaller score.
        while (Result[i] != null && i > 0)
        {
            Result[i - 1] = Result[i].Predecessor;
            i--;
        }

        // Get the players with a larger score.
        i = 4;
        while (Result[i] != null && i < 9)
        {
            Result[i + 1] = Result[i].Successor;
            i++;
        }

        // Print the table.

        for (int j = 9; j >= 0; j--)
        {
            if (Result[j] != null)
                Console.WriteLine();
        }
    }

    static void PlayerRank(int PlayerNumber)
    {

    }
}

class Tree
{
    public TreeNode Root;
}

class TreeNode
{
    public Tree tree;
    public TreeNode Parent, Left, Right;
    public int Depth, Height;
    public readonly int PlayerNumber;
    public int Booms;

    public TreeNode(int PlayerNumber, int Booms, Tree Tree)
    {
        this.tree = Tree;
        this.PlayerNumber = PlayerNumber;
        this.Booms = Booms;
        this.Height = 1;
    }

    public void Add(TreeNode ToAdd)
    {
        // If the key is smaller than the current player.
        if (ToAdd.Booms < this.Booms)
            // if there are still smaller players
            if (this.Left != null)
            {
                this.Left.Add(ToAdd);
                return;
            }
            // If there are no more smaller players.
            else
            {
                this.Left = ToAdd;
                ToAdd.Parent = this;
                if (this.Height < 2)
                {
                    this.Height = 2;
                }
                Balance();
                
                return;
            }

        // If the key is larger than the current player.
        else
            // If there are still larger players.
            if (this.Right != null)
            {
                this.Right.Add(ToAdd);
                return;
            }
            // There are no more larger players.
            else
            {
                this.Right = ToAdd;
                ToAdd.Parent = this;
                if (this.Height < 2)
                    this.Height = 2;
                Balance();
                return;
            }
    }

    public void Balance()
    {
        // If current is right heavy.
        if (this.IsRightHeavy)
        {
            // If current's right is left heavy.
            if (this.Right != null && this.Right.IsLeftHeavy)
            // Double Left Rotation.
            {
                // First we perform a single right rotation on our right child.
                RightRotation(this.Right);
                // Next we perform a single left rotation on ourself.
                LeftRotation(this);
            }
            // If current's right is not left heavy.
            else
            // Single Left Rotation.
            {
                LeftRotation(this);
            }

            // Our balance has changed, so we have to balance our parent as well, unless we are the root.
            if (this.Parent != null)
                this.Parent.Balance();
        }
        // If current is left heavy.
        else if (this.IsLeftHeavy)
        {
            // If current's left is right heavy.
            if (this.Left != null && this.Left.IsRightHeavy)
            // Double Right Rotation.
            {
                // First we perform a single left rotation on our left child.
                LeftRotation(this.Left);
                // Next we perform a single right rotation on ourself.
                RightRotation(this);
            }
            // If current's left is not right heavy.
            else
            // Single Right Rotation.
            {
                RightRotation(this);
            }

            // Our balance has changed, so we have to balance our parent as well, unless we are the root.
            if (this.Parent != null)
                this.Parent.Balance();
        }

        // Our balance did not change, or we are already stopping, we stop.
        return;
    }

    public void LeftRotation(TreeNode A)
    {
        // Situation: A \ B \ C.
        TreeNode B = A.Right;
        
        // B becomes the new root.
        B.Parent = A.Parent;

        if (A.Parent == null)
            tree.Root = B;

        // A takes B's left child as it's right child.
        A.Right = B.Left;
 
        // B takes A as it's left child.
        B.Left = A;
    }

    public void RightRotation(TreeNode C)
    {
        // Situation: C / B / A.
        TreeNode B = C.Left;

        // B becomes the new root.
        B.Parent = C.Parent;

        if (C.Parent == null)
            tree.Root = B;

        // C takes B's right child as it's left child.
        C.Left = B.Right;

        // B takes C s it's right child.
        B.Right = C;
    }

    // Gets the maximum number of steps from this node to the bottom of the tree.
    //public int Height
    //{
    //    get
    //    {
    //        if (this.Left != null)
    //            if (this.Right != null)
    //                return Math.Max(Left.Height, Right.Height) + 1;
    //            else
    //                return Left.Height + 1;
    //        else
    //            if (this.Right != null)
    //                return Right.Height + 1;
    //            else
    //                return 1;
    //    }
    //}

    // Gets the parent's other child.
    public TreeNode Sibling
    {
        get
        {
            if (this.Parent == null)
                return null;
            if (this == this.Parent.Left)
                return this.Parent.Right;
            else
                return this.Parent.Left;
        }
    }

    // Gets the  first node that is larger than this one.
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

    public bool IsLeftHeavy
    {
        get
        {
            // If there is a left child.
            if (this.Left != null)
                // If there is no right child.
                if (this.Right == null)
                    return true;
                // If there is a right child, but it is smaller than our left child
                else if (this.Right.Height < this.Left.Height)
                    return true;
            // We are either in balance, or we are right heavy.
            return false;
        }
    }

    public bool IsRightHeavy
    {
        get
        {
            // If there is a right child.
            if (this.Right != null)
                // If there is no left child/
                if (this.Left == null)
                    return true;
                // If there is a left child, but it is smaller than our right child.
                else if (this.Left.Height < this.Right.Height)
                    return true;
            // We are either in balance, or we are left heavy.
            return false;
        }
    }
}