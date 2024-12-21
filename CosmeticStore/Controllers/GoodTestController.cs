using CosmeticStore.Models.SupportModels;
using CosmeticStore.Repository;
using CosmeticStore.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace CosmeticStore.Controllers
{
    public class GoodTestController : Controller
    {
        private readonly IGoodTestRepository _repository;

        public GoodTestController(IGoodTestRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> PerformanceTest()
        {
            await _repository.EnsureLargeDataAsync();

            var smallDataResults = new List<ResultRow>
            {
                await _repository.MeasureSequentialAsync(20),
                await _repository.MeasureParallelAsync(20),
                await _repository.MeasureMultithreadedAsync(20),
                await _repository.MeasureTPLAsync(20)
            };

            var largeDataResults = new List<ResultRow>
            {
                await _repository.MeasureSequentialAsync(100_000),
                await _repository.MeasureParallelAsync(100_000),
                await _repository.MeasureMultithreadedAsync(100_000),
                await _repository.MeasureTPLAsync(100_000)
            };

            return View(new PerformanceViewModel
            {
                SmallDataResults = smallDataResults,
                LargeDataResults = largeDataResults
            });
        }

    }

}
