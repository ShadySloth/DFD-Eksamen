using Database_Benchmarking.Domain.Entities;
using Database_Benchmarking.Infrastructure.Context;
using Database_Benchmarking.Infrastructure.Repository.Interfaces;

namespace Database_Benchmarking.Infrastructure.Repository.PostgresSQL;
    public class PostgresArticleRepository : IArticleRepository
    {
        private readonly PostgresDbContext _context;

        public PostgresArticleRepository(PostgresDbContext context)
        {
            _context = context;
        }

        public TimeSpan GetAll(ICollection<Article> articles)
        {
            ClearArticles();
            _context.Articles.AddRange(articles);
            _context.SaveChanges();
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            var newArticles = _context.Articles.ToList();

            stopwatch.Stop();
            return stopwatch.Elapsed; // Returnerer den tid, det tog at hente alle artikler
        }

        public TimeSpan GetById(ICollection<Article> articles, int indexToGet)
        {
            ClearArticles();
            _context.Articles.AddRange(articles);
            _context.SaveChanges();
            
            var entId = new EntityId(articles.ElementAt(indexToGet).Id.Value);

            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            // Her henter du artiklen fra databasen ved ID
            var foundArticle = _context.Articles.Find(entId);

            stopwatch.Stop();
            return stopwatch.Elapsed;
        }


        public TimeSpan Create(ICollection<Article> articles)
        {
            ClearArticles();
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            _context.Articles.AddRange(articles);
            _context.SaveChanges();

            stopwatch.Stop();
            return stopwatch.Elapsed; // Returnerer den tid, det tog at oprette artiklerne
        }

        public TimeSpan Update(ICollection<Article> articles)
        {
            ClearArticles();
            _context.Articles.AddRange(articles);
            _context.SaveChanges();

            foreach (var article in articles)
            {
                article.Updated = DateTime.UtcNow;
            }

            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            _context.Articles.UpdateRange(articles);
            _context.SaveChanges();

            stopwatch.Stop();
            return stopwatch.Elapsed;
        }



        public TimeSpan Delete(ICollection<Article> articles)
        {
            ClearArticles();
            _context.Articles.AddRange(articles);
            _context.SaveChanges();

            var ids = articles.Select(a => a.Id).ToList();

            var articlesToDelete = _context.Articles
                .Where(a => ids.Contains(a.Id))
                .ToList();

            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            _context.Articles.RemoveRange(articlesToDelete);
            _context.SaveChanges();

            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
        
        private void ClearArticles()
        {
            _context.Authors.RemoveRange(_context.Authors);
            _context.Articles.RemoveRange(_context.Articles);
            _context.SaveChanges();
        } 
    }

    