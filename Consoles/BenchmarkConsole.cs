using Database_Benchmarking.Domain.Enums;
using Database_Benchmarking.Domain.Service.Services;

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

            var choice = GetValidInput(["1", "2"]);

            switch (choice)
            {
                case "1":
                    Benchmark(DatabaseType.Relational);
                    break;
                case "2":
                    Benchmark(DatabaseType.NoSql);
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

    private static void Benchmark(DatabaseType databaseType)
    {
        var service = new ServiceController(databaseType);
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
        var count = 0;
        switch (choice)
        {
            case "1":
                count = GetNumberInput();
                BenchmarkCreate(count, service);
                break;
            case "2":
                count = GetNumberInput();
                BenchmarkFetch(count, service);
                break;
            case "3":
                count = GetNumberInput();
                BenchmarkUpdate(count, service);
                break;
            case "4":
                count = GetNumberInput();
                BenchmarkDelete(count, service);
                break;
            case "5":
                count = GetNumberInput();
                Console.WriteLine("Benchmarking All...");
                var totalTime = BenchmarkCreate(count, service);
                totalTime += BenchmarkFetch(count, service);
                totalTime += BenchmarkUpdate(count, service);
                totalTime += BenchmarkDelete(count, service);
                Console.WriteLine($"Total time taken: {totalTime.TotalMilliseconds}ms");
                break;
        }
    }

    private static TimeSpan BenchmarkDelete(int count, ServiceController service)
    {
        Console.WriteLine($"Benchmarking {count} Deletes...");
        var time = service.DeleteArticles(count);
        Console.WriteLine($"Time taken: {time.TotalMilliseconds}ms");
        return time;
    }

    private static TimeSpan BenchmarkUpdate(int count, ServiceController service)
    {
        Console.WriteLine($"Benchmarking {count} Updates...");
        var time = service.UpdateArticles(count);
        Console.WriteLine($"Time taken: {time.TotalMilliseconds}ms");
        return time;
    }

    private static TimeSpan BenchmarkFetch(int count, ServiceController service)
    {
        Console.WriteLine($"Benchmarking {count} Fetches...");
        var time = service.GetAllArticles(count);
        Console.WriteLine($"Time taken: {time.TotalMilliseconds}ms");
        return time;
    }

    private static TimeSpan BenchmarkCreate(int count, ServiceController service)
    {
        Console.WriteLine($"Benchmarking {count} Inserts...");
        var time = service.CreateArticles(count);
        Console.WriteLine($"Time taken: {time.TotalMilliseconds}ms");
        return time;
    }
}