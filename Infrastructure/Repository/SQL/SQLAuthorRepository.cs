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
        CleanUp();
        _context.Authors.AddRange(authors);
        _context.SaveChanges();

        var query = $"SELECT * FROM \"Authors\"";

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
                    AuthorName = reader.GetString(1),
                };
                fetchedAuthors.Add(author);
            }
        }

        connection.Close();
        stopwatch.Stop();
        return stopwatch.Elapsed;
    }

    public TimeSpan GetById(ICollection<Author> authors, int indexToGet)
    {
        CleanUp();
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
                    AuthorName = reader.GetString(1),
                };
            }
        }

        connection.Close();
        stopwatch.Stop();
        return stopwatch.Elapsed;
    }

    public TimeSpan Create(ICollection<Author> authors)
    {
        var query = "COPY \"Authors\" (\"AuthorName\") FROM STDIN (FORMAT BINARY)";
        using var connection = new Npgsql.NpgsqlConnection(_connectionString);
        connection.Open();
        
        using var writer = connection.BeginBinaryImport(query);
        
        var stopwatch = Stopwatch.StartNew();
        foreach (var author in authors)
        {
            writer.StartRow();
            writer.Write(author.AuthorName);
        }
        
        writer.Complete();
        connection.Close();
        stopwatch.Stop();
        return stopwatch.Elapsed;
    }

    public TimeSpan Update(ICollection<Author> authors)
    {
       CleanUp();

       _context.Authors.AddRange(authors);
       _context.SaveChanges();

       var userIds = authors.Select(a => int.Parse(a.UserId.Value)).ToArray();
       var names = authors.Select(a => a.AuthorName).ToArray();

       var query = @"
           UPDATE ""Authors"" a
           SET ""AuthorName"" = data.name
           FROM (
               SELECT UNNEST(@userIds) AS userId,
                      UNNEST(@names) AS name
           ) AS data
           WHERE a.""UserId"" = data.userId;
       ";

       using var connection = new Npgsql.NpgsqlConnection(_connectionString);
       connection.Open();
       using var command = new Npgsql.NpgsqlCommand(query, connection);
       command.Parameters.AddWithValue("@userIds", userIds);
       command.Parameters.AddWithValue("@names", names);

       var stopwatch = Stopwatch.StartNew();
       command.ExecuteNonQuery();
       stopwatch.Stop();

       return stopwatch.Elapsed;
    }

    public TimeSpan Delete(ICollection<Author> authors)
    {
        CleanUp();
        _context.Authors.AddRange(authors);
        _context.SaveChanges();
        
        var UserIds = authors.Select(a => int.Parse(a.UserId.Value)).ToArray();
        var query = "DELETE FROM \"Authors\" WHERE \"UserId\" = ANY(@UserIds)";
        
        using var connection = new Npgsql.NpgsqlConnection(_connectionString);
        connection.Open();
        using var command = new Npgsql.NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@UserIds", UserIds);
        
        var stopwatch = Stopwatch.StartNew();
        command.ExecuteNonQuery();
        stopwatch.Stop();
        
        connection.Close();
        return stopwatch.Elapsed;
    }

    private void CleanUp()
    {
        _context.Authors.RemoveRange(_context.Authors);
        _context.SaveChanges();
    }
}