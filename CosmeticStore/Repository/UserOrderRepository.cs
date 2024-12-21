using CosmeticStore.Constants;
using CosmeticStore.Data;
using CosmeticStore.Models;
using CosmeticStore.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CosmeticStore.Repository
{
    public class UserOrderRepository : IUserOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ISupportRepository _supportRepository;

        public UserOrderRepository(ApplicationDbContext context, ISupportRepository supportRepository)
        {
            _context = context;
            _supportRepository = supportRepository;
        }

        public async Task<IEnumerable<Order>> UserOrders()
        {
            var userId = _supportRepository.GetUserId();

            if (string.IsNullOrEmpty(userId))
                throw new Exception("User is not logged-in");

            var orders = _context.Orders
                          .Include(x => x.OrderStatus)
                          .Include(x => x.OrderDetail)
                          .ThenInclude(x => x.Good)
                          .ThenInclude(x => x.Category).AsQueryable()
                          .Where(a => a.UserId == userId);

            return await orders.ToListAsync();
        }

        public async Task CancelOrder(int orderId)
        {
            var orderToCancel = _context.Orders
                .FirstOrDefault(x => x.Id == orderId);
            
            orderToCancel.OrderStatusId = (int)OrderStatusNames.Cancelled;

            // Зберігаємо зміни в базі
            await _context.SaveChangesAsync();
        }

        public async Task ApproveOrder(int orderId)
        {
            var orderToApprove = _context.Orders
                .FirstOrDefault(x => x.Id == orderId);

            orderToApprove.OrderStatusId = (int)OrderStatusNames.Delivered;

            // Зберігаємо зміни в базі
            await _context.SaveChangesAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderStatus) 
                .FirstOrDefaultAsync(o => o.Id == orderId); 
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order); 
            await _context.SaveChangesAsync();
        }
    }
}
