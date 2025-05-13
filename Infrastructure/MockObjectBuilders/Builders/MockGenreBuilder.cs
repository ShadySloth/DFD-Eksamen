using Database_Benchmarking.Domain.Entities;

namespace Database_Benchmarking.Infrastructure.MockObjectBuilders.Builders
{
    public class MockGenreBuilder
    {
        // Metode til at bygge en liste af mock-genrer
        public static List<Genre> BuildMockGenres(int count)
        {
            // Liste af genres typer
            var genreTypes = new List<string>
            {
                "Action", "Comedy", "Drama", "Horror", "Fantasy", "Sci-Fi", "Adventure", "Romance", "Thriller", "Mystery"
            };

            // Liste til de genererede genre objekter
            var genres = new List<Genre>();

            // Skabe genrer med et unikt navn og ingen Id (Id vil blive genereret af databasen)
            for (int i = 0; i < count; i++)
            {
                var genre = new Genre
                {
                    Type = $"{genreTypes[i % genreTypes.Count]} {i + 1}" // Unikt genre navn
                };

                genres.Add(genre);
            }

            return genres;
        }
    }
}