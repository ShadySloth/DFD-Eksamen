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

        public TimeSpan GetAllArticles(ICollection<Article> articles)
        {
            return _repository.GetAll(articles);
        }

        public TimeSpan GetById(ICollection<Article> articles, int indexToGet)
        {
            return _repository.GetById(articles, indexToGet);
        }

        public TimeSpan CreateArticle(ICollection<Article> articles)
        {
            return _repository.Create(articles);
        }

        public TimeSpan UpdateArticle(ICollection<Article> articles)
        {
            return _repository.Update(articles);
        }

        public TimeSpan DeleteArticle(ICollection<Article> articles)
        {
            return _repository.Delete(articles);
        }
    }
}