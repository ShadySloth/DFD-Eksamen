using Database_Benchmarking.Domain.Enums;
using Database_Benchmarking.Domain.Service.Interfaces;
using Database_Benchmarking.Infrastructure.Context;
using Database_Benchmarking.Infrastructure.Factories;

namespace Database_Benchmarking.Domain.Service.Services;

public class ServiceController : IServiceController
{
    private readonly IArticleService _articleService;
    private readonly IAuthorService _authorService;
    private readonly MockDataService _mockDataService;
    
    public ServiceController(DatabaseType databaseType)
    {
        RepositoryFactory repositoryFactory;
        
        switch (databaseType)
        {
            case DatabaseType.EfCore:
                var postgresContextFactory = new PostgresContextFactory();
                var postgresDbContext = postgresContextFactory.CreateDbContext([]);
                repositoryFactory = new RepositoryFactory(postgresDbContext);
                break;
            case DatabaseType.NoSql:
                var mongoDbContext = new MongoDbContext();
                repositoryFactory = new RepositoryFactory(mongoDbContext);
                break;
            case DatabaseType.Sql:
                var sqlContextFactory = new PostgresContextFactory();
                var sqlDbContext = sqlContextFactory.CreateDbContext([]);
                repositoryFactory = new RepositoryFactory(sqlDbContext);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(databaseType), databaseType, null);
        }
        _articleService = new ArticleService(repositoryFactory.ArticleRepository(databaseType));
        _authorService = new AuthorService(repositoryFactory.AuthorRepository(databaseType));
        
        _mockDataService = new MockDataService();
    }

    public TimeSpan CreateArticles(int count)
    {
        var articles = _mockDataService.GenerateMockArticles(count);
        return _articleService.CreateArticle(articles);
    }

    public TimeSpan GetAllArticles(int count)
    {
        var articles = _mockDataService.GenerateMockArticles(count);
        return _articleService.GetAllArticles(articles);
    }

    public TimeSpan DeleteArticles(int count)
    {
        var articles = _mockDataService.GenerateMockArticles(count);
        return _articleService.DeleteArticle(articles);
    }

    public TimeSpan UpdateArticles(int count)
    {
        var articles = _mockDataService.GenerateMockArticles(count);
        return _articleService.UpdateArticle(articles);
    }

    public TimeSpan CreateAuthors(int count)
    {
        var authors = _mockDataService.GenerateMockAuthors(count);
        return _authorService.Create(authors);
    }

    public TimeSpan GetAllAuthors(int count)
    {
        var authors = _mockDataService.GenerateMockAuthors(count);
        return _authorService.GetAll(authors);
    }

    public TimeSpan DeleteAuthors(int count)
    {
        var authors = _mockDataService.GenerateMockAuthors(count);
        return _authorService.Delete(authors);
    }

    public TimeSpan UpdateAuthors(int count)
    {
        var authors = _mockDataService.GenerateMockAuthors(count);
        return _authorService.Update(authors);
    }
}