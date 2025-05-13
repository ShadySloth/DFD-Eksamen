using System.Collections.Generic;
using Database_Benchmarking.Domain.Entities;
using Database_Benchmarking.Infrastructure.MockObjectBuilders.Interfaces;
using Database_Benchmarking.Infrastructure.MockObjectBuilders.Builders; // <- Husk at denne namespace skal matche dine konkrete klasser

namespace Database_Benchmarking.Domain.Service.Services
{
    public class MockDataService
    {
        private readonly IAuthorMockBuilder _authorMockBuilder;
        private readonly IGenreMockBuilder _genreMockBuilder;
        private readonly IArticleMockBuilder _articleMockBuilder;

        public MockDataService()
        {
            _authorMockBuilder = new MockAuthorBuilder();
            _genreMockBuilder = new MockGenreBuilder();
            _articleMockBuilder = new MockArticleBuilder();
        }

        public List<Author> GenerateMockAuthors(int authorCount)
        {
            string authorPrefix = "Author"; // Mock prefix for author names
            return _authorMockBuilder.BuildAuthors(authorCount, authorPrefix);
        }

        public List<Genre> GenerateMockGenres(int genreCount)
        {
            return _genreMockBuilder.BuildGenres(genreCount); // No extra parameters for genres
        }

        public List<Article> GenerateMockArticles(int articleCount)
        {
            int authorCount = 5; // Fixed number of authors for mock data
            int genreCount = 3; // Fixed number of genres for mock data
            string baseTitle = "Article"; // Mock base title for articles

            var genres = GenerateMockGenres(genreCount);
            var authors = GenerateMockAuthors(authorCount);

            return _articleMockBuilder.BuildArticles(articleCount, baseTitle, genres, authors);
        }
    }
}