using System.Diagnostics;
using Database_Benchmarking.Domain.Entities;
using Database_Benchmarking.Infrastructure.Context;
using Database_Benchmarking.Infrastructure.DatabaseModels;
using Database_Benchmarking.Infrastructure.Mapper;
using Database_Benchmarking.Infrastructure.Repository.Interfaces;
using MongoDB.Driver;

namespace Database_Benchmarking.Infrastructure.Repository.MongoDb;

public class MongoAuthorRepository : IAuthorRepository
{
    private readonly MongoDbContext _context;
    public MongoAuthorRepository(MongoDbContext context)
    {
        _context = context;
    }
    public TimeSpan GetAll(ICollection<Author> authors)
    {
        Stopwatch stopwatch = new Stopwatch();
        
        var authorDbModel = authors.Select(EntityMapper.ToDbModel).ToList();
        _context.Authors.InsertMany(authorDbModel);
        
        stopwatch.Start();
        var foundAuthors = _context.Authors.Find(_ => true).ToList();
        stopwatch.Stop();
        TimeSpan elapsedTime = stopwatch.Elapsed;
        return elapsedTime;
    }

    public TimeSpan Create(ICollection<Author> authors)
    {
        Stopwatch stopwatch = new Stopwatch();
        var authorDbModel = authors.Select(EntityMapper.ToDbModel).ToList();
        stopwatch.Start();
        _context.Authors.InsertMany(authorDbModel);
        stopwatch.Stop();
        TimeSpan elapsedTime = stopwatch.Elapsed;
        return elapsedTime;
    }

    public TimeSpan Update(ICollection<Author> authors)
    {
        Stopwatch stopwatch = new Stopwatch();
        
        var replacementAuthors = authors.Select(EntityMapper.ToDbModel).ToList();
        _context.Authors.InsertMany(replacementAuthors);
        var filter = Builders<AuthorDbModel>.Filter.In(author => author.UserId, replacementAuthors.Select(article 
            => article.UserId));
        var update = Builders<AuthorDbModel>.Update.Set(author => author.Name, $"newName{DateTime.Now}");
        
        stopwatch.Start();
        _context.Authors.UpdateMany(filter, update);
        stopwatch.Stop();
        TimeSpan elapsedTime = stopwatch.Elapsed;
        return elapsedTime;
    }

    public TimeSpan Delete(ICollection<Author> authors)
    {
        Stopwatch stopwatch = new Stopwatch();
        
        var authorDbModel = authors.Select(EntityMapper.ToDbModel).ToList();
        _context.Authors.InsertMany(authorDbModel);
        var filter = Builders<AuthorDbModel>.Filter.In(author => author.UserId, authorDbModel.Select(article 
            => article.UserId));
        
        stopwatch.Start();
        _context.Authors.DeleteMany(filter);
        stopwatch.Stop();
        TimeSpan elapsedTime = stopwatch.Elapsed;
        return elapsedTime;
    }
}