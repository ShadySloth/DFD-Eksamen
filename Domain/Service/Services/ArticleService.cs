using System;
using System.Collections.Generic;
using Database_Benchmarking.Domain.Entities;
using Database_Benchmarking.Infrastructure.Repository.Interfaces;
using Database_Benchmarking.Domain.Service.Interfaces;

namespace Database_Benchmarking.Domain.Service.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _repository;

        public ArticleService(IArticleRepository repository)
        {
            _repository = repository;
        }

        // Hent alle artikler
        public TimeSpan GetAllArticles()
        {
            return _repository.GetAll();
        }

        // Hent artikler baseret på ID'er
        public TimeSpan GetArticleById(ICollection<EntityId> ids)
        {
            return _repository.GetById(ids);
        }

        // Opret artikler
        public TimeSpan CreateArticle(ICollection<Article> articles)
        {
            return _repository.Create(articles);
        }

        // Opdater artikler
        public TimeSpan UpdateArticle(ICollection<Article> articles)
        {
            return _repository.Update(articles);
        }

        // Slet artikler
        public TimeSpan DeleteArticle(ICollection<EntityId> ids)
        {
            return _repository.Delete(ids);
        }
    }
}