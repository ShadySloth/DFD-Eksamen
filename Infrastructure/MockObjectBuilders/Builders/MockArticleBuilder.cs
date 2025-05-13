using System;
using System.Collections.Generic;
using Database_Benchmarking.Domain.Entities;
using Database_Benchmarking.Infrastructure.MockObjectBuilders.Interfaces;

namespace Database_Benchmarking.Infrastructure.MockObjectBuilders.Builders
{
    public class MockArticleBuilder : IArticleMockBuilder
    {
        // Metode til at bygge en liste af mock-artikler med en grundlæggende titel og et unikt tal
        public List<Article> BuildArticles(int count, string baseTitle, ICollection<Genre> genres, ICollection<Author> authors)
        {
            var articles = new List<Article>();

            // Konverter authors og genres til lister for at kunne indeksere
            var authorList = new List<Author>(authors);
            var genreList = new List<Genre>(genres);

            for (int i = 0; i < count; i++)
            {
                var article = new Article
                {
                    Title = $"{baseTitle} {i + 1}", // Unik titel med baseTitle og et tal tilføjet
                    BodyText = $"This is the body of the article {i + 1}.", // Generisk body tekst
                    AuthorId = authorList[i % authorList.Count].UserId, // Tildel en forfatter fra listen
                    Author = authorList[i % authorList.Count], // Tildel Author objektet
                    Genres = new List<Genre> { genreList[i % genreList.Count] }, // Tildel en genre fra listen
                    Updated = DateTime.Now, // Sæt opdateringsdato til nu
                    Deleted = null // Ingen slettede datoer
                };

                articles.Add(article);
            }

            return articles;
        }
    }
}