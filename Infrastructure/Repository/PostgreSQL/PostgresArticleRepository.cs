using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Database_Benchmarking.Domain.Entities;
using Database_Benchmarking.Infrastructure.Context;
using Database_Benchmarking.Infrastructure.Repository.Interfaces;

namespace Database_Benchmarking.Infrastructure.Repository
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly PostgresContext _context;

        public ArticleRepository(PostgresContext context)
        {
            _context = context;
        }

        public TimeSpan GetAll()
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            var articles = _context.Articles.ToList();

            stopwatch.Stop();
            return stopwatch.Elapsed; // Returnerer den tid, det tog at hente alle artikler
        }

        public TimeSpan GetById(ICollection<EntityId> ids)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            var articles = _context.Articles
                .Where(a => ids.Contains(a.Id))
                .ToList();

            stopwatch.Stop();
            return stopwatch.Elapsed; // Returnerer den tid, det tog at hente artiklerne med de angivne id'er
        }

        public TimeSpan Create(ICollection<Article> articles)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            _context.Articles.AddRange(articles);
            _context.SaveChanges();

            stopwatch.Stop();
            return stopwatch.Elapsed; // Returnerer den tid, det tog at oprette artiklerne
        }

        public TimeSpan Update(ICollection<Article> articles)
        {
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



        public TimeSpan Delete(ICollection<EntityId> ids)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            var articlesToDelete = _context.Articles
                .Where(a => ids.Contains(a.Id))
                .ToList();

            _context.Articles.RemoveRange(articlesToDelete);
            _context.SaveChanges();

            stopwatch.Stop();
            return stopwatch.Elapsed; // Returnerer den tid, det tog at slette artiklerne
        }
    }
}
