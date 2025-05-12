namespace Database_Benchmarking.Infrastructure.Context;

public class PostgresContext : DbContext
{
    public PostgresContext(DbContextOptions<PostgresContext> options) : base(options) { }

    // Hvis denne bruges, behøver man ikke noget conn string i DP
    /**
     * example til program.cs:
     * builder.Services.AddDbContext<LogbookContext>(options =>
        {
            options.UseNpgsql(
                $"Host={hostname};Port={port};Database=postgres;Username={username};Password={password};Trust Server Certificate=true;");
        });
     */
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost:5432;Database=postgres;Username=postgres;Password=postgres");
    }
}