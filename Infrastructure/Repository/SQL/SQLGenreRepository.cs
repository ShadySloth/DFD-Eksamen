using Database_Benchmarking.Domain.Entities;
using Database_Benchmarking.Infrastructure.Repository.Interfaces;

namespace Database_Benchmarking.Infrastructure.Repository.SQL;

public class SQLGenreRepository : IGenreRepository
{
    public TimeSpan GetAll()
    {
        throw new NotImplementedException();
    }

    public TimeSpan Create(ICollection<Genre> genre)
    {
        throw new NotImplementedException();
    }

    public TimeSpan Update(ICollection<Genre> genre)
    {
        throw new NotImplementedException();
    }

    public TimeSpan Delete(ICollection<EntityId> id)
    {
        throw new NotImplementedException();
    }
}