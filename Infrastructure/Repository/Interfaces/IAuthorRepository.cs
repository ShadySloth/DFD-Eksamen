using Database_Benchmarking.Domain.Entities;

namespace Database_Benchmarking.Infrastructure.Repository.Interfaces;

public interface IAuthorRepository
{
    TimeSpan GetAll(ICollection<Author> authors);
    TimeSpan GetById(ICollection<Author> authors, int indexToGet);
    TimeSpan Create(ICollection<Author> authors);
    TimeSpan Update(ICollection<Author> authors);
    TimeSpan Delete(ICollection<Author> authors);
}