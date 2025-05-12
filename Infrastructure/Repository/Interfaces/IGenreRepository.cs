using Database_Benchmarking.Domain.Entities;

namespace Database_Benchmarking.Infrastructure.Repository.Interfaces;

public interface IGenreRepository
{
    IEnumerable<Genre> GetAll();
    Genre GetById(EntityId id); 
    Genre Create(Genre genre);
    Genre Update(Genre genre);
    void Delete(EntityId id);
}