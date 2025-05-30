﻿using System.Collections.Generic;
using Database_Benchmarking.Domain.Entities;
using Database_Benchmarking.Infrastructure.MockObjectBuilders.Interfaces;

namespace Database_Benchmarking.Infrastructure.MockObjectBuilders.Builders
{
    public class MockAuthorBuilder : IAuthorMockBuilder
    {
        // Metode til at bygge en liste af mock-authors med et grundlæggende navn og unikt tal
        public List<Author> BuildAuthors(int count, string baseName)
        {
            var authors = new List<Author>();

            for (int i = 0; i < count; i++)
            {
                var author = new Author
                {
                    AuthorName = $"{baseName} {i + 1}"
                };

                authors.Add(author);
            }

            return authors;
        }
    }
}