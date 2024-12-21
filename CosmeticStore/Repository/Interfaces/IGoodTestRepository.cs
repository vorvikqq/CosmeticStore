using CosmeticStore.Models;

namespace CosmeticStore.Repository.Interfaces
{
    public interface IGoodTestRepository
    {
        Task<ResultRow> MeasureSequentialAsync(int count);
        Task<ResultRow> MeasureParallelAsync(int count);
        Task<ResultRow> MeasureMultithreadedAsync(int count);
        Task<ResultRow> MeasureTPLAsync(int count);
        Task EnsureLargeDataAsync();
    }
}
