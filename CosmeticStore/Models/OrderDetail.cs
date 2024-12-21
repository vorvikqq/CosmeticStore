using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CosmeticStore.Models
{
    [Table("OrderDetail")]
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }

        [Required]
        public int OrderId { get; set; }
        
        [Required]
        public int GoodId { get; set; }
        
        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")] 
        public decimal UnitPrice { get; set; }

        public Good? Good { get; set; }

        public Order? Order { get; set; }

    }
}
