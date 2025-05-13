using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Database_Benchmarking.Infrastructure.Context;

namespace Database_Benchmarking.Infrastructure.Factories;

public class PostgresContextFactory : IDesignTimeDbContextFactory<PostgresDbContext>
{
    public PostgresDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PostgresDbContext>();

        optionsBuilder.UseNpgsql("Host=localhost;Database=postgres;Username=postgres;Password=postgres");

        return new PostgresDbContext(optionsBuilder.Options);
    }
}