using Database_Benchmarking.Infrastructure.Context;

namespace Database_Benchmarking.Domain.Service;

public class ServiceController(DatabaseType databaseType) : IServiceController
{
    private RepositoryFactory GetRepositoryFactory()
    {
        switch (databaseType)
        {
            case DatabaseType.Relational:
                return new RepositoryFactory(new PostgresDbContext());
            case DatabaseType.NoSql:
                return new RepositoryFactory(new MongoDbContext());
            default:
                throw new ArgumentOutOfRangeException();
        }
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