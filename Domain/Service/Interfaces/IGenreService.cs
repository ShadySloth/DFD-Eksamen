using Database_Benchmarking.Domain.Entities;

namespace Database_Benchmarking.Domain.Service.Interfaces;

public interface IGenreService
{
    TimeSpan GetAll();
    TimeSpan GetById(ICollection<EntityId> ids);
    TimeSpan Create(ICollection<Genre> genres);
    TimeSpan Update(ICollection<Genre> genres);
    TimeSpan Delete(ICollection<EntityId> ids);
}