namespace Database_Benchmarking.Consoles.SharedModels;

public class ResultSet
{
    public int EFCorePG { get; set; }
    public int NpgSql { get; set; }
    public int MongoDb { get; set; }
    public string TestType { get; set; }
    public int BatchSize { get; set; }
    public bool IsAverage { get; set; }
}
