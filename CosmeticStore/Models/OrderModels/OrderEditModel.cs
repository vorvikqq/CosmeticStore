using System.ComponentModel.DataAnnotations;

namespace CosmeticStore.Models.OrderModels
{
    public class OrderEditModel
    {
        public int Id { get; set; } // Ідентифікатор замовлення

        [Required]
        [MaxLength(30)]
        public string? Name { get; set; } // Ім'я

        [Required]
        [MaxLength(200)]
        public string? ShippingAddress { get; set; } // Адреса доставки
        [Required]
        public string? PaymentMethod { get; set; }
    }
}
