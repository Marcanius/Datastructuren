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
    KlantRecord resultOne;
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

        // Setup piet's stack and the printers' queues.
        piet = new Piet();
        printerA = new Printer(piet);
        printerB = new Printer(piet);
        printerC = new Printer(piet);

        // Recieve all input, and parse to the tempQueue.
        string input = Console.ReadLine();
        string[] InputSplit;
        while (input != "sluit")
        {
            // Split the input into it's parts.
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

            // Read the next line of input.
            input = Console.ReadLine();
        }

        // Sort the temp queue.
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
        // Result one: The customer who has been waiting the longest to print.
        CalculateResultOne();

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
                printerA.EnQueue(k);
                return;
            }
            // C is the shortest, we use C
            else
            {
                // Add it to C's queue.
                printerC.EnQueue(k);
                return;
            }
        }
        // B is shorter than A, we will never use A
        else
        {
            // B is shorter than or equal to C, we use B.
            if (printerB.Length <= printerC.Length)
            {
                // Add it to B's queue.
                printerB.EnQueue(k);
                return;
            }
            // C is the shortest, we use C.
            else
            {
                // Add it to C's queue.
                printerC.EnQueue(k);
                return;
            }
        }
    }

    static void CalculateResultOne()
    {
        // A is larger than, or equal to B, we will never choose B.
        if (printerA.currentRecord.WaitTime >= printerB.currentRecord.WaitTime)
        {
            // A is (shared) largest, we use A.
            if (printerA.currentRecord.WaitTime >= printerC.currentRecord.WaitTime)
                Console.WriteLine(printerA.currentRecord.CustNo + ": " + printerA.currentRecord.WaitTime);
            // C is the largest, we use C.
            else
                Console.WriteLine(printerC.currentRecord.CustNo + ": " + printerC.currentRecord.WaitTime);
        }
        // B is larger than A, we will never use A.
        else
        {
            // B is (shared) shortest, we use B.
            if (printerB.currentRecord.WaitTime >= printerC.currentRecord.WaitTime)
                Console.WriteLine(printerB.currentRecord.CustNo + ": " + printerB.currentRecord.WaitTime);
            // C is the largest, we use C.
            else
                Console.WriteLine(printerC.currentRecord.CustNo + ": " + printerC.currentRecord.WaitTime);
        }
    }

    // Indicates whether all queues and stacks are empty, so we can stop looping through the timeline.
    static bool QueuesNotEmpty
    {
        get
        {
            // If there are still customers outside, we can never be done.
            if (Outside.Length > 0)
                return true;
            // If there are no more customers outside, check if there are still customers inside.
            else
                return ((printerA.Length + printerB.Length + printerC.Length + piet.Length) > 0);
        }
    }
}

public class Outside
{
    // The queue of customers waiting to go inside.
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

    // Enters a customer into the store, but does not return it.
    public void DeQueue()
    {
        Next++;
    }

    // Enters a customer into the store, and returns the customer.
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

public class Printer
{
    // The queue of customers waiting to print.
    Klant[] Queue;
    // The current/previous customer, and the last customer waiting in line.
    long currentJob;
    long nextJob;
    long firstNull;
    // An instance of Piet, so we can add prints to his stack.
    Piet piet;
    // The TimeStep when the printer is done with it's current print.
    long WhenDone;
    // Whether the printer is printing right now.
    bool busy;
    // The customer who has been waiting the longest in line for the printer.
    public KlantRecord currentRecord;

    public Printer(Piet Piet)
    {
        Queue = new Klant[700000];
        currentJob = -1;
        nextJob = 0;
        firstNull = 0;
        WhenDone = -1;
        piet = Piet;
        currentRecord = new KlantRecord(-1, -1);
    }

    public void Update(long TimeStep)
    {
        // if we just finished on a job, send the job to Piet, and set currentJob to -1.
        if (WhenDone == TimeStep)
        {
            // Record part of the answer to query 2.
            // TODO
            // Send the job to Piet.
            piet.Add(currentCustomer);
            // Set ourselves to idle.
            currentJob = -1;
        }
        // If we are not printing right now, check if we can start on the next customer.
        if (currentJob == -1 && nextJob != firstNull)
        {
            // Setup the current job
            currentJob = nextJob;

            // Record the result of the first query.
            if ((TimeStep - currentCustomer.T) > currentRecord.WaitTime)
                currentRecord = new KlantRecord(currentCustomer.CustNo, TimeStep - currentCustomer.T);

            // Record when we will be done.
            WhenDone = TimeStep + Queue[currentJob].P;

            // Setup the next job.
            nextJob = (nextJob + 1) % 700000;
        }
    }

    public void EnQueue(Klant k)
    {
        // Add the customer to the queue.
        Queue[firstNull] = k;
        firstNull = (firstNull + 1) % 700000;
    }

    // Returns the customer we are working on right now.
    private Klant currentCustomer
    {
        get
        {
            if (currentJob != -1)
                return Queue[currentJob];
            else
                return null;
        }
    }

    // Return the length of the queue.
    public long Length
    {
        get
        {
            if (currentJob != -1)
                return firstNull - currentJob;
            else
                return firstNull - nextJob;
        }
    }
}

public class Piet
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
        // If we have not been doing anything the last update, check if there is a customer, and start on them.
        if (!busy)
        {
            // Start on the next customer.
            NextCustomer(TimeStep);
        }
        // If we just finished our work, calculate the result for the customer.
        else if (WhenDone == TimeStep)
            // Calculate the result for the customer we just finished on.
            // TODO
            // Start on the next customer.
            NextCustomer(TimeStep);
    }

    // Check is there is a customer waiting on the stack.
    void NextCustomer(long TimeStep)
    {
        // If there are customers on the stack, start on them.
        if (top >= 0)
        {
            WhenDone = TimeStep + Stack[top].S;
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

public class Klant
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

public class KlantRecord
{
    public long CustNo;
    public long WaitTime;

    public KlantRecord(long CustNo, long WaitTime)
    {
        this.CustNo = CustNo;
        this.WaitTime = WaitTime;
    }
}