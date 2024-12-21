using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CosmeticStore.Models
{
    [Table("Good")]
    public class Good
    {
        public int Id { get; set; }

        [Required]
        public string? UserId { get; set; }
        [MaxLength(40)]
        public string? Name { get; set; }
        
        public string? Description { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime ProductionDate { get; set; }
        
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        public int CategoryId { get; set; }
        public ApplicationUser? User { get; set; }

        public Category? Category { get; set; }

        public List<OrderDetail>? OrderDetail { get; set; }

        public List<CartDetail>? CartDetail { get; set; }

    }
}
