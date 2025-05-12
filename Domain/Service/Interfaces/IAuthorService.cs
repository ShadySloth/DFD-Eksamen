using Database_Benchmarking.Domain.Entities;

namespace Database_Benchmarking.Domain.Service.Interfaces;

public interface IAuthorService
{
    TimeSpan GetAll();
    TimeSpan GetById(ICollection<EntityId> ids);
    TimeSpan Create(ICollection<Author> authors);
    TimeSpan Update(ICollection<Author> authors);
    TimeSpan Delete(ICollection<EntityId> ids);
}