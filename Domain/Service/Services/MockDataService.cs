using System.Collections.Generic;
using Database_Benchmarking.Domain.Entities;
using Database_Benchmarking.Infrastructure.MockObjectBuilders.Interfaces;
using Database_Benchmarking.Infrastructure.MockObjectBuilders.Builders; 

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
            string authorPrefix = "Author";
            return _authorMockBuilder.BuildAuthors(authorCount, authorPrefix);
        }

        public List<Genre> GenerateMockGenres(int genreCount)
        {
            return _genreMockBuilder.BuildGenres(genreCount);
        }

        public List<Article> GenerateMockArticles(int articleCount = 10, int authorCount = 5, int genreCount = 3)
        {
            string baseTitle = "Article";

            var genres = GenerateMockGenres(genreCount);
            var authors = GenerateMockAuthors(authorCount);

            return _articleMockBuilder.BuildArticles(articleCount, baseTitle, genres, authors);
        }
    }
}