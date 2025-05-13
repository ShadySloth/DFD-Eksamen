using Database_Benchmarking.Domain.Entities;
using Database_Benchmarking.Infrastructure.Context;
using Database_Benchmarking.Infrastructure.Repository.Interfaces;

namespace Database_Benchmarking.Infrastructure.Repository.PostgresSQL
{
    public class PostgresAuthorRepository : IAuthorRepository
    {
        private readonly PostgresDbContext _context;

        public PostgresAuthorRepository(PostgresDbContext context)
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
            _context.Authors.AddRange(authors);
            _context.SaveChanges();

            foreach (var author in authors)
            {
                author.Name += " - updated";
            }

            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            _context.Authors.UpdateRange(authors);
            _context.SaveChanges();

            stopwatch.Stop();
            return stopwatch.Elapsed;
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
