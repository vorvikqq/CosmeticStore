using CosmeticStore.Repository;

namespace CosmeticStore.Models.SupportModels
{
    public class PerformanceViewModel
    {
        public List<ResultRow> SmallDataResults { get; set; }
        public List<ResultRow> LargeDataResults { get; set; }
    }
}
