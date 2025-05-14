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
        
        var query = $"SELECT * FROM \"Authors\" LIMIT {authors.Count}";

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
                    UserId = new EntityId(reader.GetInt32(0).ToString()),
                    Name = reader.GetString(1),
                };
                fetchedAuthors.Add(author);
            }
        }
        connection.Close();
        stopwatch.Stop();
        CleanUp();
        return stopwatch.Elapsed;
    }

    public TimeSpan GetById(ICollection<Author> authors, int indexToGet)
    {
        _context.Authors.AddRange(authors);
        _context.SaveChanges();
        
        var query =
            "SELECT * FROM (" +
                "SELECT *, row_number() OVER (ORDER BY \"UserId\") AS rNum " +
                "FROM \"Authors\"" +
            ") AS subquery WHERE rNum = @indexToGet";
        
        using var connection = new Npgsql.NpgsqlConnection(_connectionString);
        connection.Open();
        using var command = new Npgsql.NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@indexToGet", indexToGet);
        var stopwatch = Stopwatch.StartNew();
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var author = new Author
                {
                    UserId = new EntityId(reader.GetInt32(0).ToString()),
                    Name = reader.GetString(1),
                };
            }
        }
        connection.Close();
        stopwatch.Stop();
        CleanUp();
        return stopwatch.Elapsed;
    }

    public TimeSpan Create(ICollection<Author> authors)
    {
        var query = "INSERT INTO \"Authors\" (Name) VALUES (@Name)";
        using var connection = new Npgsql.NpgsqlConnection(_connectionString);
        connection.Open();
        using var command = new Npgsql.NpgsqlCommand(query, connection);
        var stopwatch = Stopwatch.StartNew();
        foreach (var author in authors)
        {
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@Name", author.Name);
            command.ExecuteNonQuery();
        }
        connection.Close();
        stopwatch.Stop();
        CleanUp();
        return stopwatch.Elapsed;
    }

    public TimeSpan Update(ICollection<Author> authors)
    {
        _context.Authors.AddRange(authors);
        _context.SaveChanges();
        
        var query = "UPDATE \"Authors\" SET \"Name\" = @Name WHERE \"UserId\" = @UserId";
        using var connection = new Npgsql.NpgsqlConnection(_connectionString);
        connection.Open();
        using var command = new Npgsql.NpgsqlCommand(query, connection);
        var stopwatch = Stopwatch.StartNew();
        foreach (var author in authors)
        {
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@UserId", author.UserId.ToString());
            command.Parameters.AddWithValue("@Name", author.Name);
            command.ExecuteNonQuery();
        }
        stopwatch.Stop();
        CleanUp();
        return stopwatch.Elapsed;
    }

    public TimeSpan Delete(ICollection<Author> authors)
    {
        _context.Authors.AddRange(authors);
        _context.SaveChanges();
        
        var query = "DELETE FROM \"Authors\" WHERE \"UserId\" = @UserId";
        using var connection = new Npgsql.NpgsqlConnection(_connectionString);
        connection.Open();
        using var command = new Npgsql.NpgsqlCommand(query, connection);
        var stopwatch = Stopwatch.StartNew();
        foreach (var author in authors)
        {
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@UserId", author.UserId.ToString());
            command.ExecuteNonQuery();
        }
        connection.Close();
        stopwatch.Stop();
        CleanUp();
        return stopwatch.Elapsed;
    }

    private void CleanUp()
    {
        _context.Authors.RemoveRange(_context.Authors);
        _context.SaveChanges();
    }
}