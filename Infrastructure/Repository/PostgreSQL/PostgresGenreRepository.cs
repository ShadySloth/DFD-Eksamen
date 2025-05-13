using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Database_Benchmarking.Domain.Entities;
using Database_Benchmarking.Infrastructure.Context;
using Database_Benchmarking.Infrastructure.Repository.Interfaces;

namespace Database_Benchmarking.Infrastructure.Repository
{
    public class GenreRepository : IGenreRepository
    {
        private readonly PostgresContext _context;

        public GenreRepository(PostgresContext context)
        {
            _context = context;
        }

        public TimeSpan GetAll()
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            var genres = _context.Genres.ToList();

            stopwatch.Stop();
            return stopwatch.Elapsed; // Returnerer den tid, det tog at hente alle genrer
        }

        public TimeSpan GetById(ICollection<EntityId> ids)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            // Antager at EntityId er en int og vi kan konvertere det til en liste af id'er
            var genres = _context.Genres
                .Where(g => ids.Contains(g.Id))
                .ToList();

            stopwatch.Stop();
            return stopwatch.Elapsed; // Returnerer den tid, det tog at hente genrer med de angivne id'er
        }

        public TimeSpan Create(ICollection<Genre> genres)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            _context.Genres.AddRange(genres);
            _context.SaveChanges();

            stopwatch.Stop();
            return stopwatch.Elapsed; // Returnerer den tid, det tog at oprette genrerne
        }

        public TimeSpan Update(ICollection<Genre> genres)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            _context.Genres.UpdateRange(genres);
            _context.SaveChanges();

            stopwatch.Stop();
            return stopwatch.Elapsed; // Returnerer den tid, det tog at opdatere genrerne
        }

        public TimeSpan Delete(ICollection<EntityId> ids)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            var genresToDelete = _context.Genres
                .Where(g => ids.Contains(g.Id))
                .ToList();

            _context.Genres.RemoveRange(genresToDelete);
            _context.SaveChanges();

            stopwatch.Stop();
            return stopwatch.Elapsed; // Returnerer den tid, det tog at slette genrerne
        }
    }
}
