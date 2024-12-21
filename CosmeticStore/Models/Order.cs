using Microsoft.CodeAnalysis.Options;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CosmeticStore.Models
{
    [Table("Order")]
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [MaxLength(40)]
        public string Name { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime OrderDate { get; set; }

        [Required]
        public int OrderStatusId { get; set; }
        
        [Required]
        public string? MobileNumber { get; set; }
        [Required]
        [MaxLength(200)]
        public string ShippingAddress { get; set; }
        [Required]
        [MaxLength(40)]
        public string? PaymentMethod { get; set; }
        public bool IsPaid { get; set; }

        public OrderStatus? OrderStatus { get; set; }

        public List<OrderDetail>? OrderDetail { get; set; }
    }
}
