using Database_Benchmarking.Domain.Entities;

namespace Database_Benchmarking.Infrastructure.Repository.Interfaces;

public interface IAuthorRepository
{
    IEnumerable<Author> GetAll();
    Author GetById(EntityId id);
    Author Create(Author author);
    Author Update(Author author);
    void Delete(EntityId id);
}