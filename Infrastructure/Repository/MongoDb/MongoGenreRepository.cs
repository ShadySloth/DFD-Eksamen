using System.Diagnostics;
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
    public TimeSpan GetAll()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var genres = _context.Genres.Find(_ => true).ToList();
        stopwatch.Stop();
        var elapsedTime = stopwatch.Elapsed;
        return elapsedTime;
    }

    public TimeSpan Create(ICollection<Genre> genres)
    {
        var stopwatch = new Stopwatch();
        foreach (var genre in genres)
        {
            var genreDbModel = EntityMapper.ToDbModel(genre);
            stopwatch.Start();
            _context.Genres.InsertOne(genreDbModel);
            stopwatch.Stop();
        }
        var elapsedTime = stopwatch.Elapsed;
        return elapsedTime;
    }

    public TimeSpan Update(ICollection<Genre> genres)
    {
        var stopwatch = new Stopwatch();
        foreach (var genre in genres)
        {
            var replacementGenre = EntityMapper.ToDbModel(genre);
            stopwatch.Start();
            _context.Genres.ReplaceOne(genreDbModel => genreDbModel.Id == new MongoDB.Bson.ObjectId(genre.Id.Value), replacementGenre);
            stopwatch.Stop();
        }
        var elapsedTime = stopwatch.Elapsed;
        return elapsedTime;
    }

    public TimeSpan Delete(ICollection<EntityId> ids)
    {
        var stopwatch = new Stopwatch();
        foreach (var id in ids)
        {
            stopwatch.Start();
            _context.Genres.DeleteOne(genre => genre.Id == new MongoDB.Bson.ObjectId(id.Value));
            stopwatch.Stop();
        }
        var elapsedTime = stopwatch.Elapsed;
        return elapsedTime;
    }
}