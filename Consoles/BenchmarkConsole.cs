using Database_Benchmarking.Domain.Enums;
using Database_Benchmarking.Domain.Service.Services;

namespace Database_Benchmarking.Consoles;

public static class BenchmarkConsole
{
    public static void Run()
    {
        var endApp = false;

        Console.WriteLine("=======================================");
        Console.WriteLine("         Database Benchmarking         ");
        Console.WriteLine("=======================================\n");

        Console.WriteLine("Input 'q' at any time to quit the app.\n");

        while (!endApp)
        {
            Console.WriteLine("Select a database to benchmark:");
            Console.WriteLine("  1. Relational ORM");
            Console.WriteLine("  2. Relational SQL");
            Console.WriteLine("  3. NoSQL");
            Console.WriteLine();

            var choice = GetValidInput(["1", "2", "3"]);

            switch (choice)
            {
                case "1":
                    Benchmark(DatabaseType.Relational);
                    break;
                case "2":
                    Benchmark(DatabaseType.RelationalRawdogging);
                    break;
                case "3":
                    Benchmark(DatabaseType.NoSql);
                    break;
            }

            Console.WriteLine();
            Console.WriteLine("\n---------------------------------------------");
            Console.WriteLine("Press 'q' to quit, or any key to continue...");
            Console.WriteLine("---------------------------------------------\n");

            if (Console.ReadKey(true).KeyChar == 'q') endApp = true;

            Console.WriteLine("\n");
        }
    }

    private static string GetValidInput(string[] validInputs)
    {
        string input;
        
        Console.Write("Your choice: ");
        do
        {
            
            input = Console.ReadLine()!;
            if (input == "q")
            {
                Console.WriteLine("Quitting the application. Goodbye!");
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
        Console.WriteLine("\nEnter the number of records to benchmark.");
        while (true)
        {
            Console.Write("Your choice: ");

            var input = Console.ReadLine();
            
            if (input == "q")
            {
                Console.WriteLine("Quitting the application. Goodbye!");
                Environment.Exit(0);
            }
            
            if (int.TryParse(input, out var number))
            {
                return number;
            }

            Console.WriteLine("Invalid input. Please enter a number.");
        }
    }
    
    private static int GetIndexInput(int count)
    {
        Console.WriteLine("\nEnter the index of the record to benchmark.");
        while (true)
        {
            Console.Write($"Index to find (must be smaller than {count}: ");

            var input = Console.ReadLine();
            
            if (input == "q")
            {
                Console.WriteLine("Quitting the application. Goodbye!");
                Environment.Exit(0);
            }
            
            if (int.TryParse(input, out var number) && number < count)
            {
                return number;
            }

            Console.WriteLine("Invalid input. Please enter a number.");
        }
    }

    private static void Benchmark(DatabaseType databaseType)
    {
        var service = new ServiceController(databaseType);

        Console.WriteLine("\nPick a benchmarking method:");
        Console.WriteLine("  1. Insert");
        Console.WriteLine("  2. Query All");
        Console.WriteLine("  3. Query Single");
        Console.WriteLine("  4. Update");
        Console.WriteLine("  5. Delete");
        Console.WriteLine("  6. All");
        Console.WriteLine();

        var choice = GetValidInput(["1", "2", "3", "4", "5", "6"]);

        Console.WriteLine();
        
        var count = 0;
        var indexToGet = 0;
        switch (choice)
        {
            case "1":
                count = GetNumberInput();
                var time = BenchmarkCreate(count, service);
                Console.WriteLine($"\nTotal time taken: {GetRoundedMilliseconds(time)} ms.");
                break;
            case "2":
                count = GetNumberInput();
                time = BenchmarkFetchAll(count, service);
                Console.WriteLine($"\nTotal time taken: {GetRoundedMilliseconds(time)} ms.");
                break;
            case "3":
                count = GetNumberInput();
                indexToGet = GetIndexInput(count);
                time = BenchmarkFetchOne(count, indexToGet,service);
                Console.WriteLine($"\nTotal time taken: {GetRoundedMilliseconds(time)} ms.");
                break;
            case "4":
                count = GetNumberInput();
                time = BenchmarkUpdate(count, service);
                Console.WriteLine($"\nTotal time taken: {GetRoundedMilliseconds(time)} ms.");
                break;
            case "5":
                count = GetNumberInput();
                time = BenchmarkDelete(count, service);
                Console.WriteLine($"\nTotal time taken: {GetRoundedMilliseconds(time)} ms.");
                break;
            case "6":
                count = GetNumberInput();
                indexToGet = GetIndexInput(count);
                Console.WriteLine("\nBenchmarking All...");
                var totalTime = BenchmarkCreate(count, service);
                totalTime += BenchmarkFetchAll(count, service);
                totalTime += BenchmarkFetchOne(count, indexToGet, service);
                totalTime += BenchmarkUpdate(count, service);
                totalTime += BenchmarkDelete(count, service);
                Console.WriteLine($"\nTotal time taken: {GetRoundedMilliseconds(totalTime)} ms.");
                break;
        }
    }

    private static TimeSpan BenchmarkDelete(int count, ServiceController service)
    {
        Console.WriteLine($"\nBenchmarking {count} Deletes...");
        var time = service.DeleteArticles(count);
        var time2 = service.DeleteAuthors(count);
        Console.WriteLine($"  Time taken for articles: {GetRoundedMilliseconds(time)} ms.");
        Console.WriteLine($"  Time taken for authors: {GetRoundedMilliseconds(time2)} ms.");
        return time + time2;
    }

    private static TimeSpan BenchmarkUpdate(int count, ServiceController service)
    {
        Console.WriteLine($"\nBenchmarking {count} Updates...");
        var time = service.UpdateArticles(count);
        var time2 = service.UpdateAuthors(count);
        Console.WriteLine($"  Time taken for articles: {GetRoundedMilliseconds(time)} ms.");
        Console.WriteLine($"  Time taken for authors: {GetRoundedMilliseconds(time2)} ms.");
        return time + time2;
    }

    private static TimeSpan BenchmarkFetchAll(int count, ServiceController service)
    {
        Console.WriteLine($"\nBenchmarking {count} Fetches...");
        var time = service.GetAllArticles(count);
        var time2 = service.GetAllAuthors(count);
        Console.WriteLine($"  Time taken for articles: {GetRoundedMilliseconds(time)} ms.");
        Console.WriteLine($"  Time taken for authors: {GetRoundedMilliseconds(time2)} ms.");
        return time + time2;
    }
    
    private static TimeSpan BenchmarkFetchOne(int count, int indexToGet, ServiceController service)
    {
        Console.WriteLine($"\nBenchmarking {count} Fetching number {indexToGet}...");
        var time = service.GetByIdArticles(count, indexToGet);
        var time2 = service.GetByIdAuthors(count, indexToGet);
        Console.WriteLine($"  Time taken for articles: {GetRoundedMilliseconds(time)} ms.");
        Console.WriteLine($"  Time taken for authors: {GetRoundedMilliseconds(time2)} ms.");
        return time + time2;
    }

    private static TimeSpan BenchmarkCreate(int count, ServiceController service)
    {
        Console.WriteLine($"\nBenchmarking {count} Inserts...");
        var time = service.CreateArticles(count);
        var time2 = service.CreateAuthors(count);
        Console.WriteLine($"  Time taken for articles: {GetRoundedMilliseconds(time)} ms.");
        Console.WriteLine($"  Time taken for authors: {GetRoundedMilliseconds(time2)} ms.");
        return time + time2;
    }

    private static int GetRoundedMilliseconds(TimeSpan timeSpan)
    {
        return (int)Math.Round(timeSpan.TotalMilliseconds);
    }
}