using Database_Benchmarking.Domain.Entities;

namespace Database_Benchmarking.Infrastructure.Repository.Interfaces;

public interface IArticleRepository
{
    TimeSpan GetAll(ICollection<Article> articles);
    TimeSpan Create(ICollection<Article> articles);
    TimeSpan Update(ICollection<Article> articles);
    TimeSpan Delete(ICollection<Article> articles);
}