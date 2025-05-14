using Database_Benchmarking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database_Benchmarking.Infrastructure.Context;

public class PostgresDbContext : DbContext
{
    public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options) { }

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
        
        // add hardcoded gernes
        modelBuilder.Entity<Genre>().HasData(
            new Genre { Id = new EntityId("11111111-1111-1111-1111-111111111111"), Type = "Action" },
            new Genre { Id = new EntityId("22222222-2222-2222-2222-222222222222"), Type = "Comedy" },
            new Genre { Id = new EntityId("33333333-3333-3333-3333-333333333333"), Type = "Drama" },
            new Genre { Id = new EntityId("44444444-4444-4444-4444-444444444444"), Type = "Horror" },
            new Genre { Id = new EntityId("55555555-5555-5555-5555-555555555555"), Type = "Fantasy" }
        );
    }

}


