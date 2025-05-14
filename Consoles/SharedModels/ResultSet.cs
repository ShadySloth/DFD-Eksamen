namespace Database_Benchmarking.Consoles.SharedModels;

public class ResultSet
{
    public TimeSpan EFCorePG { get; set; }
    public TimeSpan NpgSql { get; set; }
    public TimeSpan MongoDb { get; set; }
    public string TestType { get; set; }
    public int BatchSize { get; set; }
    public bool IsAverage { get; set; }
}
