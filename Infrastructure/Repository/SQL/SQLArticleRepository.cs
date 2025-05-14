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
        _context.Articles.AddRange(articles);
        _context.SaveChanges();
        
        var query = "SELECT * FROM \"Articles\"";

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
                    Id = new EntityId(reader.GetString(0)),
                    Title = reader.GetString(1),
                    BodyText = reader.GetString(2),
                    Updated = reader.GetDateTime(3),
                    Deleted = !reader.IsDBNull(4) ? reader.GetDateTime(4) : (DateTime?)null,
                    AuthorId = new EntityId(reader.GetString(5))
                };
                articles.Add(article);
            }
        }
        connection.Close();
        stopwatch.Stop();
        return stopwatch.Elapsed;
    }

    public TimeSpan Create(ICollection<Article> articles)
    {
        var query = "INSERT INTO \"Articles\" (Id, Title, BodyText, Updated, Deleted, AuthorId) VALUES (@Id, @Title, @BodyText, @Updated, @Deleted, @AuthorId)";
        
        using var connection = new Npgsql.NpgsqlConnection(_connectionString);
        connection.Open();
        using var command = new Npgsql.NpgsqlCommand(query, connection);
        var stopwatch = Stopwatch.StartNew();
        foreach (var article in articles)
        {
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@Id", Guid.NewGuid());
            command.Parameters.AddWithValue("@Title", article.Title);
            command.Parameters.AddWithValue("@BodyText", article.BodyText);
            command.Parameters.AddWithValue("@Updated", DateTime.UtcNow);
            command.Parameters.AddWithValue("@Deleted", DBNull.Value);
            command.Parameters.AddWithValue("@AuthorId", article.AuthorId.Value);

            command.ExecuteNonQuery();
        }
        stopwatch.Stop();
        return stopwatch.Elapsed;
    }

    public TimeSpan Update(ICollection<Article> articles)
    {
        throw new NotImplementedException();
    }

    public TimeSpan Delete(ICollection<Article> articles)
    {
        throw new NotImplementedException();
    }
}