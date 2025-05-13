using Database_Benchmarking.Infrastructure.Context;

namespace Database_Benchmarking.Domain.Service;

public class ServiceController(DatabaseType databaseType) : IServiceController
{
    private RepositoryFactory GetRepositoryFactory()
    {
        return databaseType switch
        {
            DatabaseType.Relational => new RepositoryFactory(new PostgresDbContext()),
            DatabaseType.NoSql => new RepositoryFactory(new MongoDbContext()),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    public TimeSpan CreateArticles(int count)
    {
        throw new NotImplementedException();
    }

    public TimeSpan GetAllArticles()
    {
        throw new NotImplementedException();
    }

    public TimeSpan DeleteArticles(int count)
    {
        throw new NotImplementedException();
    }

    public TimeSpan UpdateArticles(int count)
    {
        throw new NotImplementedException();
    }
}