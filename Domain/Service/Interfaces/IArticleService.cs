using Database_Benchmarking.Domain.Entities;

namespace Database_Benchmarking.Domain.Service.Interfaces;

public interface IArticleService
{
    TimeSpan GetAllArticles(ICollection<Article> articles);
    TimeSpan CreateArticle(ICollection<Article> articles);
    TimeSpan UpdateArticle(ICollection<Article> articles);
    TimeSpan DeleteArticle(ICollection<Article> articles);
}