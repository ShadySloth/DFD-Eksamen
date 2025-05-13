using Database_Benchmarking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database_Benchmarking.Infrastructure.Context;

public class PostgresContext : DbContext
{
    public PostgresContext(DbContextOptions<PostgresContext> options) : base(options) { }

    public DbSet<Genre> Genres { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Article> Articles { get; set; }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Ignorer EntityId som en entitet
        modelBuilder.Ignore<EntityId>();

        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasConversion(
                    v => v.Value,
                    v => new EntityId(v));
        });

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.UserId);
            entity.Property(e => e.UserId)
                .HasConversion(
                    v => v.Value,
                    v => new EntityId(v));
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .HasConversion(
                    v => v.Value,
                    v => new EntityId(v));
        });
    }

}


