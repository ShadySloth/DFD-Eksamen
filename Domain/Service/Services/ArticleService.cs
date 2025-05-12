using Database_Benchmarking.Domain.Entities;
using Database_Benchmarking.Domain.Service.Interfaces;
using Database_Benchmarking.Infrastructure.Repository.Interfaces;

namespace Database_Benchmarking.Domain.Service.Services;

public class ArticleService : IArticleService
{
    private readonly IArticleRepository _repository;

    public ArticleService(IArticleRepository repository)
    {
        _repository = repository;
    }

    public TimeSpan GetAllArticles()
    {
        throw new NotImplementedException();
    }

    public TimeSpan GetArticleById(ICollection<EntityId> ids)
    {
        throw new NotImplementedException();
    }

    public TimeSpan CreateArticle(ICollection<Article> articles)
    {
        throw new NotImplementedException();
    }

    public TimeSpan UpdateArticle(ICollection<Article> articles)
    {
        throw new NotImplementedException();
    }

    public TimeSpan DeleteArticle(ICollection<EntityId> ids)
    {
        throw new NotImplementedException();
    }
}