using System.Collections.Generic;
using Database_Benchmarking.Domain.Entities;

namespace Database_Benchmarking.Infrastructure.MockObjectBuilders.Interfaces
{
    public interface IArticleMockBuilder
    {
        List<Article> BuildArticles(int count, string baseTitle, ICollection<Genre> genres, ICollection<Author> authors);  // Builder metode der tager en integer, baseTitle, genres og authors og returnerer en liste af mockede Article objekter
    }
}