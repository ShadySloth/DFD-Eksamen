using System.Diagnostics;
using Database_Benchmarking.Domain.Entities;
using Database_Benchmarking.Infrastructure.Context;
using Database_Benchmarking.Infrastructure.Repository.Interfaces;

namespace Database_Benchmarking.Infrastructure.Repository.SQL;

public class SQLArticleRepository : IArticleRepository
{
    private readonly string _connectionString = "Host=localhost;Database=postgres;Username=postgres;Password=postgres";
    private readonly PostgresDbContext _context;
    public SQLArticleRepository(PostgresDbContext context)
    {
        _context = context;
    }
    
    public TimeSpan GetAll(ICollection<Article> articles)
    {
        CleanUp();
        _context.Articles.AddRange(articles);
        _context.SaveChanges();

        var query = $"SELECT * FROM \"Articles\"";

        using var connection = new Npgsql.NpgsqlConnection(_connectionString);
        connection.Open();

        using var command = new Npgsql.NpgsqlCommand(query, connection);
        var stopwatch = Stopwatch.StartNew();
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                // Process each article
                var article = new Article
                {
                    Id = new EntityId(reader.GetInt32(0).ToString()),
                    Title = reader.GetString(1),
                    BodyText = reader.GetString(2),
                    Updated = reader.GetDateTime(3),
                    Deleted = !reader.IsDBNull(4) ? reader.GetDateTime(4) : (DateTime?)null,
                    AuthorId = new EntityId(reader.GetInt32(5).ToString())
                };
                articles.Add(article);
            }
        }

        connection.Close();
        stopwatch.Stop();
        return stopwatch.Elapsed;
    }

    public TimeSpan GetById(ICollection<Article> articles, int indexToGet)
    {
        CleanUp();
        _context.Articles.AddRange(articles);
        _context.SaveChanges();

        var query =
            "SELECT * FROM (" +
                "SELECT *, row_number() OVER (ORDER BY \"Id\") AS rNum " +
                "FROM \"Articles\"" +
            ") AS subquery WHERE rnum = @RowNum";

        using var connection = new Npgsql.NpgsqlConnection(_connectionString);
        connection.Open();
        using var command = new Npgsql.NpgsqlCommand(query, connection);
        var stopwatch = Stopwatch.StartNew();
        command.Parameters.AddWithValue("@RowNum", indexToGet);
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var article = new Article
                {
                    Id = new EntityId(reader.GetInt32(0).ToString()),
                    Title = reader.GetString(1),
                    BodyText = reader.GetString(2),
                    Updated = reader.GetDateTime(3),
                    Deleted = !reader.IsDBNull(4) ? reader.GetDateTime(4) : (DateTime?)null,
                    AuthorId = new EntityId(reader.GetInt32(5).ToString())
                };
            }
        }

        connection.Close();
        stopwatch.Stop();
        return stopwatch.Elapsed;
    }

    public TimeSpan Create(ICollection<Article> articles)
    {
        CleanUp();
        var newAuthor = new Author { UserId = new EntityId("1"), AuthorName = "Test Author" };
        _context.Authors.Add(newAuthor);
        _context.SaveChanges();

        var query = "COPY \"Articles\" (\"Title\", \"BodyText\", \"Updated\", \"Deleted\", \"AuthorUserId\") " +
                    "FROM STDIN (FORMAT BINARY)";
        
        using var connection = new Npgsql.NpgsqlConnection(_connectionString);
        connection.Open();
        
        using var writer = connection.BeginBinaryImport(query);
        var stopwatch = Stopwatch.StartNew();
        foreach (var article in articles)
        {
            writer.StartRow();
            writer.Write(article.Title);
            writer.Write(article.BodyText);
            writer.Write(DateTime.UtcNow);
            writer.WriteNull();
            writer.Write(int.Parse(newAuthor.UserId.Value));
        }

        writer.Complete();
        stopwatch.Stop();
        return stopwatch.Elapsed;
    }

    public TimeSpan Update(ICollection<Article> articles)
    {
        CleanUp();
        _context.Articles.AddRange(articles);
        _context.SaveChanges();
        _context.ChangeTracker.Clear();

        var ids = articles.Select(a => int.Parse(a.Id.Value)).ToArray();
        var titles = articles.Select(a => a.Title ?? string.Empty).ToArray();
        var bodyTexts = articles.Select(a => a.BodyText = $"UPdated now: {a.Id.Value}"?? string.Empty).ToArray();
        var updatedTimestamps = Enumerable.Repeat(DateTime.UtcNow, articles.Count).ToArray();

        var query = @"
        UPDATE ""Articles"" a
        SET ""Title"" = data.title,
            ""BodyText"" = data.bodyText,
            ""Updated"" = data.updated
        FROM (
            SELECT UNNEST(@ids) AS id,
                   UNNEST(@titles) AS title,
                   UNNEST(@bodyTexts) AS bodyText,
                   UNNEST(@updateds) AS updated
        ) AS data
        WHERE a.""Id"" = data.id;
    ";
        
        using var connection = new Npgsql.NpgsqlConnection(_connectionString);
        connection.Open();
        using var command = new Npgsql.NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@ids", ids);
        command.Parameters.AddWithValue("@titles", titles);
        command.Parameters.AddWithValue("@bodyTexts", bodyTexts);
        command.Parameters.AddWithValue("@updateds", updatedTimestamps);

        var stopwatch = Stopwatch.StartNew();
        command.ExecuteNonQuery();
        stopwatch.Stop();

        return stopwatch.Elapsed;
    }

    public TimeSpan Delete(ICollection<Article> articles)
    {
        CleanUp();
        _context.Articles.AddRange(articles);
        _context.SaveChanges();

        var ids = articles.Select(a => int.Parse(a.Id.Value)).ToArray();
        var query = "DELETE FROM \"Articles\" WHERE \"Id\" = ANY(@Ids)";

        using var connection = new Npgsql.NpgsqlConnection(_connectionString);
        connection.Open();
        using var command = new Npgsql.NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@Ids", ids);

        var stopwatch = Stopwatch.StartNew();
        command.ExecuteNonQuery();
        stopwatch.Stop();

        connection.Close();
        return stopwatch.Elapsed;
    }

    private void CleanUp()
    {
        _context.Articles.RemoveRange(_context.Articles);
        _context.Authors.RemoveRange(_context.Authors);
        _context.SaveChanges();
    }
}