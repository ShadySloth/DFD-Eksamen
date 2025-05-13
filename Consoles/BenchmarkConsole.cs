using Database_Benchmarking.Domain;
using Database_Benchmarking.Domain.Enums;
using Database_Benchmarking.Domain.Service;
using Database_Benchmarking.Domain.Service.Services;
using Database_Benchmarking.Infrastructure.Repository.Interfaces;

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
        TimeSpan time;
        var count = 0;
        switch (choice)
        {
            case "1":
                count = GetNumberInput();
                Console.WriteLine($"Benchmarking {count} Inserts...");
                time = service.CreateArticles(count);
                Console.WriteLine($"Time taken: {time.TotalMilliseconds}ms");
                break;
            case "2":
                count = GetNumberInput();
                Console.WriteLine($"Benchmarking {count} Fetches...");
                time = service.GetAllArticles(count);
                Console.WriteLine($"Time taken: {time.TotalMilliseconds}ms");
                break;
            case "3":
                count = GetNumberInput();
                Console.WriteLine($"Benchmarking {count} Updates...");
                time = service.UpdateArticles(count);
                Console.WriteLine($"Time taken: {time.TotalMilliseconds}ms");
                break;
            case "4":
                count = GetNumberInput();
                Console.WriteLine($"Benchmarking {count} Deletes...");
                time = service.DeleteArticles(count);
                Console.WriteLine($"Time taken: {time.TotalMilliseconds}ms");
                break;
            case "5":
                count = GetNumberInput();
                Console.WriteLine("Benchmarking All...");
                time = service.CreateArticles(count);
                time += service.GetAllArticles(count);
                time += service.UpdateArticles(count);
                time += service.DeleteArticles(count);
                Console.WriteLine($"Total time taken: {time.TotalMilliseconds}ms");
                break;
        }
    }
}