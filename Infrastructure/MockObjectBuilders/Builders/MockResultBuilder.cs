using Database_Benchmarking.Consoles.SharedModels;

namespace Database_Benchmarking.Infrastructure.MockObjectBuilders.Builders;

public class ResultSetMocker
{
    private static Random _random = new Random();

    public static List<ResultSet> MockResultSets(int[] batchSizes)
    {
        var resultSets = new List<ResultSet>();

        foreach (var batchSize in batchSizes)
        {
            // Generate 3 individual measurements
            for (int i = 0; i < 3; i++)
            {
                resultSets.Add(new ResultSet
                {
                    EFCorePG = GetRandomTimeSpan(),
                    NpgSql = GetRandomTimeSpan(),
                    MongoDb = GetRandomTimeSpan(),
                    TestType = GetRandomTestType(),
                    BatchSize = batchSize,
                    IsAverage = false // These are individual measurements
                });
            }

            // Generate 1 average measurement
            resultSets.Add(new ResultSet
            {
                EFCorePG = GetAverageTimeSpan(batchSize),
                NpgSql = GetAverageTimeSpan(batchSize),
                MongoDb = GetAverageTimeSpan(batchSize),
                TestType = "Average",
                BatchSize = batchSize,
                IsAverage = true // This is the average measurement
            });
        }

        return resultSets;
    }

    private static int GetRandomTimeSpan()
    {
        // Generate a random TimeSpan between 0 and 1 hour
        int seconds = _random.Next(0, 3600);
        return seconds;
    }

    private static int GetAverageTimeSpan(int batchSize)
    {
        // Generate an average TimeSpan based on the batch size
        int seconds = _random.Next(0, 3600); // Random time span, potentially adjusted by batch size
        return seconds;
    }

    private static string GetRandomTestType()
    {
        // List of potential test types
        var testTypes = new[] { "Insert", "Update", "Delete", "Select" };
        return testTypes[_random.Next(testTypes.Length)];
    }
}