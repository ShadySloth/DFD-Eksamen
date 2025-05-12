using System.Diagnostics;
using Database_Benchmarking.Domain.Entities;
using Database_Benchmarking.Infrastructure.Context;
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
    public TimeSpan GetAll()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        var authors = _context.Authors.Find(_ => true).ToList();
        stopwatch.Stop();
        TimeSpan elapsedTime = stopwatch.Elapsed;
        return elapsedTime;
    }

    public TimeSpan GetById(ICollection<EntityId> ids)
    {
        Stopwatch stopwatch = new Stopwatch();
        foreach (var id in ids)
        {
            stopwatch.Start();
            var author = _context.Authors.Find(authorDbModel => authorDbModel.UserId == new MongoDB.Bson.ObjectId(id.Value))
                .FirstOrDefault();
            stopwatch.Stop();
        }
        TimeSpan elapsedTime = stopwatch.Elapsed;
        return elapsedTime;
    }

    public TimeSpan Create(ICollection<Author> authors)
    {
        Stopwatch stopwatch = new Stopwatch();
        foreach (var author in authors)
        {
            var authorDbModel = EntityMapper.ToDbModel(author);
            stopwatch.Start();
            _context.Authors.InsertOne(authorDbModel);
            stopwatch.Stop();
        }
        TimeSpan elapsedTime = stopwatch.Elapsed;
        return elapsedTime;
    }

    public TimeSpan Update(ICollection<Author> authors)
    {
        Stopwatch stopwatch = new Stopwatch();
        foreach (var author in authors)
        {
            var replacementAuthor = EntityMapper.ToDbModel(author);
            stopwatch.Start();
            _context.Authors.ReplaceOne(authorDbModel => authorDbModel.UserId == new MongoDB.Bson.ObjectId(author.UserId.Value), replacementAuthor);
            stopwatch.Stop();
        }
        TimeSpan elapsedTime = stopwatch.Elapsed;
        return elapsedTime;
    }

    public TimeSpan Delete(ICollection<EntityId> ids)
    {
        Stopwatch stopwatch = new Stopwatch();
        foreach (var id in ids)
        {
            stopwatch.Start();
            _context.Authors.DeleteOne(author => author.UserId == new MongoDB.Bson.ObjectId(id.Value));
            stopwatch.Stop();
        }
        TimeSpan elapsedTime = stopwatch.Elapsed;
        return elapsedTime;
    }
}