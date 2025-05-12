using Database_Benchmarking.Domain.Entities;
using Database_Benchmarking.Infrastructure.Context;
using Database_Benchmarking.Infrastructure.Mapper;
using Database_Benchmarking.Infrastructure.Repository.Interfaces;
using MongoDB.Driver;

namespace Database_Benchmarking.Infrastructure.Repository.MongoDb;

public class MongoGenreRepository : IGenreRepository
{
    private readonly MongoDbContext _context;
    public MongoGenreRepository(MongoDbContext context)
    {
        _context = context;
    }
    public IEnumerable<Genre> GetAll()
    {
        var genres = _context.Genres.Find(_ => true).ToList();
        return genres.Select(EntityMapper.ToDomainEntity);
    }

    public Genre GetById(EntityId id)
    {
        var genre = _context.Genres.Find(genreDbModel => genreDbModel.Id == new MongoDB.Bson.ObjectId(id.Value))
            .FirstOrDefault();
        return EntityMapper.ToDomainEntity(genre);
    }

    public Genre Create(Genre genre)
    {
        var genreDbModel = EntityMapper.ToDbModel(genre);
        _context.Genres.InsertOne(genreDbModel);
        return EntityMapper.ToDomainEntity(genreDbModel);
    }

    public Genre Update(Genre genre)
    {
        var replacementGenre = EntityMapper.ToDbModel(genre);
        _context.Genres.ReplaceOne(genreDbModel => genreDbModel.Id == new MongoDB.Bson.ObjectId(genre.Id.Value), replacementGenre);
        return EntityMapper.ToDomainEntity(replacementGenre);
    }

    public void Delete(EntityId id)
    {
        _context.Genres.DeleteOne(genre => genre.Id == new MongoDB.Bson.ObjectId(id.Value));
    }
}