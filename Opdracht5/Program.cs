using System;
using System.IO;

// Naam: Matthijs Platenburg
// Studentnummer: 4260953

public class Program
{
    // The temporary input queue, the 'outside' of the store.
    static Outside Outside;

    // The Queues and Stack.
    static Printer printerA, printerB, printerC;
    static Piet piet;

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

        // Setup the printer queues and piet's stack.
        piet = new Piet();
        printerA = new Printer(piet);
        printerB = new Printer(piet);
        printerC = new Printer(piet);

        // Recieve all input, and parse to the tempQueue.
        string input = Console.ReadLine();
        string[] InputSplit;
        while (input != "sluit")
        {
            // Split the input into its necessary parts.
            InputSplit = input.Split(' ');

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

            // Recieve the next input.
            input = Console.ReadLine();
        }

        // Sort the temp queue, in case the input was not given in order of the customer's entry times.
        Outside.Sort();

        // Setup the timeline.
        TimeStep = Outside.CheckNext.T;

        // Start looping through the timeline, doing the stuff.
        //      The stuff:
        //      - Customer enters the correct Queue.
        //      - If a printer is done, add it to Piet's stack, and calculate the customer's waiting time: Result 1.
        //      - If a printer is done, start on the next customer.
        //      - If Piet is done, The customer picks up his plate, and calculate the customer's waiting time: Result 2 & 3.
        //      - If Piet is done, He picks up the next plate from the stack.
        // Repeat loop until the queues and piet's stack are empty: Result 4.
        while (QueuesNotEmpty && TimeStep < 2100000000)
        {
            // Stuff one: Enter a customer, if their T == the timeStep.
            if (Outside.Length > 0)
            {
                Klant Next = Outside.CheckNext;
                if (Next.T == TimeStep)
                    EnterIntoPrinter(Outside.ReturnDeQueue());
            }
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

    // Search for the shortest Printer Queue, and add the customer to its queue.
    static void EnterIntoPrinter(Klant k)
    {
        // A is shorter than or equal to B, we will never chose B
        if (printerA.Length <= printerB.Length)
        {
            // A has the (shared) shortest line, we use A.
            if (printerA.Length <= printerC.Length)
                // Add it to A's queue.
                printerA.EnQueue(k);
            // C has the shortest line, we use C.
            else
                // Add it to C's queue.
                printerC.EnQueue(k);
        }
        // B is shorter than A, we will never use A
        else
        {
            // B has the (shared) shortest line, we use B.
            if (printerB.Length <= printerC.Length)
                // Add it to B's queue.
                printerB.EnQueue(k);
            // C has the shortest line, we use C.
            else
                // Add it to C's queue.
                printerC.EnQueue(k);
        }
    }

    // Indicates whether all queues and stacks are empty, so we can stop looping through the timeline.
    static bool QueuesNotEmpty
    {
        get
        {
            // If there are still customers outside, we can never be done.
            if (Outside.Length > 0)
                return false;
            // If there are no more customers outside, check if there are still customers inside.
            else
                return ((printerA.Length + printerB.Length + printerC.Length + piet.Length) > 0);
        }
    }
}

public struct Outside
{
    Klant[] Queue;
    // The first element, inclusive, and the last element, exclusive.
    long Next, Last;

    public Outside()
    {
        Queue = new Klant[2100000];
        Next = 0;
        Last = 0;
    }

    public void Add(Klant k)
    {
        Queue[Last] = k;
        Last++;
    }

    // Sorts the customers in order of the time they enter the store, then it assigns them their Customer Number.
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
        for (long i = 0; i < Last; i++)
            Queue[i].CustNo = i + 1;
    }

    // Enters a customer into the store, but don't return it.
    public void DeQueue()
    {
        Next++;
    }

    // Enters a customer into the store, and return the customer.
    public Klant ReturnDeQueue()
    {
        Next++;
        return Queue[Next - 1];
    }

    // Checks the next customer waiting to go inside.
    public Klant CheckNext
    {
        get { return Queue[Next]; }
    }

    // Checks how many persons there are outside.
    public long Length
    {
        get { return Last - Next; }
    }
}

public struct Printer
{
    Klant[] Queue;
    // The current/previous customer, and the last customer waiting in line.
    long First, Last;
    Piet piet;
    long WhenDone;
    bool busy;
    KlantRecord currentRecord;

    public Printer(Piet Piet)
    {
        Queue = new Klant[700000];
        First = 0;
        Last = 0;

        busy = false;
        WhenDone = -1;

        // The instance of piet's stack, so we can add to it.
        piet = Piet;

        // The record of the customer who has been waiting the longest.
        currentRecord = new KlantRecord(0, -1);
    }

    public void Update(long TimeStep)
    {
        // Check if we need to start on the next customer right now.
        // If we have been doing nothing the last update, check if there is a customer in our queue.
        if (!busy)
        {
            NextCustomer(TimeStep);
        }
        // If we finished printing this update, send the current customer to Piet. Then start on the next one, if the queue is not empty.
        else if (WhenDone == TimeStep)
        {
            // Send the current customer to Piet.
            piet.Add(Queue[First]);

            // Start on the next customer.
            NextCustomer(TimeStep);
        }
    }

    // Adds a customer to the queue.
    public void EnQueue(Klant k)
    {
        Queue[Last] = k;
        Last = (Last + 1) % 700000;
    }

    // Starts on the next customer in the line, if there is one.
    private void NextCustomer(long TimeStep)
    {
        // If there are customers waiting.
        if (this.Length > 0)
        {
            First = (First + 1) % 700000;

            // Check the time they have been waiting in line, if that is more than the current record, record it.
            Klant nextKlant = Queue[First];
            if (TimeStep - nextKlant.T > currentRecord.Time)
                currentRecord = new KlantRecord(nextKlant.CustNo, TimeStep - nextKlant.T);

            // Start on the next customer.
            WhenDone += nextKlant.P;
            busy = true;
        }

        // If there are no customers waiting, go into idle mode.
        else
            busy = false;
    }

    // Returns the number of customers waiting in line.
    public long Length
    {
        get
        {
            // If the queue does not extend past the end of the array.
            if (Last >= First)
                return Last - First;
            // If the queue extends past the end of the array.
            else
                return Queue.LongLength - (First - Last);
        }
    }
}

public struct Piet
{
    // The stack of customers waiting.
    Klant[] Stack;
    // The topmost element of the stack, inclusive.
    long top;
    // Whether we are printing or not.
    bool busy;
    // The timestep we finish with our current work.
    long WhenDone;

    public Piet()
    {
        Stack = new Klant[700000];
        // For an empty stack, the top element will be 0, and will always be an empty customer.
        top = -1;
        WhenDone = -1;
        busy = false;
    }

    public void Update(long TimeStep)
    {
        // If we have not been doing anything the last update, or we just finished our work, 
        // check if there is a customer, and start on them.
        if (!busy || WhenDone == TimeStep)
        {
            // Start on the next customer.
            NextCustomer();
        }
    }

    // Check is there is a customer waiting on the stack.
    void NextCustomer()
    {
        // If there are customers on the stack, start on them.
        if (top >= 0)
        {
            WhenDone += Stack[top].S;
            top--;
            busy = true;
        }
        // If there are no customers on the stack, go into idle mode.
        else
            busy = false;
    }

    // Adds a customer to the stack.
    public void Add(Klant k)
    {
        if (top < 700000)
        {
            top++;
            Stack[top] = k;
        }
    }

    // Returns the number of items in the stack.
    public long Length
    {
        get { return top + 1; }
    }
}

public struct Klant
{
    public readonly long T, P, S;
    public long CustNo;

    public Klant(long T, long P, long S)
    {
        this.T = T;
        this.P = P;
        this.S = S;
        CustNo = -1;
    }
}

public struct KlantRecord
{
    public long CustNo;
    public long Time;

    public KlantRecord(long CustNo, long Time)
    {
        this.CustNo = CustNo;
        this.Time = Time;
    }
}