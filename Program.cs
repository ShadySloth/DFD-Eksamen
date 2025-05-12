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
                    BenchmarkRelational();
                    break;
                case "2":
                    BenchmarkNoSql();
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

    private static void BenchmarkRelational()
    {
        // Code to benchmark relational databases
        Console.WriteLine();
        Console.WriteLine("Pick a benchmarking method:");
        Console.WriteLine("\t1. Insert");
        Console.WriteLine("\t2. Query");
        Console.WriteLine("\t3. Update");
        Console.WriteLine("\t4. Delete");
        Console.WriteLine("\t5. All");
        Console.WriteLine();
        
        Console.Write("Your choice: ");
        switch (Console.ReadLine())
        {
            case "1":
                Console.WriteLine("Benchmarking Insert...");
                break;
            case "2":
                Console.WriteLine("Benchmarking Query...");
                break;
            case "3":
                Console.WriteLine("Benchmarking Update...");
                break;
            case "4":
                Console.WriteLine("Benchmarking Delete...");
                break;
            case "5":
                Console.WriteLine("Benchmarking All...");
                break;
        }
        // Add your benchmarking logic here
    }

    private static void BenchmarkNoSql()
    {
        // Code to benchmark NoSQL databases
        Console.WriteLine();
        Console.WriteLine("Pick a benchmarking method:");
        Console.WriteLine("\t1. Insert");
        Console.WriteLine("\t2. Query");
        Console.WriteLine("\t3. Update");
        Console.WriteLine("\t4. Delete");
        Console.WriteLine("\t5. All");
        Console.WriteLine();
        
        Console.Write("Your choice: ");
        switch (Console.ReadLine())
        {
            case "1":
                Console.WriteLine("Benchmarking Insert...");
                break;
            case "2":
                Console.WriteLine("Benchmarking Query...");
                break;
            case "3":
                Console.WriteLine("Benchmarking Update...");
                break;
            case "4":
                Console.WriteLine("Benchmarking Delete...");
                break;
            case "5":
                Console.WriteLine("Benchmarking All...");
                break;
        }
        // Add your benchmarking logic here
    }
}