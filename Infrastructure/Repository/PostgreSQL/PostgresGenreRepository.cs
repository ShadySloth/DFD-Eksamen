using Database_Benchmarking.Domain.Entities;
using Database_Benchmarking.Infrastructure.Context;
using Database_Benchmarking.Infrastructure.Repository.Interfaces;

namespace Database_Benchmarking.Infrastructure.Repository.PostgreSQL;

public class PostgresGenreRepository : IGenreRepository
{
    private readonly PostgresDbContext _context;
    public PostgresGenreRepository(PostgresDbContext context)
    {
        _context = context;
    }

    public TimeSpan GetAll()
    {
        throw new NotImplementedException();
    }

    public TimeSpan GetById(ICollection<EntityId> id)
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