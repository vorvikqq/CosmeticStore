using CosmeticStore.Models;

namespace CosmeticStore.Repository.Interfaces
{
    public interface IUserOrderRepository
    {
        Task<IEnumerable<Order>> UserOrders();

        Task CancelOrder(int orderId);
        Task ApproveOrder(int orderId);
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task UpdateOrderAsync(Order order);
    }
}
