using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Database_Benchmarking.Infrastructure.Context;

namespace Database_Benchmarking.Infrastructure.Factories;

public class PostgresContextFactory : IDesignTimeDbContextFactory<PostgresContext>
{
    public PostgresContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PostgresContext>();

        optionsBuilder.UseNpgsql("Host=localhost;Database=postgres;Username=postgres;Password=postgres");

        return new PostgresContext(optionsBuilder.Options);
    }
}