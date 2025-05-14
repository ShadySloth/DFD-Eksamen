namespace Database_Benchmarking.Consoles.SharedModels;

public class ResultSet
{
    TimeSpan EFCorePG { get; set; }
    TimeSpan NpgSql { get; set; }
    TimeSpan MongoDb { get; set; }
    string TestType { get; set; }
    int BatchSize { get; set; }
    bool IsAverage { get; set; }
}
