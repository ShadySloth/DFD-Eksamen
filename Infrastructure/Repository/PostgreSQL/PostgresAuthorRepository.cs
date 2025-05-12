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
    
    public IEnumerable<Author> GetAll()
    {
        throw new NotImplementedException();
    }

    public Author GetById(EntityId id)
    {
        throw new NotImplementedException();
    }

    public Author Create(Author author)
    {
        throw new NotImplementedException();
    }

    public Author Update(Author author)
    {
        throw new NotImplementedException();
    }

    public void Delete(EntityId id)
    {
        throw new NotImplementedException();
    }
}