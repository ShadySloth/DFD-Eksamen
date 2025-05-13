using Database_Benchmarking.Domain.Enums;
using Database_Benchmarking.Domain.Service.Interfaces;
using Database_Benchmarking.Infrastructure.Context;
using Database_Benchmarking.Infrastructure.Factories;

namespace Database_Benchmarking.Domain.Service.Services;

public class ServiceController : IServiceController
{
    private readonly IArticleService _articleService;
    private readonly IAuthorService _authorService;
    private readonly IGenreService _genreService;
    
    public ServiceController(DatabaseType databaseType)
    {
        RepositoryFactory repositoryFactory;
        
        switch (databaseType)
        {
            case DatabaseType.NoSql:
                var mongoDbContext = new MongoDbContext();
                repositoryFactory = new RepositoryFactory(mongoDbContext);
                break;
            case DatabaseType.Relational:
                var factory = new PostgresContextFactory();
                var postgresDbContext = factory.CreateDbContext([]);
                repositoryFactory = new RepositoryFactory(postgresDbContext);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(databaseType), databaseType, null);
        }
        _articleService = new ArticleService(repositoryFactory.ArticleRepository(databaseType));
        _authorService = new AuthorService(repositoryFactory.AuthorRepository(databaseType));
        _genreService = new GenreService(repositoryFactory.GenreRepository(databaseType));
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

    public TimeSpan CreateAuthors(int count)
    {
        throw new NotImplementedException();
    }

    public TimeSpan GetAllAuthors()
    {
        throw new NotImplementedException();
    }

    public TimeSpan DeleteAuthors(int count)
    {
        throw new NotImplementedException();
    }

    public TimeSpan UpdateAuthors(int count)
    {
        throw new NotImplementedException();
    }
}