using Database_Benchmarking.Domain.Entities;
using Database_Benchmarking.Infrastructure.MockObjectBuilders.Interfaces;

namespace Database_Benchmarking.Infrastructure.MockObjectBuilders.Builders
{
    public class MockGenreBuilder: IGenreMockBuilder
    {
        public List<Genre> BuildGenres(int count)
        {
            var genreTypes = new List<string>
            {
                "Action", "Comedy", "Drama", "Horror", "Fantasy", "Sci-Fi", "Adventure", "Romance", "Thriller", "Mystery"
            };

            var genres = new List<Genre>();

            for (int i = 0; i < count; i++)
            {
                var genre = new Genre
                {
                    Type = $"{genreTypes[i % genreTypes.Count]} {i + 1}"
                };

                genres.Add(genre);
            }

            return genres;
        }

    }
}