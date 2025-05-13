using Database_Benchmarking.Domain.Entities;
using Database_Benchmarking.Infrastructure.Context;
using Database_Benchmarking.Infrastructure.Repository.Interfaces;

namespace Database_Benchmarking.Infrastructure.Repository.PostgreSQL;

public class PostgresAuthorRepository : IAuthorRepository
{
    private readonly PostgresDbContext _context;
    public PostgresAuthorRepository(PostgresDbContext context)
    {
        _context = context;
    }

    public TimeSpan GetAll()
    {
        throw new NotImplementedException();
    }

    public TimeSpan GetById(ICollection<EntityId> ids)
    {
        throw new NotImplementedException();
    }

    public TimeSpan Create(ICollection<Author> authors)
    {
        throw new NotImplementedException();
    }

    public TimeSpan Update(ICollection<Author> authors)
    {
        throw new NotImplementedException();
    }

    public TimeSpan Delete(ICollection<EntityId> ids)
    {
        throw new NotImplementedException();
    }
}