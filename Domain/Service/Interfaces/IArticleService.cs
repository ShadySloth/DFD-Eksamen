using Database_Benchmarking.Domain.Entities;

namespace Database_Benchmarking.Domain.Service.Interfaces;

public interface IArticleService
{
    TimeSpan GetAllArticles();
    TimeSpan GetArticleById(ICollection<EntityId> ids);
    TimeSpan CreateArticle(ICollection<Article> articles);
    TimeSpan UpdateArticle(ICollection<Article> articles);
    TimeSpan DeleteArticle(ICollection<EntityId> ids);
}