using Database_Benchmarking.Domain.Entities;

namespace Database_Benchmarking.Infrastructure.MockObjectBuilders.Interfaces
{
    public interface IAuthorMockBuilder
    {
        List<Author> BuildAuthors(int count, string namePrefix);  // Builder metode der tager en integer og en string (navn prefix) og returnerer en liste af mockede Author objekter
    }
}