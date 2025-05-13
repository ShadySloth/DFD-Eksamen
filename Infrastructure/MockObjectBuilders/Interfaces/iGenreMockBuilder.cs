using Database_Benchmarking.Domain.Entities;

namespace Database_Benchmarking.Infrastructure.MockObjectBuilders.Interfaces
{
    public interface IGenreMockBuilder
    {
        List<Genre> BuildGenres(int count);  // Builder metode der tager en integer og returnerer en liste af mockede Genre objekter
    }
}
