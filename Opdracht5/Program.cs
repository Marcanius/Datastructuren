using System;
using System.IO;

// Naam: Matthijs Platenburg
// Studentnummer: 4260953

public class Program
{
    // The temporary input queue, the 'outside' of the store.
    static Klant[] Outside;
    // The first available spot in the outside array;
    static long OutsideAvailable;
    static Random random;

    // Which customer stood in line for the printer the longest.
    string resultOne;
    // Which customer stood in line for Piet the longest.
    string resultTwo;
    // Which customer just would not leave the store.
    string resultThree;
    // When could Piet leave.
    string resultFour;

    static void Main(string[] args)
    {
        // Setup the temporary queue for customers, Outside.
        Outside = new Klant[2100000];
        OutsideAvailable = 0;
        random = new Random();

        // Recieve all input, and parse to the tempQueue.
        string input = Console.ReadLine();
        while (input != "sluit")
        {
            string[] InputSplit = input.Split(' ');
            InputKlant(long.Parse(InputSplit[0]), long.Parse(InputSplit[1]), long.Parse(InputSplit[2]));
        }

        // Sort the temp queue.
        RandomizedQuicksort(Outside, 0, OutsideAvailable);


        // Start looping through the timeline, doing the stuff.
        //      The stuff:
        //      - Customer enters the correct Queue.
        //      - If a printer is done, add it to Piet's stack, and calculate the customer's waiting time: Result 1.
        //      - If a printer is done, start on the next customer.
        //      - If Piet is done, The customer picks up his plate, and calculate the customer's waiting time: Result 2 & 3.
        //      - If Piet is done, He picks up the next plate from the stack.
        // Repeat loop until the queues and piet's stack are empty: Result 4.

        // return the results
    }

    static void InputKlant(long t, long p, long s)
    {
        // Create a customer.
        Klant klant = new Klant(t, p, s);
        // Add to the temp queue.
        Outside[OutsideAvailable] = klant;
        // Self-Explanatory.
        OutsideAvailable++;
    }

    static long RandomizedPartition(Klant[] A, long p, long r)
    {
        // Determine a random pivot.
        long pivot = random.Next((int)p, (int)r);
        // Switch the pivot and the last element of the range.
        Klant temp = A[r];
        A[r] = A[pivot];
        A[pivot] = temp;
        
        // 

        return pivot;
    }

    static void RandomizedQuicksort(Klant[] A, long p, long r)
    {
        if (p < r)
        {
            long pivot = RandomizedPartition(A, p, r);

            RandomizedQuicksort(A, p, pivot - 1);
            RandomizedQuicksort(A, pivot + 1, r);
        }
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
    long t, p, s;

    public Klant(long T, long P, long S)
    {
        this.t = T;
        this.p = P;
        this.s = S;
    }
}

/* while input != sluit
        while tijd < input.Parse.Split.1
            Doe dingen    
        input
 
    Werk door totdat alle queues en piet's bak leeg zijn.
 */