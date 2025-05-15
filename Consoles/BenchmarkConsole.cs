using Database_Benchmarking.Consoles.SharedModels;
using Database_Benchmarking.Domain.Enums;
using Database_Benchmarking.Domain.Service.Services;
using Database_Benchmarking.Infrastructure.Generators;
using Microsoft.EntityFrameworkCore.Query.Internal;

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
            Console.WriteLine("  0. Run all benchmarks");
            Console.WriteLine("  1. Relational ORM");
            Console.WriteLine("  2. Relational SQL");
            Console.WriteLine("  3. NoSQL");
            Console.WriteLine();

            var choice = GetValidInput(["0","1", "2", "3"]);

            switch (choice)
            {
                case "0":
                    BenchmarkAll();
                    break;
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

    private static void BenchmarkAll()
    {
        var relationalOrmService = new ServiceController(DatabaseType.Relational);
        var relationalRawdoggingService = new ServiceController(DatabaseType.RelationalRawdogging);
        var noSqlService = new ServiceController(DatabaseType.NoSql);

        var count = GetNumberInput();
        var indexToGet = GetIndexInput(count);
        
        Console.WriteLine("\nBenchmarking All...");
        var resultSetCreateArticles = new List<ResultSet>();
        var resultSetCreateAuthors = new List<ResultSet>();
        var resultSetFetchAllArticles = new List<ResultSet>();
        var resultSetFetchAllAuthors = new List<ResultSet>();
        var resultSetFetchOneArticle = new List<ResultSet>();
        var resultSetFetchOneAuthor = new List<ResultSet>();
        var resultSetUpdateArticles = new List<ResultSet>();
        var resultSetUpdateAuthors = new List<ResultSet>();
        var resultSetDeleteArticles = new List<ResultSet>();
        var resultSetDeleteAuthors = new List<ResultSet>();
        
        for (int indexTest = 0; indexTest < 5; indexTest++)
        {
            switch (indexTest)
            {
                case 0:
                    Console.WriteLine($"\nBenchmarking {count} insert articles...");
                    resultSetCreateArticles = BenchmarkTestInsertArticles(count, resultSetCreateArticles);
                    break;
                case 1:
                    Console.WriteLine($"\nBenchmarking {count} query all articles...");
                    resultSetFetchAllArticles = BenchmarkTestFetchAllArticles(count, resultSetFetchAllArticles);
                    break;
                case 2:
                    Console.WriteLine($"\nBenchmarking {count} query one article...");
                    resultSetFetchOneArticle = BenchmarkTestFetchOneArticle(count, indexToGet, 
                        resultSetFetchOneArticle);
                    break;
                case 3:
                    Console.WriteLine($"\nBenchmarking {count} update articles...");
                    resultSetUpdateArticles = BenchmarkTestUpdateArticles(count, resultSetUpdateArticles);
                    break;
                case 4:
                    Console.WriteLine($"\nBenchmarking {count} delete articles...");
                    resultSetDeleteArticles = BenchmarkTestDeleteArticles(count, resultSetDeleteArticles);
                    break;
            }
        }

        Console.WriteLine("\n---------------------------------------------");
        
        for (int indexTest = 0; indexTest < 5; indexTest++)
        {
            switch (indexTest)
            {
                case 0:
                    Console.WriteLine($"\nBenchmarking {count} insert authors...");
                    resultSetCreateAuthors = BenchmarkTestInsertAuthors(count, resultSetCreateAuthors);
                    break;
                case 1:
                    Console.WriteLine($"\nBenchmarking {count} query all authors...");
                    resultSetFetchAllAuthors = BenchmarkTestFetchAllAuthors(count, resultSetFetchAllAuthors);
                    break;
                case 2:
                    Console.WriteLine($"\nBenchmarking {count} query one author...");
                    resultSetFetchOneAuthor = BenchmarkTestFetchOneAuthor(count, indexToGet, resultSetFetchOneAuthor);
                    break;
                case 3:
                    Console.WriteLine($"\nBenchmarking {count} update authors...");
                    resultSetUpdateAuthors = BenchmarkTestUpdateAuthors(count, resultSetUpdateAuthors);
                    break;
                case 4:
                    Console.WriteLine($"\nBenchmarking {count} delete authors...");
                    resultSetDeleteAuthors = BenchmarkTestDeleteAuthors(count, resultSetDeleteAuthors);
                    break;
            }
        }
        
                
        // Angiv stien til output PNG-fil
        string outputPath = "C:\\results\\diagramq.png";

        // Kald DiagramGenerator's GenerateDiagram metode
        DiagramGenerator.GenerateDiagram(resultSetCreateArticles, outputPath);

    }

    private static List<ResultSet> BenchmarkTestDeleteAuthors(int count, List<ResultSet> resultSetDeleteAuthors)
    {
        for (int index = 0; index < 3; index++)
        {
            var relationalOrmService = new ServiceController(DatabaseType.Relational);
            var relationalRawdoggingService = new ServiceController(DatabaseType.RelationalRawdogging);
            var noSqlService = new ServiceController(DatabaseType.NoSql);
            var resultSet = new ResultSet
            {
                EFCorePG = GetRoundedMilliseconds(BenchmarkDeleteAuthors(count, relationalOrmService)),
                NpgSql = GetRoundedMilliseconds(BenchmarkDeleteAuthors(count, relationalRawdoggingService)),
                MongoDb = GetRoundedMilliseconds(BenchmarkDeleteAuthors(count, noSqlService)),
                TestType = "DeleteAuthors",
                BatchSize = count,
                IsAverage = false
            };
            resultSetDeleteAuthors.Add(resultSet);
            if (index == 2)
            {
                var average = new ResultSet
                {
                    EFCorePG = Convert.ToInt32(resultSetDeleteAuthors.Average(x => x.EFCorePG)),
                    NpgSql = Convert.ToInt32(resultSetDeleteAuthors.Average(x => x.NpgSql)),
                    MongoDb = Convert.ToInt32(resultSetDeleteAuthors.Average(x => x.MongoDb)),
                    TestType = "DeleteAuthors",
                    BatchSize = count,
                    IsAverage = true
                };
                resultSetDeleteAuthors.Add(average);
            }
        }

        return resultSetDeleteAuthors;
    }

    private static List<ResultSet> BenchmarkTestUpdateAuthors(int count, List<ResultSet> resultSetUpdateAuthors)
    {
        for (int index = 0; index < 3; index++)
        {
            var relationalOrmService = new ServiceController(DatabaseType.Relational);
            var relationalRawdoggingService = new ServiceController(DatabaseType.RelationalRawdogging);
            var noSqlService = new ServiceController(DatabaseType.NoSql);
            var resultSet = new ResultSet
            {
                EFCorePG = GetRoundedMilliseconds(BenchmarkUpdateAuthors(count, relationalOrmService)),
                NpgSql = GetRoundedMilliseconds(BenchmarkUpdateAuthors(count, relationalRawdoggingService)),
                MongoDb = GetRoundedMilliseconds(BenchmarkUpdateAuthors(count, noSqlService)),
                TestType = "UpdateAuthors",
                BatchSize = count,
                IsAverage = false
            };
            resultSetUpdateAuthors.Add(resultSet);
            if (index == 2)
            {
                var average = new ResultSet
                {
                    EFCorePG = Convert.ToInt32(resultSetUpdateAuthors.Average(x => x.EFCorePG)),
                    NpgSql = Convert.ToInt32(resultSetUpdateAuthors.Average(x => x.NpgSql)),
                    MongoDb = Convert.ToInt32(resultSetUpdateAuthors.Average(x => x.MongoDb)),
                    TestType = "UpdateAuthors",
                    BatchSize = count,
                    IsAverage = true
                };
                resultSetUpdateAuthors.Add(average);
            }
        }

        return resultSetUpdateAuthors;
    }

    private static List<ResultSet> BenchmarkTestFetchOneAuthor(int count, int indexToGet,
        List<ResultSet> resultSetFetchOneAuthor)
    {
        for (int index = 0; index < 3; index++)
        {
            var relationalOrmService = new ServiceController(DatabaseType.Relational);
            var relationalRawdoggingService = new ServiceController(DatabaseType.RelationalRawdogging);
            var noSqlService = new ServiceController(DatabaseType.NoSql);
            var resultSet = new ResultSet
            {
                EFCorePG = GetRoundedMilliseconds(BenchmarkFetchOneAuthors(count, indexToGet, relationalOrmService)),
                NpgSql = GetRoundedMilliseconds(BenchmarkFetchOneAuthors(count, indexToGet, relationalRawdoggingService)),
                MongoDb = GetRoundedMilliseconds(BenchmarkFetchOneAuthors(count, indexToGet, noSqlService)),
                TestType = "FetchOneAuthors",
                BatchSize = count,
                IsAverage = false
            };
            resultSetFetchOneAuthor.Add(resultSet);
            if (index == 2)
            {
                var average = new ResultSet
                {
                    EFCorePG = Convert.ToInt32(resultSetFetchOneAuthor.Average(x => x.EFCorePG)),
                    NpgSql = Convert.ToInt32(resultSetFetchOneAuthor.Average(x => x.NpgSql)),
                    MongoDb = Convert.ToInt32(resultSetFetchOneAuthor.Average(x => x.MongoDb)),
                    TestType = "FetchOneAuthors",
                    BatchSize = count,
                    IsAverage = true
                };
                resultSetFetchOneAuthor.Add(average);
            }
        }

        return resultSetFetchOneAuthor;
    }

    private static List<ResultSet> BenchmarkTestFetchAllAuthors(int count, List<ResultSet> resultSetFetchAllAuthors)
    {
        for (int index = 0; index < 3; index++)
        {
            var relationalOrmService = new ServiceController(DatabaseType.Relational);
            var relationalRawdoggingService = new ServiceController(DatabaseType.RelationalRawdogging);
            var noSqlService = new ServiceController(DatabaseType.NoSql);
            var resultSet = new ResultSet
            {
                EFCorePG = GetRoundedMilliseconds(BenchmarkFetchAllAuthors(count, relationalOrmService)),
                NpgSql = GetRoundedMilliseconds(BenchmarkFetchAllAuthors(count, relationalRawdoggingService)),
                MongoDb = GetRoundedMilliseconds(BenchmarkFetchAllAuthors(count, noSqlService)),
                TestType = "FetchAllAuthors",
                BatchSize = count,
                IsAverage = false
            };
            resultSetFetchAllAuthors.Add(resultSet);
            if (index == 2)
            {
                var average = new ResultSet
                {
                    EFCorePG = Convert.ToInt32(resultSetFetchAllAuthors.Average(x => x.EFCorePG)),
                    NpgSql = Convert.ToInt32(resultSetFetchAllAuthors.Average(x => x.NpgSql)),
                    MongoDb = Convert.ToInt32(resultSetFetchAllAuthors.Average(x => x.MongoDb)),
                    TestType = "FetchAllAuthors",
                    BatchSize = count,
                    IsAverage = true
                };
                resultSetFetchAllAuthors.Add(average);
            }
        }

        return resultSetFetchAllAuthors;
    }

    private static List<ResultSet> BenchmarkTestInsertAuthors(int count, List<ResultSet> resultSetCreateAuthors)
    {
        for (int index = 0; index < 3; index++)
        {
            var relationalOrmService = new ServiceController(DatabaseType.Relational);
            var relationalRawdoggingService = new ServiceController(DatabaseType.RelationalRawdogging);
            var noSqlService = new ServiceController(DatabaseType.NoSql);
            var resultSet = new ResultSet
            {
                EFCorePG = GetRoundedMilliseconds(BenchmarkCreateAuthors(count, relationalOrmService)),
                NpgSql = GetRoundedMilliseconds(BenchmarkCreateAuthors(count, relationalRawdoggingService)),
                MongoDb = GetRoundedMilliseconds(BenchmarkCreateAuthors(count, noSqlService)),
                TestType = "CreateAuthors",
                BatchSize = count,
                IsAverage = false
            };
            resultSetCreateAuthors.Add(resultSet);
            if (index == 2)
            {
                var average = new ResultSet
                {
                    EFCorePG = Convert.ToInt32(resultSetCreateAuthors.Average(x => x.EFCorePG)),
                    NpgSql = Convert.ToInt32(resultSetCreateAuthors.Average(x => x.NpgSql)),
                    MongoDb = Convert.ToInt32(resultSetCreateAuthors.Average(x => x.MongoDb)),
                    TestType = "CreateAuthors",
                    BatchSize = count,
                    IsAverage = true
                };
                resultSetCreateAuthors.Add(average);
            }
        }

        return resultSetCreateAuthors;
    }

    private static List<ResultSet> BenchmarkTestDeleteArticles(int count, List<ResultSet> resultSetDeleteArticles)
    {
        for (int index = 0; index < 3; index++)
        {
            var relationalOrmService = new ServiceController(DatabaseType.Relational);
            var relationalRawdoggingService = new ServiceController(DatabaseType.RelationalRawdogging);
            var noSqlService = new ServiceController(DatabaseType.NoSql);
            var resultSet = new ResultSet
            {
                EFCorePG = GetRoundedMilliseconds(BenchmarkDeleteArticles(count, relationalOrmService)),
                NpgSql = GetRoundedMilliseconds(BenchmarkDeleteArticles(count, relationalRawdoggingService)),
                MongoDb = GetRoundedMilliseconds(BenchmarkDeleteArticles(count, noSqlService)),
                TestType = "DeleteArticles",
                BatchSize = count,
                IsAverage = false
            };
            resultSetDeleteArticles.Add(resultSet);
            if (index == 2)
            {
                var average = new ResultSet
                {
                    EFCorePG = Convert.ToInt32(resultSetDeleteArticles.Average(x => x.EFCorePG)),
                    NpgSql = Convert.ToInt32(resultSetDeleteArticles.Average(x => x.NpgSql)),
                    MongoDb = Convert.ToInt32(resultSetDeleteArticles.Average(x => x.MongoDb)),
                    TestType = "DeleteArticles",
                    BatchSize = count,
                    IsAverage = true
                };
                resultSetDeleteArticles.Add(average);
            }
        }

        return resultSetDeleteArticles;
    }

    private static List<ResultSet> BenchmarkTestUpdateArticles(int count, List<ResultSet> resultSetUpdateArticles)
    {
        for (int index = 0; index < 3; index++)
        {
            var relationalOrmService = new ServiceController(DatabaseType.Relational);
            var relationalRawdoggingService = new ServiceController(DatabaseType.RelationalRawdogging);
            var noSqlService = new ServiceController(DatabaseType.NoSql);
            var resultSet = new ResultSet
            {
                EFCorePG = GetRoundedMilliseconds(BenchmarkUpdateArticles(count, relationalOrmService)),
                NpgSql = GetRoundedMilliseconds(BenchmarkUpdateArticles(count, relationalRawdoggingService)),
                MongoDb = GetRoundedMilliseconds(BenchmarkUpdateArticles(count, noSqlService)),
                TestType = "UpdateArticles",
                BatchSize = count,
                IsAverage = false
            };
            resultSetUpdateArticles.Add(resultSet);
            if (index == 2)
            {
                var average = new ResultSet
                {
                    EFCorePG = Convert.ToInt32(resultSetUpdateArticles.Average(x => x.EFCorePG)),
                    NpgSql = Convert.ToInt32(resultSetUpdateArticles.Average(x => x.NpgSql)),
                    MongoDb = Convert.ToInt32(resultSetUpdateArticles.Average(x => x.MongoDb)),
                    TestType = "UpdateArticles",
                    BatchSize = count,
                    IsAverage = true
                };
                resultSetUpdateArticles.Add(average);
            }
        }

        return resultSetUpdateArticles;
    }

    private static List<ResultSet> BenchmarkTestFetchOneArticle(int count, int indexToGet, 
        List<ResultSet> resultSetFetchOneArticles)
    {
        for (int index = 0; index < 3; index++)
        {
            var relationalOrmService = new ServiceController(DatabaseType.Relational);
            var relationalRawdoggingService = new ServiceController(DatabaseType.RelationalRawdogging);
            var noSqlService = new ServiceController(DatabaseType.NoSql);
            var resultSet = new ResultSet
            {
                EFCorePG = GetRoundedMilliseconds(BenchmarkFetchOneArticle(count, indexToGet, relationalOrmService)),
                NpgSql = GetRoundedMilliseconds(BenchmarkFetchOneArticle(count, indexToGet, relationalRawdoggingService)),
                MongoDb = GetRoundedMilliseconds(BenchmarkFetchOneArticle(count, indexToGet, noSqlService)),
                TestType = "FetchOneArticles",
                BatchSize = count,
                IsAverage = false
            };
            resultSetFetchOneArticles.Add(resultSet);
            if (index == 2)
            {
                var average = new ResultSet
                {
                    EFCorePG = Convert.ToInt32(resultSetFetchOneArticles.Average(x => x.EFCorePG)),
                    NpgSql = Convert.ToInt32(resultSetFetchOneArticles.Average(x => x.NpgSql)),
                    MongoDb = Convert.ToInt32(resultSetFetchOneArticles.Average(x => x.MongoDb)),
                    TestType = "FetchOneArticles",
                    BatchSize = count,
                    IsAverage = true
                };
                resultSetFetchOneArticles.Add(average);
            }
        }

        return resultSetFetchOneArticles;
    }

    private static List<ResultSet> BenchmarkTestFetchAllArticles(int count,  List<ResultSet> resultSetFetchAllArticles)
    {
        for (int index = 0; index < 3; index++)
        {
            var relationalOrmService = new ServiceController(DatabaseType.Relational);
            var relationalRawdoggingService = new ServiceController(DatabaseType.RelationalRawdogging);
            var noSqlService = new ServiceController(DatabaseType.NoSql);
            var resultSet = new ResultSet
            {
                EFCorePG = GetRoundedMilliseconds(BenchmarkFetchAllArticles(count, relationalOrmService)),
                NpgSql = GetRoundedMilliseconds(BenchmarkFetchAllArticles(count, relationalRawdoggingService)),
                MongoDb = GetRoundedMilliseconds(BenchmarkFetchAllArticles(count, noSqlService)),
                TestType = "FetchAllArticles",
                BatchSize = count,
                IsAverage = false
            };
            resultSetFetchAllArticles.Add(resultSet);
            if (index == 2)
            {
                var average = new ResultSet
                {
                    EFCorePG = Convert.ToInt32(resultSetFetchAllArticles.Average(x => x.EFCorePG)),
                    NpgSql = Convert.ToInt32(resultSetFetchAllArticles.Average(x => x.NpgSql)),
                    MongoDb = Convert.ToInt32(resultSetFetchAllArticles.Average(x => x.MongoDb)),
                    TestType = "FetchAllArticles",
                    BatchSize = count,
                    IsAverage = true
                };
                resultSetFetchAllArticles.Add(average);
            }
        }

        return resultSetFetchAllArticles;
    }

    private static List<ResultSet> BenchmarkTestInsertArticles(int count, List<ResultSet> resultSetCreateArticles)
    {
        for (int index = 0; index < 3; index++)
        {
            var relationalOrmService = new ServiceController(DatabaseType.Relational);
            var relationalRawdoggingService = new ServiceController(DatabaseType.RelationalRawdogging);
            var noSqlService = new ServiceController(DatabaseType.NoSql);
            var resultSet = new ResultSet
            {
                EFCorePG = GetRoundedMilliseconds(BenchmarkCreateArticles(count, relationalOrmService)),
                NpgSql = GetRoundedMilliseconds(BenchmarkCreateArticles(count, relationalRawdoggingService)),
                MongoDb = GetRoundedMilliseconds(BenchmarkCreateArticles(count, noSqlService)),
                TestType = "CreateArticles",
                BatchSize = count,
                IsAverage = false
            };
            resultSetCreateArticles.Add(resultSet);
            if (index == 2)
            {
                var average = new ResultSet
                {
                    EFCorePG = Convert.ToInt32(resultSetCreateArticles.Average(x => x.EFCorePG)),
                    NpgSql = Convert.ToInt32(resultSetCreateArticles.Average(x => x.NpgSql)),
                    MongoDb = Convert.ToInt32(resultSetCreateArticles.Average(x => x.MongoDb)),
                    TestType = "CreateArticles",
                    BatchSize = count,
                    IsAverage = true
                };
                resultSetCreateArticles.Add(average);
            }
        }

        return resultSetCreateArticles;
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
        Console.WriteLine($"\nBenchmarking {count} Inserts..");
        var time = service.CreateArticles(count);
        var time2 = service.CreateAuthors(count);
        Console.WriteLine($"  Time taken for articles: {GetRoundedMilliseconds(time)} ms.");
        Console.WriteLine($"  Time taken for authors: {GetRoundedMilliseconds(time2)} ms.");
        return time + time2;
    }
    
    private static TimeSpan BenchmarkCreateArticles(int count, ServiceController service)
    {
        Console.WriteLine($"\nBenchmarking {count} Inserts using {service.ToString()}...");
        var time = service.CreateArticles(count);
        Console.WriteLine($"  Time taken for articles: {GetRoundedMilliseconds(time)} ms.");
        return time;
    }
    private static TimeSpan BenchmarkCreateAuthors(int count, ServiceController service)
    {
        Console.WriteLine($"\nBenchmarking {count} Inserts using {service.ToString()}...");
        var time = service.CreateAuthors(count);
        Console.WriteLine($"  Time taken for authors: {GetRoundedMilliseconds(time)} ms.");
        return time;
    }
    
    private static TimeSpan BenchmarkFetchAllArticles(int count, ServiceController service)
    {
        Console.WriteLine($"\nBenchmarking {count} Fetches using {service.ToString()}...");
        var time = service.GetAllArticles(count);
        Console.WriteLine($"  Time taken for articles: {GetRoundedMilliseconds(time)} ms.");
        return time;
    }
    
    private static TimeSpan BenchmarkFetchAllAuthors(int count, ServiceController service)
    {
        Console.WriteLine($"\nBenchmarking {count} Fetches using {service.ToString()}...");
        var time = service.GetAllAuthors(count);
        Console.WriteLine($"  Time taken for authors: {GetRoundedMilliseconds(time)} ms.");
        return time;
    }
    
    private static TimeSpan BenchmarkFetchOneArticle(int count, int indexToGet, ServiceController service)
    {
        Console.WriteLine($"\nBenchmarking {count} Fetching number {indexToGet} using {service.ToString()}...");
        var time = service.GetByIdArticles(count, indexToGet);
        Console.WriteLine($"  Time taken for articles: {GetRoundedMilliseconds(time)} ms.");
        return time;
    }
    
    private static TimeSpan BenchmarkFetchOneAuthors(int count, int indexToGet, ServiceController service)
    {
        Console.WriteLine($"\nBenchmarking {count} Fetching number {indexToGet} using {service.ToString()}...");
        var time = service.GetByIdAuthors(count, indexToGet);
        Console.WriteLine($"  Time taken for authors: {GetRoundedMilliseconds(time)} ms.");
        return time;
    }
    
    private static TimeSpan BenchmarkUpdateArticles(int count, ServiceController service)
    {
        Console.WriteLine($"\nBenchmarking {count} Updates using {service.ToString()}...");
        var time = service.UpdateArticles(count);
        Console.WriteLine($"  Time taken for articles: {GetRoundedMilliseconds(time)} ms.");
        return time;
    }
    
    private static TimeSpan BenchmarkUpdateAuthors(int count, ServiceController service)
    {
        Console.WriteLine($"\nBenchmarking {count} Updates using {service.ToString()}...");
        var time = service.UpdateAuthors(count);
        Console.WriteLine($"  Time taken for authors: {GetRoundedMilliseconds(time)} ms.");
        return time;
    }
    
    private static TimeSpan BenchmarkDeleteArticles(int count, ServiceController service)
    {
        Console.WriteLine($"\nBenchmarking {count} Deletes using {service.ToString()}...");
        var time = service.DeleteArticles(count);
        Console.WriteLine($"  Time taken for articles: {GetRoundedMilliseconds(time)} ms.");
        return time;
    }
    
    private static TimeSpan BenchmarkDeleteAuthors(int count, ServiceController service)
    {
        Console.WriteLine($"\nBenchmarking {count} Deletes using {service.ToString()}...");
        var time = service.DeleteAuthors(count);
        Console.WriteLine($"  Time taken for authors: {GetRoundedMilliseconds(time)} ms.");
        return time;
    }

    private static int GetRoundedMilliseconds(TimeSpan timeSpan)
    {
        return (int)Math.Round(timeSpan.TotalMilliseconds);
    }
}