using CosmeticStore.Data;
using CosmeticStore.Models;
using CosmeticStore.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Numerics;

namespace CosmeticStore.Repository
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _context;

        public HomeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Good>> DisplayGoods(string categoryName = "", decimal? maxPrice = null)
        {
            var goods = from good in _context.Goods
                        select good;

            
            if (!string.IsNullOrEmpty(categoryName))
            {
                goods = goods.Where(x => x.Category!.Name == categoryName);
            }

            if (maxPrice.HasValue)
            {
                goods = goods.Where(g => g.Price <= maxPrice.Value);
            }

            return await goods.ToListAsync();
        }

        public async Task<List<Good>> DisplaySortedGoods(string sortBy)
        {
            var goods = from good in _context.Goods
                        select good;

            goods = sortBy switch
            {
                "price" => goods = goods.OrderBy(x => x.Price),
                "date" => goods = goods.OrderBy(x => x.ProductionDate),
                _ => goods,
            };

            return await goods.ToListAsync();
        }

        public async Task<List<string>> GetCategoryList()
        {
            var categories = from category in _context.Categories orderby category.Name select category.Name;

            return await categories.Distinct().ToListAsync();
        }
    }
}
