using Database_Benchmarking.Infrastructure.DatabaseModels;
using MongoDB.Driver;

namespace Database_Benchmarking.Infrastructure.Context;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext()
    {
        var client = new MongoClient("mongodb://localhost:27017");
        _database = client.GetDatabase("MongoDatabase");
    }
    
    public IMongoCollection<ArticleDbModel> Articles => _database.GetCollection<ArticleDbModel>("Articles");
    public IMongoCollection<AuthorDbModel> Authors => _database.GetCollection<AuthorDbModel>("Authors");
    public IMongoCollection<GenreDbModel> Genres => _database.GetCollection<GenreDbModel>("Genres");
}