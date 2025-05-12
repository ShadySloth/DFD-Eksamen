using System.Diagnostics;
using Database_Benchmarking.Domain.Entities;
using Database_Benchmarking.Infrastructure.Context;
using Database_Benchmarking.Infrastructure.Mapper;
using Database_Benchmarking.Infrastructure.Repository.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Database_Benchmarking.Infrastructure.Repository.MongoDb;

public class MongoArticleRepository : IArticleRepository
{
    private readonly MongoDbContext _context;

    public MongoArticleRepository(MongoDbContext context)
    {
        _context = context;
    }

    public TimeSpan GetAll()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        var articles = _context.Articles.Find(_ => true).ToList();
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
            var article = _context.Articles.Find(articleDbModel => articleDbModel.Id == new ObjectId(id.Value))
                .FirstOrDefault();
            stopwatch.Stop();
        }
        TimeSpan elapsedTime = stopwatch.Elapsed;
        return elapsedTime;
    }

    public TimeSpan Create(ICollection<Article> articles)
    {
        Stopwatch stopwatch = new Stopwatch();
        foreach (var article in articles)
        {
            var articleDbModel = EntityMapper.ToDbModel(article);
            stopwatch.Start();
            _context.Articles.InsertOne(articleDbModel);
            stopwatch.Stop();
        }
        TimeSpan elapsedTime = stopwatch.Elapsed;
        return elapsedTime;
    }

    public TimeSpan Update(ICollection<Article> articles)
    {
        Stopwatch stopwatch = new Stopwatch();
        foreach (var article in articles)
        {
            var replacementArticle = EntityMapper.ToDbModel(article);
            stopwatch.Start();
            _context.Articles.ReplaceOne(articleDbModel => articleDbModel.Id == new ObjectId(article.Id.Value), replacementArticle);
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
            _context.Articles.DeleteOne(article => article.Id == new ObjectId(id.Value));
            stopwatch.Stop();
        }
        TimeSpan elapsedTime = stopwatch.Elapsed;
        return elapsedTime;
    }
}