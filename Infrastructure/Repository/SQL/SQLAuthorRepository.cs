using Database_Benchmarking.Domain.Entities;
using Database_Benchmarking.Infrastructure.Context;
using Database_Benchmarking.Infrastructure.Repository.Interfaces;

namespace Database_Benchmarking.Infrastructure.Repository.SQL;

public class SQLAuthorRepository : IAuthorRepository
{
    private readonly PostgresDbContext _context;
    
    public SQLAuthorRepository(PostgresDbContext context)
    {
        _context = context;
    }
    public TimeSpan GetAll(ICollection<Author> authors)
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

    public TimeSpan Delete(ICollection<Author> authors)
    {
        throw new NotImplementedException();
    }
}