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
        Tree = new Tree();

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
            Tree.Root.TryToAdd(newPlayer);
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
                Console.WriteLine(Result[j]._PlayerNumber);
        }
    }

    static void PlayerRank(int PlayerNumber)
    {
        Console.WriteLine(lookup[PlayerNumber].StartRank);
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
    public readonly int _PlayerNumber;
    public int Height, Size, _Booms;

    public TreeNode(int PlayerNumber, int Booms, Tree Tree)
    {
        this.tree = Tree;
        this._PlayerNumber = PlayerNumber;
        this._Booms = Booms;
        this.Height = 1;
        this.Size = 1;
    }

    #region Adding

    public void TryToAdd(TreeNode ToAdd)
    {
        this.Size++;

        // If the key is smaller than the current player.
        if (ToAdd._Booms < this._Booms)
            // if there are still smaller players
            if (this.Left != null)
            {
                this.Left.TryToAdd(ToAdd);
                return;
            }
            // If there are no more smaller players.
            else
            {
                this.Add(ToAdd);
                return;
            }

        // If the key is larger than the current player.
        else
            // If there are still larger players.
            if (this.Right != null)
            {
                this.Right.TryToAdd(ToAdd);
                return;
            }
            // There are no more larger players.
            else
            {
                this.Add(ToAdd);
                return;
            }
    }

    private void Add(TreeNode ToAdd)
    {
        // Add the node to the correct side
        ToAdd.Parent = this;
        if (ToAdd._Booms < this._Booms)
            this.Left = ToAdd;
        else
            this.Right = ToAdd;

        TreeNode current = this;
        bool done = false;
        int preHeight;
        while (!done)
        {
            // Set the height from before any rotating.
            preHeight = current.Height;
            // Calculate a new height for our current node.
            current.CalculateHeight();
            // if current is out of balance.
            if (current.IsLeftHeavy || current.IsRightHeavy)
                current.Balance();
            // y.height was unchanged, and y was in balance.
            else if (current.Height == preHeight || current == tree.Root)
                done = true;
            else
                current = current.Parent;
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
        }
    }

    public void LeftRotation(TreeNode A)
    {
        // Situation: A \ B.
        TreeNode B = A.Right;

        // B takes A's parent as his own.
        B.Parent = A.Parent;

        if (A.Parent == null)
            tree.Root = B;
        else if (A.Parent.Left == A)
            A.Parent.Left = B;
        else
            A.Parent.Right = B;

        // A takes B's left child as it's right child.
        A.Right = B.Left;
        if (B.Left != null)
            B.Left.Parent = A;

        // B takes A as it's left child.
        B.Left = A;
        A.Parent = B;

        // Recalculate the heights of A and B.
        A.CalculateHeight();
        B.CalculateHeight();

        // Recalculate the sizes of A and B.
        B.Size = A.Size;
        if (A.Left != null)
            if (A.Right != null)
                A.Size = A.Left.Size + A.Right.Size + 1;
            else
                A.Size = A.Left.Size + 1;
        else
            if (A.Right != null)
                A.Size = A.Right.Size + 1;
            else
                A.Size = 1;
    }

    public void RightRotation(TreeNode A)
    {
        // Situation: B / A.
        TreeNode B = A.Left;

        // B becomes the new root.
        B.Parent = A.Parent;

        if (A.Parent == null)
            tree.Root = B;
        else if (A.Parent.Left == A)
            A.Parent.Left = B;
        else
            A.Parent.Right = B;

        // A takes B's right child as it's left child.
        A.Left = B.Right;
        if (B.Right != null)
            B.Right.Parent = B;

        // B takes A as it's right child.
        B.Right = A;
        A.Parent = B;

        // Recalculate the heights of B and A.
        B.CalculateHeight();
        A.CalculateHeight();

        // Recalculate the sizes of A and B.
        B.Size = A.Size;
        if (A.Left != null)
            if (A.Right != null)
                A.Size = A.Left.Size + A.Right.Size + 1;
            else
                A.Size = A.Left.Size + 1;
        else
            if (A.Right != null)
                A.Size = A.Right.Size + 1;
            else
                A.Size = 1;
    }

    // Calculates a new height for this node, based on our subtrees.
    public void CalculateHeight()
    {
        if (this.Left != null)
            if (this.Right != null)
                this.Height = Math.Max(Left.Height, Right.Height) + 1;
            else
                this.Height = Left.Height + 1;
        else
            if (this.Right != null)
                this.Height = Right.Height + 1;
            else
                this.Height = 1;
    }

    // If the difference in height between our children is more than one, we are heavy on one side.
    public bool IsLeftHeavy
    {
        get
        {
            // If there is a left child.
            if (this.Left != null)
                // If there is no right child, it's height is 1.
                if (this.Right == null && this.Left.Height > 1)
                    return true;
                // If there is a right child, but the difference is more than 1.
                else if (this.Right != null && this.Left.Height - this.Right.Height > 1)
                    return true;

            // We are either in balance, or we are right heavy.
            return false;
        }
    }

    // If the difference in height between our children is more than one, we are heavy on one side.
    public bool IsRightHeavy
    {
        get
        {
            // If there is a right child.
            if (this.Right != null)
                // If there is no left child, it's height is 1, so we check if our height is more than that.
                if (this.Left == null)
                    if (this.Right.Height > 1)
                        return true;
                    else
                        return false;
                // If there is a left child, and the difference between them is more than 1.
                else
                    if (this.Right.Height - this.Left.Height > 1)
                        return true;
                    else
                        return false;
            else
                // We are either in balance, or we are left heavy.
                return false;
        }
    }

    #endregion

    #region Range

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

    #endregion

    #region Rank

    public int StartRank
    {
        get
        {
            int result = 1, toTest = this._Booms;
            TreeNode successor;

            if (this.Successor != null)
            {
                successor = this.Successor;

                // Get your first successor that has a larger score than you.
                while (successor._Booms == toTest)
                {
                    toTest = successor._Booms;
                    successor = successor.Successor;
                }
                // Add 1 to the rank of your first true successor to get your score;
                result += successor.Rank;
            }

            return result;
        }
    }

    // Get the number of player with a higher score than this player.
    public int Rank
    {
        get
        {
            int result = 1;
            // Get all the players below you with a higher score.
            if (this.Right != null)
                result = this.Right.Size + 1;

            // Go up the tree, and keep adding players with a higher score than you.
            TreeNode current = this;
            while (current != null && current != tree.Root)
            {
                if (current == current.Parent.Left)
                    if (current.Parent.Right != null)
                        result += current.Parent.Right.Size + 1;
                    else
                        result++;
                current = current.Parent;
            }

            // Return all the players with a higher score than you.
            return result;
        }
    }

    #endregion
}