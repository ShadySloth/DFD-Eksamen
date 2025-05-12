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

    public IEnumerable<Article> GetAll()
    {
        var articles = _context.Articles.Find(_ => true).ToList();
        return articles.Select(EntityMapper.ToDomainEntity);
    }

    public Article GetById(EntityId id)
    {
        var article = _context.Articles.Find(articleDbModel => articleDbModel.Id == new ObjectId(id.Value))
            .FirstOrDefault();
        return EntityMapper.ToDomainEntity(article);
    }

    public Article Create(Article article)
    {
        var articleDbModel = EntityMapper.ToDbModel(article);
        _context.Articles.InsertOne(articleDbModel);
        return EntityMapper.ToDomainEntity(articleDbModel);
    }

    public Article Update(Article article)
    {
        var replacementArticle = EntityMapper.ToDbModel(article);
        _context.Articles.ReplaceOne(articleDbModel => articleDbModel.Id == new ObjectId(article.Id.Value), replacementArticle);
        return EntityMapper.ToDomainEntity(replacementArticle);
    }

    public void Delete(EntityId id)
    {
        _context.Articles.DeleteOne(article => article.Id == new ObjectId(id.Value));
    }
}