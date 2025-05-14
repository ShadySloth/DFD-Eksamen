using System.Diagnostics;
using Database_Benchmarking.Domain.Entities;
using Database_Benchmarking.Infrastructure.Context;
using Database_Benchmarking.Infrastructure.Repository.Interfaces;

namespace Database_Benchmarking.Infrastructure.Repository.SQL;

public class SQLAuthorRepository : IAuthorRepository
{
    private readonly string _connectionString = "Host=localhost;Database=postgres;Username=postgres;Password=postgres";
    private readonly PostgresDbContext _context;
    
    public SQLAuthorRepository(PostgresDbContext context)
    {
        _context = context;
    }
    public TimeSpan GetAll(ICollection<Author> authors)
    {
        _context.Authors.AddRange(authors);
        _context.SaveChanges();
        
        var query = "SELECT * FROM \"Authors\"";

        using var connection = new Npgsql.NpgsqlConnection(_connectionString);
        connection.Open();
        
        using var command = new Npgsql.NpgsqlCommand(query, connection);
        var fetchedAuthors = new List<Author>();
        var stopwatch = Stopwatch.StartNew();
        
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var author = new Author
                {
                    UserId = new EntityId(reader.GetString(0)),
                    Name = reader.GetString(1),
                };
                fetchedAuthors.Add(author);
            }
        }
        connection.Close();
        stopwatch.Stop();
        return stopwatch.Elapsed;
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