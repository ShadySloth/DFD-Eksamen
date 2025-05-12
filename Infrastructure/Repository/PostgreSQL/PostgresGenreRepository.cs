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
    
    public IEnumerable<Genre> GetAll()
    {
        throw new NotImplementedException();
    }

    public Genre GetById(EntityId id)
    {
        throw new NotImplementedException();
    }

    public Genre Create(Genre genre)
    {
        throw new NotImplementedException();
    }

    public Genre Update(Genre genre)
    {
        throw new NotImplementedException();
    }

    public void Delete(EntityId id)
    {
        throw new NotImplementedException();
    }
}