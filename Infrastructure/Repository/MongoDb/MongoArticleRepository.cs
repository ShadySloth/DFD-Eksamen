using System.Diagnostics;
using Database_Benchmarking.Domain.Entities;
using Database_Benchmarking.Infrastructure.Context;
using Database_Benchmarking.Infrastructure.DatabaseModels;
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
        var articleDbModel = articles.Select(EntityMapper.ToDbModel).ToList();
        stopwatch.Start();
        _context.Articles.InsertMany(articleDbModel);
        stopwatch.Stop();
        TimeSpan elapsedTime = stopwatch.Elapsed;
        return elapsedTime;
    }

    public TimeSpan Update(ICollection<Article> articles)
    {
        Stopwatch stopwatch = new Stopwatch();
        
        var replacementArticles = articles.Select(EntityMapper.ToDbModel).ToList();
        _context.Articles.InsertMany(replacementArticles);
        var filter = Builders<ArticleDbModel>.Filter.In(article => article.Id, replacementArticles.Select(article => article.Id));
        var update = Builders<ArticleDbModel>.Update.Set(article => article.Updated, DateTime.Now);
        
        stopwatch.Start();
        _context.Articles.UpdateMany(filter, update);
        stopwatch.Stop();
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