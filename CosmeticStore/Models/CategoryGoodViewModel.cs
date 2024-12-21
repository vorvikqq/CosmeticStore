using Microsoft.AspNetCore.Mvc.Rendering;

namespace CosmeticStore.Models
{
    public class CategoryGoodViewModel
    {
        public SelectList? Categories { get; set; }
        public List<Good>? Goods { get; set; }
        public string? CategoryName { get; set; }
        public decimal MaxPrice { get; set; }
        public string? SortBy { get; set; }

    }
}
