using Database_Benchmarking.Domain.Entities;

namespace Database_Benchmarking.Infrastructure.Repository.Interfaces;

public interface IArticleRepository
{
    IEnumerable<Article> GetAll();
    Article GetById(EntityId id);
    Article Create(Article article);
    Article Update(Article article);
    void Delete(EntityId id);
}