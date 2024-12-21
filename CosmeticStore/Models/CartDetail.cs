using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CosmeticStore.Models
{
    [Table("CartDetail")]
    public class CartDetail
    {
        public int Id { get; set; }

        [Required]
        public int ShoppingCartId { get; set; }

        [Required]
        public int GoodId { get; set; }

        [Required]
        public int Quantity { get; set; }

        public ShoppingCart? ShoppingCart { get; set; }

        public Good? Good { get; set; }
    }
}
