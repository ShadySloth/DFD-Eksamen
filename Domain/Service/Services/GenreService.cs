using Database_Benchmarking.Domain.Entities;
using Database_Benchmarking.Domain.Service.Interfaces;
using Database_Benchmarking.Infrastructure.Repository.Interfaces;

namespace Database_Benchmarking.Domain.Service.Services;

public class GenreService : IGenreService
{
    private readonly IGenreRepository _repository;

    public GenreService(IGenreRepository repository)
    {
        _repository = repository;
    }

    public TimeSpan GetAll()
    {
        TimeSpan timeSpan = _repository.GetAll();
        return timeSpan;
    }

    public TimeSpan Create(ICollection<Genre> genres)
    {
        TimeSpan timeSpan = _repository.Create(genres);
        return timeSpan;
    }

    public TimeSpan Update(ICollection<Genre> genres)
    {
        TimeSpan timeSpan = _repository.Update(genres);
        return timeSpan;
    }

    public TimeSpan Delete(ICollection<EntityId> ids)
    {
        TimeSpan timeSpan = _repository.Delete(ids);
        return timeSpan;
    }
}