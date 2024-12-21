using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CosmeticStore.Models
{
    [Table("Category")]
    public class Category
    {
        public int Id { get; set; }

        [MaxLength(40)]
        public string? Name { get; set; }

        public string Description { get; set; }

        public List<Good>? Goods { get; set; }
    }
}
