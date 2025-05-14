using Database_Benchmarking.Domain.Entities;
using Database_Benchmarking.Domain.Service.Interfaces;
using Database_Benchmarking.Infrastructure.Repository.Interfaces;

namespace Database_Benchmarking.Domain.Service.Services;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _authorRepository;

    public AuthorService(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public TimeSpan GetAll(ICollection<Author> authors)
    {
        var timeSpan = _authorRepository.GetAll(authors);
        return timeSpan;
    }

    public TimeSpan Create(ICollection<Author> authors)
    {
        var timeSpan = _authorRepository.Create(authors);
        return timeSpan;
    }

    public TimeSpan Update(ICollection<Author> authors)
    {
        var timeSpan = _authorRepository.Update(authors);
        return timeSpan;
    }

    public TimeSpan Delete(ICollection<Author> authors)
    { 
        var timeSpan = _authorRepository.Delete(authors);
        return timeSpan;
    }
}