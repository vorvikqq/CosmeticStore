using CosmeticStore.Data;
using CosmeticStore.Models;
using CosmeticStore.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CosmeticStore.Repository
{

    public class GoodTestRepository : IGoodTestRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public GoodTestRepository(ApplicationDbContext context, DbContextOptions<ApplicationDbContext> options)
        {
            _context = context;
            _options = options;
        }

        public async Task<ResultRow> MeasureSequentialAsync(int count)
        {
            var stopwatch = Stopwatch.StartNew();
            var data = await _context.GoodTests.Take(count).ToListAsync();
            stopwatch.Stop();

            return new ResultRow
            {
                Technology = "Послідовно",
                Time = stopwatch.ElapsedMilliseconds
            };
        }

        public async Task<ResultRow> MeasureParallelAsync(int count)
        {
            var stopwatch = Stopwatch.StartNew();

            // Завантаження даних
            var data = await _context.GoodTests.ToListAsync();

            // Паралельна обробка
            var parallelData = data.AsParallel().Take(count).ToList();

            stopwatch.Stop();

            return new ResultRow
            {
                Technology = "Паралельно (PLINQ)",
                Time = stopwatch.ElapsedMilliseconds
            };
        }


        public async Task<ResultRow> MeasureMultithreadedAsync(int count)
        {
            var stopwatch = Stopwatch.StartNew();
            var cores = 6;

            var tasks = Enumerable.Range(0, cores).Select(async i =>
            {
                using (var context = new ApplicationDbContext(_options)) // Створюємо новий екземпляр DbContext
                {
                    return await context.GoodTests
                        .Skip(i * (count / cores))
                        .Take(count / cores)
                        .ToListAsync();
                }
            });

            await Task.WhenAll(tasks);

            stopwatch.Stop();

            return new ResultRow
            {
                Technology = "На кількох ядрах",
                Time = stopwatch.ElapsedMilliseconds
            };
        }


        public async Task<ResultRow> MeasureTPLAsync(int count)
        {
            var stopwatch = Stopwatch.StartNew();

            Parallel.ForEach(Enumerable.Range(0, count / 1000), i =>
            {
                using (var context = new ApplicationDbContext(_options))
                {
                    context.GoodTests.Skip(i * 1000).Take(1000).ToList();
                }
            });

            stopwatch.Stop();

            return new ResultRow
            {
                Technology = "Task Parallel Library",
                Time = stopwatch.ElapsedMilliseconds
            };
        }

        public async Task EnsureLargeDataAsync()
        {
            Random random = new Random();
            if (!_context.GoodTests.Any())
            {
                var goodTests = Enumerable.Range(1, 100_000).Select(i => new GoodTest
                {
                    CategoryId = random.Next(1, 5),
                    Name = $"Test {i}"
                }).ToList();

                await _context.GoodTests.AddRangeAsync(goodTests);
                await _context.SaveChangesAsync();
            }
        }
    }
    public class ResultRow
    {
        public string Technology { get; set; }
        public long Time { get; set; }
    }

}
