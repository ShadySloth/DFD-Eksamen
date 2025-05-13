using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Database_Benchmarking.Domain.Entities;
using Database_Benchmarking.Infrastructure.Context;
using Database_Benchmarking.Infrastructure.Repository.Interfaces;

namespace Database_Benchmarking.Infrastructure.Repository
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly PostgresContext _context;

        public AuthorRepository(PostgresContext context)
        {
            _context = context;
        }

        public TimeSpan GetAll()
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            var authors = _context.Authors.ToList();

            stopwatch.Stop();
            return stopwatch.Elapsed; // Returnerer den tid, det tog at hente alle forfattere
        }

        public TimeSpan GetById(ICollection<EntityId> ids)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            // Henter forfattere baseret på id'erne
            var authors = _context.Authors
                .Where(a => ids.Contains(a.UserId))
                .ToList();

            stopwatch.Stop();
            return stopwatch.Elapsed; // Returnerer den tid, det tog at hente forfattere med de angivne id'er
        }

        public TimeSpan Create(ICollection<Author> authors)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            _context.Authors.AddRange(authors);
            _context.SaveChanges();

            stopwatch.Stop();
            return stopwatch.Elapsed; // Returnerer den tid, det tog at oprette forfatterne
        }

        public TimeSpan Update(ICollection<Author> authors)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            _context.Authors.UpdateRange(authors);
            _context.SaveChanges();

            stopwatch.Stop();
            return stopwatch.Elapsed; // Returnerer den tid, det tog at opdatere forfatterne
        }

        public TimeSpan Delete(ICollection<EntityId> ids)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            var authorsToDelete = _context.Authors
                .Where(a => ids.Contains(a.UserId))
                .ToList();

            _context.Authors.RemoveRange(authorsToDelete);
            _context.SaveChanges();

            stopwatch.Stop();
            return stopwatch.Elapsed; // Returnerer den tid, det tog at slette forfatterne
        }
    }
}
