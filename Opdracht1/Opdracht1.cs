using System;

// Naam: Matthijs Platenburg
// Studentnummer: 4260953
class Program
{
    static void Main(string[] args)
    {
        int numberOfLines;
        string line;
        string[] lineSplit;

        line = Console.ReadLine();
        numberOfLines = int.Parse(line);

        for (int i = 0; i < numberOfLines; i++)
        {
            line = Console.ReadLine();
            lineSplit = line.Split(' ');
            Console.WriteLine(int.Parse(lineSplit[0]) + int.Parse(lineSplit[1]));
        }
    }
}
