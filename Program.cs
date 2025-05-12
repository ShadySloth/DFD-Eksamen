class Program
{
    static void Main(string[] args)
    {
        var endApp = false;

        Console.WriteLine("Database Benchmarking.\r");
        Console.WriteLine("----------------------\n");

        while (!endApp)
        {
            Console.WriteLine("Pick a database to benchmark:");
            Console.WriteLine("\t1. Relational");
            Console.WriteLine("\t2. NoSQL");
            Console.WriteLine();
            Console.Write("Your choice: ");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine("Relational");
                    break;
                case "2":
                    Console.WriteLine("NoSQL");
                    break;
            }

            Console.WriteLine();
            Console.WriteLine("-------------------\n");
            Console.WriteLine("Press 'q' to quit the app, or any key to continue...");
            
            if (Console.ReadLine() == "q") endApp = true;
            
            Console.WriteLine("\n");
        }

        return;
    }
}