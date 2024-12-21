using CosmeticStore.Models;

namespace CosmeticStore.Repository.Interfaces
{
    public interface IHomeRepository
    {
        Task<List<Good>> DisplayGoods(string categoryName = "", decimal? maxPrice = null);
        Task<List<string>> GetCategoryList();
        Task<List<Good>> DisplaySortedGoods(string sortBy);
    }
}
