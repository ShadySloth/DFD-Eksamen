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
    public IEnumerable<Author> GetAll()
    {
        var authors = _context.Authors.Find(_ => true).ToList();
        return authors.Select(EntityMapper.ToDomainEntity);
    }

    public Author GetById(EntityId id)
    {
        var author = _context.Authors.Find(authorDbModel => authorDbModel.UserId == new MongoDB.Bson.ObjectId(id.Value))
            .FirstOrDefault();
        return EntityMapper.ToDomainEntity(author);
    }

    public Author Create(Author author)
    {
        var authorDbModel = EntityMapper.ToDbModel(author);
        _context.Authors.InsertOne(authorDbModel);
        return EntityMapper.ToDomainEntity(authorDbModel);
    }

    public Author Update(Author author)
    {
        var replacementAuthor = EntityMapper.ToDbModel(author);
        _context.Authors.ReplaceOne(authorDbModel => authorDbModel.UserId == new MongoDB.Bson.ObjectId(author.UserId.Value), replacementAuthor);
        return EntityMapper.ToDomainEntity(replacementAuthor);
    }

    public void Delete(EntityId id)
    {
        _context.Authors.DeleteOne(author => author.UserId == new MongoDB.Bson.ObjectId(id.Value));
    }
}