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

    static void Main(string[] args)
    {
        // Setup the temporary queue for customers, Outside.
        Outside = new Outside();

        // Setup piet's stack and the printers' queues.
        piet = new Piet();
        printerA = new Printer('A', piet);
        printerB = new Printer('B', piet);
        printerC = new Printer('C', piet);

        // Recieve all input, and parse to the tempQueue.
        string input = Console.ReadLine();
        string[] InputSplit = input.Split(' ');
        // The customer number, for the results.
        long i = 0;

        // Where we start the simulation; when the first customer enters the store.
        TimeStep = long.Parse(InputSplit[0]);

        // Start setting up all the customers.
        while (input != "sluit")
        {
            i++;
            // Split the input into it's parts.
            InputSplit = input.Split(' ');

            // Add a new customer to the temp queue.
            Outside.Add
            (
                new Klant
                (
                    i,
                    long.Parse(InputSplit[0]),
                    long.Parse(InputSplit[1]),
                    long.Parse(InputSplit[2])
                )
            );

            // Read the next line of input.
            input = Console.ReadLine();
        }

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
            if (Outside.Length > 0 && Outside.CheckNext(TimeStep))
            {
                EnterIntoPrinter(Outside.ReturnDeQueue());
            }

            // Stuff Two and Three: Check if a printer is done or idle.
            printerA.Update(TimeStep);
            printerB.Update(TimeStep);
            printerC.Update(TimeStep);

            // Stuff Four and Five: Check if Piet is done or idle.
            piet.Update(TimeStep);

            // End of this timeStep, go to the next timeStep.
            TimeStep++;
        }

        // Return the results
        // Result one: The customer who has been waiting the longest to print.
        CalculateResultOne();
        // Result two: The customer who has been waiting the longest at piets desk.
        piet.resultTwo.Print();
        // Result three: The customer who has spent the most time in the store.
        piet.resultThree.Print();
        // Result four: When Piet could go home.
        Console.WriteLine("sluitingstijd: " + TimeStep);


        #region DEBUG
        Console.ReadLine();
        #endregion
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
        if (printerA.resultOne.WaitTime >= printerB.resultOne.WaitTime)
        {
            // A is (shared) largest, we use A.
            if (printerA.resultOne.WaitTime >= printerC.resultOne.WaitTime)
                printerA.resultOne.Print();
            // C is the largest, we use C.
            else
                printerC.resultOne.Print();
        }
        // B is larger than A, we will never use A.
        else
        {
            // B is (shared) shortest, we use B.
            if (printerB.resultOne.WaitTime >= printerC.resultOne.WaitTime)
                printerB.resultOne.Print();
            // C is the largest, we use C.
            else
                printerC.resultOne.Print();
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

    // Enters a customer into the store, and returns the customer.
    public Klant ReturnDeQueue()
    {
        Next++;
        return Queue[Next - 1];
    }

    // Checks if the next customer will enter this timestep.
    public bool CheckNext(long TimeStep)
    {
        return Queue[Next].T == TimeStep;
    }

    // Checks how many persons there are outside.
    public long Length
    {
        get { return Last - Next; }
    }
}

public class Printer
{
    char id;
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
    // The customer who has been waiting the longest in line for the printer.
    public KlantRecord resultOne;

    public Printer(char Id, Piet Piet)
    {
        this.id = Id;
        this.Queue = new Klant[700000];
        this.currentJob = -1;
        this.nextJob = 0;
        this.firstNull = 0;
        this.WhenDone = -1;
        this.piet = Piet;
        this.resultOne = new KlantRecord();
    }

    public void Update(long TimeStep)
    {
        // if we just finished on a job, send the job to Piet, and set currentJob to -1.
        if (WhenDone == TimeStep)
        {
            // Record part of the answer to query 2.
            currentCustomer.PrintDone = TimeStep;
            // Send the job to Piet.
            piet.Add(currentCustomer);
            // Set ourselves to idle.
            currentJob = -1;
        }
        // If we are not printing right now, start on the next job if it is available.
        if (currentJob == -1 && nextJob != firstNull)
        {
            // Setup the current job
            currentJob = nextJob;

            // Record the result of the first query.
            if ((TimeStep - currentCustomer.T) > resultOne.WaitTime)
            {
                resultOne.CustNo = currentCustomer.CustNo;
                resultOne.WaitTime = TimeStep - currentCustomer.T;
            }

            // Record when we will be done.
            WhenDone = TimeStep + currentCustomer.P;

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
    // The first element of the stack that is null; not a customer waiting for his plate.
    long firstNull;
    // The timestep we finish with our current work.
    long WhenDone;
    // The customer for whom we are currently cutting a plate.
    Klant currentKlant;
    // The customer who has been waiting on Piet the longest;
    public KlantRecord resultTwo;
    // The customer who has spent the most time in the store.
    public KlantRecord resultThree;


    public Piet()
    {
        Stack = new Klant[700000];
        firstNull = 0;
        WhenDone = -1;
        resultTwo = new KlantRecord();
        resultThree = new KlantRecord();
    }

    public void Update(long TimeStep)
    {
        // If we just finished our work, calculate the result for the customer, and send the customer away.
        if (WhenDone == TimeStep)
        {
            // Calculate the results for the customer we just finished on.
            if (TimeStep - currentKlant.PrintDone > resultTwo.WaitTime)
            {
                resultTwo.CustNo = currentKlant.CustNo;
                resultTwo.WaitTime = TimeStep - currentKlant.PrintDone;
            }
            if (TimeStep - currentKlant.T > resultThree.WaitTime)
            {
                resultThree.CustNo = currentKlant.CustNo;
                resultThree.WaitTime = TimeStep - currentKlant.T;
            }

            currentKlant = null;
        }
        // If we are not cutting right now, check if there are plates waiting, and start on them.
        if (currentKlant == null && firstNull > 0)
        {
            currentKlant = Stack[firstNull - 1];
            WhenDone = TimeStep + currentKlant.S;
            firstNull--;
        }
    }

    // Adds a customer to the stack.
    public void Add(Klant k)
    {
        Stack[firstNull] = k;
        firstNull++;
    }

    // Returns the number of items in the stack.
    public long Length
    {
        get
        {
            if (currentKlant != null)
                return firstNull + 1;
            else
                return firstNull;
        }
    }
}

public class Klant
{
    public readonly long CustNo, T, P, S;
    public long PrintDone;

    public Klant(long CustNo, long T, long P, long S)
    {
        this.T = T;
        this.P = P;
        this.S = S;
        this.CustNo = CustNo;
        this.PrintDone = -1;
    }
}

public class KlantRecord
{
    public long CustNo;
    public long WaitTime;

    public KlantRecord(long CustNo = -1, long WaitTime = -1)
    {
        this.CustNo = CustNo;
        this.WaitTime = WaitTime;
    }

    public void Print()
    {
        Console.WriteLine(CustNo + ": " + WaitTime);
    }
}