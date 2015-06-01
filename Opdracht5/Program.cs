using System;
using System.IO;

// Naam: Matthijs Platenburg
// Studentnummer: 4260953

public class Program
{
    // Which customer stood in line for the printer the longest
    string resultOne;
    // Which customer stood in line for Piet the longest
    string resultTwo;
    // Which customer just would not leave the store
    string resultThree;
    // When could Piet leave
    string resultFour;

    static void Main(string[] args)
    {
        // Recieve first input
        // Start loop
        // Loop through the timeline until we reached the next customer
        // Recieve next input
        // Repeat loop until input == sluit

        // return the results
    }

    void InputKlant(int t, int p, int s)
    {
        // Create a customer.
        Klant klant = new Klant(t, p, s);
        // Add to the correct queue.

    }
}

public struct Printer
{
    Klant[] Queue;

    public Printer()
    {
        Queue = new Klant[700000];
    }

}

public struct Piet
{
    Klant[] Stack;
    // The topmost element, inclusive.
    long top;

    public Piet()
    {
        Stack = new Klant[700000];
        // For an empty stack, the top element will be 0, and will always be an empty customer.
        top = 0;
    }

    public Klant Pop()
    {
        if (top > 0)
        {
            top--;
            return Stack[top + 1];
        }
        else
            return new Klant(0, 0, 0);
    }

    public void Pop()
    {
        if (top > 0)
            top--;
    }

    public void Push(Klant k)
    {
        if (top < 700000)
        {
            top++;
            Stack[top] = k;
        }
    }
}

public struct Klant
{
    int t, p, s;

    public Klant(int T, int P, int S)
    {
        this.t = T;
        this.p = P;
        this.s = S;
    }
}