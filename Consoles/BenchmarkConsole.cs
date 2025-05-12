namespace Database_Benchmarking.Consoles;

public static class BenchmarkConsole
{
    public static void Run()
    {
        var endApp = false;

        Console.WriteLine("Database Benchmarking.\r");
        Console.WriteLine("----------------------\n");

        Console.WriteLine("Inputting 'q' at any time will quit the app.\n");

        while (!endApp)
        {
            Console.WriteLine("Pick a database to benchmark:");
            Console.WriteLine("\t1. Relational");
            Console.WriteLine("\t2. NoSQL");
            Console.WriteLine();
            Console.Write("Your choice: ");

            var choice = GetValidInput(["1", "2", "q"]);

            switch (choice)
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

            if (Console.ReadKey(true).KeyChar == 'q') endApp = true;

            Console.WriteLine("\n");
        }
    }

    private static string GetValidInput(string[] validInputs)
    {
        string input;
        do
        {
            input = Console.ReadLine()!;
            if (input == "q")
            {
                Console.WriteLine("Quitting the app.");
                Environment.Exit(0);
            }

            if (Array.Exists(validInputs, inputs => inputs == input))
                return input;

            Console.WriteLine("Invalid input. Please try again.");
            Console.Write("Your choice: ");
        } while (true);
    }

    private static int GetNumberInput()
    {
        Console.WriteLine();
        Console.WriteLine("Enter the number of times to benchmark.");
        Console.WriteLine();
        while (true)
        {
            Console.Write("Your choice: ");

            var input = Console.ReadLine();
            
            if (input == "q")
            {
                Console.WriteLine("Quitting the app.");
                Environment.Exit(0);
            }
            
            if (int.TryParse(input, out var number))
            {
                return number;
            }

            Console.WriteLine("Invalid input. Please enter a number.");
        }
    }

    private static void BenchmarkRelational()
    {
        Console.WriteLine();
        Console.WriteLine("Pick a benchmarking method:");
        Console.WriteLine("\t1. Insert");
        Console.WriteLine("\t2. Query");
        Console.WriteLine("\t3. Update");
        Console.WriteLine("\t4. Delete");
        Console.WriteLine("\t5. All");
        Console.WriteLine();

        Console.Write("Your choice: ");

        var choice = GetValidInput(["1", "2", "3", "4", "5"]);
        switch (choice)
        {
            case "1":
                Console.WriteLine("Benchmarking Insert...");
                Console.WriteLine(GetNumberInput());
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
    }

    private static void BenchmarkNoSql()
    {
        Console.WriteLine();
        Console.WriteLine("Pick a benchmarking method:");
        Console.WriteLine("\t1. Insert");
        Console.WriteLine("\t2. Query");
        Console.WriteLine("\t3. Update");
        Console.WriteLine("\t4. Delete");
        Console.WriteLine("\t5. All");
        Console.WriteLine();

        Console.Write("Your choice: ");
        var choice = GetValidInput(["1", "2", "3", "4", "5"]);

        switch (choice)
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
    }
}