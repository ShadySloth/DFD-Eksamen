using Database_Benchmarking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Database_Benchmarking.Infrastructure.Context;

public class PostgresDbContext : DbContext
{
    public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options) { }

    public DbSet<Genre> Genres { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Article> Articles { get; set; }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Konvertering fra EntityId til int
        var entityIdToIntConverter = new ValueConverter<EntityId, int>(
            id => int.Parse(id.Value),         // EntityId → int
            val => new EntityId(val.ToString()) // int → EntityId
        );
    
        // Ignorer EntityId som en entitet, da den kun bruges som en wrapper
        modelBuilder.Ignore<EntityId>();

        // Definer konvertering for Article, Author og Genre
        modelBuilder.Entity<Article>(b =>
        {
            b.HasKey(e => e.Id);
            b.Property(e => e.Id).HasConversion(entityIdToIntConverter);
        });

        modelBuilder.Entity<Author>(b =>
        {
            b.HasKey(e => e.UserId);
            b.Property(e => e.UserId).HasConversion(entityIdToIntConverter);
        });

        modelBuilder.Entity<Genre>(b =>
        {
            b.HasKey(e => e.Id);
            b.Property(e => e.Id).HasConversion(entityIdToIntConverter);

            // Seed data – konverter EntityId til int værdi
            b.HasData(
                new Genre { Id = new EntityId("1"), Type = "Action" },
                new Genre { Id = new EntityId("2"), Type = "Comedy" },
                new Genre { Id = new EntityId("3"), Type = "Drama" },
                new Genre { Id = new EntityId("4"), Type = "Horror" },
                new Genre { Id = new EntityId("5"), Type = "Fantasy" }
            );
        });
    }



}


