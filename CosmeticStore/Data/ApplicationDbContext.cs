using CosmeticStore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CosmeticStore.Data
{
        public class ApplicationDbContext : IdentityDbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
            {
            }

            public DbSet<Good> Goods { get; set; }
            public DbSet<Category> Categories { get; set; }
            public DbSet<ShoppingCart> ShoppingCarts { get; set; }
            public DbSet<CartDetail> CartDetails { get; set; }
            public DbSet<Order> Orders { get; set; }
            public DbSet<OrderDetail> OrderDetails { get; set; }
            public DbSet<OrderStatus> OrderStatuses { get; set; }
            public DbSet<BlockedUser> BlockedUsers { get; set; }
            public DbSet<SoldItem> SoldItems { get; set; }
            public DbSet<GoodTest> GoodTests { get; set; }
        
    }
}
