
using Database_Benchmarking.Domain.Entities;
using Database_Benchmarking.Infrastructure.Context;
using Database_Benchmarking.Infrastructure.Repository.Interfaces;

namespace Database_Benchmarking.Infrastructure.Repository.PostgreSQL;

public class PostgresArticleRepository : IArticleRepository
{
    private readonly PostgresDbContext _context;
    public PostgresArticleRepository(PostgresDbContext context)
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

    public TimeSpan Create(ICollection<Article> articles)
    {
        throw new NotImplementedException();
    }

    public TimeSpan Update(ICollection<Article> articles)
    {
        throw new NotImplementedException();
    }

    public TimeSpan Delete(ICollection<EntityId> ids)
    {
        throw new NotImplementedException();
    }
}