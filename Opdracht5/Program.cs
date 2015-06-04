using System;
using System.IO;

// Naam: Matthijs Platenburg
// Studentnummer: 4260953

public class Program
{
    // The temporary input queue, the 'outside' of the store.
    static Outside Outside;
    // The first available spot in the outside array;
    static long OutsideAvailable;
    static Random random;

    // The Queues and Stack.
    static Printer printerA, printerB, printerC;
    static Piet piet;

    // Indicates whether all queues and stacks are empty, so we can stop looping through the timeline.
    static bool QueuesNotEmpty;
    // Where we are in the timeline.
    static long TimeStep;

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
        Outside = new Outside();
        OutsideAvailable = 0;
        random = new Random();

        piet = new Piet();
        printerA = new Printer(piet);
        printerB = new Printer(piet);
        printerC = new Printer(piet);

        // Recieve all input, and parse to the tempQueue.
        string input = Console.ReadLine();
        while (input != "sluit")
        {
            string[] InputSplit = input.Split(' ');
            // Add a new customer to the temp queue.
            Outside.Add
            (
                new Klant
                (
                    long.Parse(InputSplit[0]),
                    long.Parse(InputSplit[1]),
                    long.Parse(InputSplit[2])
                )
            );
        }

        // Sort the temp queue.
        Outside.Sort();

        // Setup the timeline.
        QueuesNotEmpty = false;
        TimeStep = Outside.CheckNext.T;

        // Start looping through the timeline, doing the stuff.
        //      The stuff:
        //      - Customer enters the correct Queue.
        //      - If a printer is done, add it to Piet's stack, and calculate the customer's waiting time: Result 1.
        //      - If a printer is done, start on the next customer.
        //      - If Piet is done, The customer picks up his plate, and calculate the customer's waiting time: Result 2 & 3.
        //      - If Piet is done, He picks up the next plate from the stack.
        // Repeat loop until the queues and piet's stack are empty: Result 4.
        while (QueuesNotEmpty && TimeStep <= 2100000000)
        {
            // Stuff one: Enter a customer, if their T == the timeStep.
            Klant Next = Outside.CheckNext;
            if (Next.T == TimeStep)
                EnterIntoPrinter(Outside.ReturnDeQueue());

            // Stuff Two and Three: Check if a printer is done.
            printerA.Update(TimeStep);
            printerB.Update(TimeStep);
            printerC.Update(TimeStep);

            // Stuff Four and Five: Check if Piet is done.
            piet.Update(TimeStep);

            // End of this timeStep, go to the next timeStep.
            TimeStep++;
        }

        // return the results
    }

    static void EnterIntoPrinter(Klant k)
    {
        // Search for the shortest Printer Queue, and add the customer to its queue.
        // A is shorter than or equal to B, we will never chose B
        if (printerA.Length <= printerB.Length)
        {
            // A is the shortest, or shared shortest, we use A.
            if (printerA.Length <= printerC.Length)
            {
                // Add it to A's queue.
                // TODO
            }
            // C is the shortest, we use C
            else
            {
                // Add it to C's queue.
                // TODO
            }
        }
        // B is shorter than A, we will never use A
        else
        {
            // B is shorter than or equal to C, we use B.
            if (printerB.Length <= printerC.Length)
            {
                // Add it to B's queue.
                // TODO
            }
            // C is the shortest, we use C.
            else
            {
                // Add it to C's queue.
                // TODO
            }
        }
    }
}

public struct Outside
{
    Klant[] Queue;
    // The first element, inclusive, and the last element, exclusive.
    long First, Last;

    public Outside()
    {
        Queue = new Klant[2100000];
        First = 1;
        Last = 1;
    }

    public void Add(Klant k)
    {
        Queue[Last] = k;
        Last++;
    }

    public void Sort()
    {
        Klant Temp;

        // Check every customer from the second to the last.
        for (long j = 1; j < Last; j++)
        {
            long i = j;
            while (i > 0 && Queue[i - 1].T > Queue[i].T)
            {
                // Switch A[i] and A[i-1].
                Temp = Queue[i - 1];
                Queue[i - 1] = Queue[i];
                Queue[i] = Temp;

                // Lower i, so we check the next/previous value.
                i--;
            }
        }
    }

    // Enters a customer into the store, but don't return it.
    public void DeQueue()
    {
        First++;
    }

    // Enters a customer into the store, and return the customer.
    public Klant ReturnDeQueue()
    {
        First++;
        return Queue[First - 1];
    }

    // Checks the next customer waiting to go inside.
    public Klant CheckNext
    {
        get { return Queue[First]; }
    }

    // Checks how many persons there are outside.
    public long Length
    {
        get { return Last - First; }
    }
}

public struct Printer
{
    Klant[] Queue;
    // The current
    long First, Last;
    Piet piet;
    long WhenDone;
    bool busy;

    public Printer(Piet Piet)
    {
        Queue = new Klant[700000];
        First = 0;
        Last = 0;
        WhenDone = -1;
        piet = Piet;
        busy = false;
    }

    public void Update(long TimeStep)
    {
        // Check if we need to start on the next customer right now.
        // If we have been doing nothing the last update, check if there is a customer in our queue.
        if (!busy)
        {
            NextCustomer(false);
        }
        // If we finished printing this update, check if there is a customer in our queue, and start on them
        else if (WhenDone == TimeStep)
        {
            NextCustomer(true);
        }
    }

    public void EnQueue(Klant k)
    {
        Queue[Last] = k;
        Last++;
    }

    private void NextCustomer(bool JustDone)
    {
        // If we
        if (JustDone)
            First++;
        if (this.Length > 0)
        {
            WhenDone += Queue[First].P;
            busy = true;
        }
        else
            busy = false;
    }

    public long Length
    {
        get { return Last - First; }
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
    public long T, P, S;

    public Klant(long T, long P, long S)
    {
        this.T = T;
        this.P = P;
        this.S = S;
    }
}

/* while input != sluit
        while tijd < input.Parse.Split.1
            Doe dingen    
        input
 
    Werk door totdat alle queues en piet's bak leeg zijn.
 */