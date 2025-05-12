using Database_Benchmarking.Domain.Entities;

namespace Database_Benchmarking.Infrastructure.Repository.Interfaces;

public interface IGenreRepository
{
    TimeSpan GetAll();
    TimeSpan GetById(ICollection<EntityId> id); 
    TimeSpan Create(ICollection<Genre> genre);
    TimeSpan Update(ICollection<Genre> genre);
    TimeSpan Delete(ICollection<EntityId> id);
}