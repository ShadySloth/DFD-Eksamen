using Database_Benchmarking.Domain.Entities;
using Database_Benchmarking.Infrastructure.Context;
using Database_Benchmarking.Infrastructure.Repository.Interfaces;

namespace Database_Benchmarking.Infrastructure.Repository.PostgresSQL
{
    public class PostgresGenreRepository : IGenreRepository
    {
        private readonly PostgresDbContext _context;

        public PostgresGenreRepository(PostgresDbContext context)
        {
            _context = context;
        }

        public TimeSpan GetAll()
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            var genres = _context.Genres.ToList();

            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
        
        public TimeSpan Create(ICollection<Genre> genres)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            _context.Genres.AddRange(genres);
            _context.SaveChanges();

            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

        public TimeSpan Update(ICollection<Genre> genres)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            _context.Genres.UpdateRange(genres);
            _context.SaveChanges();

            stopwatch.Stop();
            return stopwatch.Elapsed;
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
            return stopwatch.Elapsed;
        }
    }
}
