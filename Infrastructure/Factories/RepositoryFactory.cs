﻿using Database_Benchmarking.Domain.Enums;
using Database_Benchmarking.Infrastructure.Context;
using Database_Benchmarking.Infrastructure.Repository.Interfaces;
using Database_Benchmarking.Infrastructure.Repository.MongoDb;
using Database_Benchmarking.Infrastructure.Repository.PostgresSQL;
using Database_Benchmarking.Infrastructure.Repository.SQL;

namespace Database_Benchmarking.Infrastructure.Factories;

public class RepositoryFactory
{
    private readonly MongoDbContext? _mongoDbContext;
    private readonly PostgresDbContext? _postgresDbContext;
    
    public RepositoryFactory(MongoDbContext mongoDbContext)
    {
        _mongoDbContext = mongoDbContext;
    }

    public RepositoryFactory(PostgresDbContext postgresDbContext)
    {
        _postgresDbContext = postgresDbContext;
    }

    public IArticleRepository ArticleRepository(DatabaseType databaseType)
    {
        return databaseType switch
        {
            DatabaseType.Relational => new PostgresArticleRepository(_postgresDbContext!),
            DatabaseType.RelationalRawdogging => new SQLArticleRepository(_postgresDbContext!),
            DatabaseType.NoSql => new MongoArticleRepository(_mongoDbContext!),
            _ => throw new ArgumentOutOfRangeException(nameof(databaseType), databaseType, null)
        };
    }

    public IAuthorRepository AuthorRepository(DatabaseType databaseType)
    {
        return databaseType switch
        {
            DatabaseType.Relational => new PostgresAuthorRepository(_postgresDbContext!),
            DatabaseType.RelationalRawdogging => new SQLAuthorRepository(_postgresDbContext!),
            DatabaseType.NoSql => new MongoAuthorRepository(_mongoDbContext!),
            _ => throw new ArgumentOutOfRangeException(nameof(databaseType), databaseType, null)
        };
    }

    public IGenreRepository GenreRepository(DatabaseType databaseType)
    {
        return databaseType switch
        {
            DatabaseType.Relational => new PostgresGenreRepository(_postgresDbContext!),
            DatabaseType.RelationalRawdogging => new SQLGenreRepository(_postgresDbContext!),
            DatabaseType.NoSql => new MongoGenreRepository(_mongoDbContext!),
            _ => throw new ArgumentOutOfRangeException(nameof(databaseType), databaseType, null)
        };
    }
}