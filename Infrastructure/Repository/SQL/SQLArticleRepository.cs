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

        var query = $"SELECT * FROM \"Articles\" LIMIT {articles.Count}";

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
                    AuthorId = new EntityId(reader.GetInt16(5).ToString())
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
                    AuthorId = new EntityId(reader.GetInt16(5).ToString())
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

        var query = "UPDATE \"Articles\" SET \"Title\" = @Title, \"BodyText\" = @BodyText, \"Updated\" = @Updated WHERE \"Id\" = @Id";

        using var connection = new Npgsql.NpgsqlConnection(_connectionString);
        connection.Open();
        using var command = new Npgsql.NpgsqlCommand(query, connection);
        var stopwatch = Stopwatch.StartNew();
        foreach (var article in articles)
        {
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@Id", int.Parse(article.Id.Value));
            command.Parameters.AddWithValue("@Title", article.Title);
            command.Parameters.AddWithValue("@BodyText", article.BodyText);
            command.Parameters.AddWithValue("@Updated", DateTime.UtcNow);

            command.ExecuteNonQuery();
        }

        connection.Close();
        stopwatch.Stop();
        return stopwatch.Elapsed;
    }

    public TimeSpan Delete(ICollection<Article> articles)
    {
        CleanUp();
        _context.Articles.AddRange(articles);
        _context.SaveChanges();

        var query = "DELETE FROM \"Articles\" WHERE \"Id\" = @Id";

        using var connection = new Npgsql.NpgsqlConnection(_connectionString);
        connection.Open();
        using var command = new Npgsql.NpgsqlCommand(query, connection);
        var stopwatch = Stopwatch.StartNew();
        foreach (var article in articles)
        {
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@Id", int.Parse(article.Id.Value));

            command.ExecuteNonQuery();
        }

        connection.Close();
        stopwatch.Stop();
        return stopwatch.Elapsed;
    }

    private void CleanUp()
    {
        _context.Articles.RemoveRange(_context.Articles);
        _context.Authors.RemoveRange(_context.Authors);
        _context.SaveChanges();
    }
}