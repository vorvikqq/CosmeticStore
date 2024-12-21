using CosmeticStore.Models;
using CosmeticStore.Models.OrderModels;

namespace CosmeticStore.Repository.Interfaces
{
    public interface ICartRepository
    {
        Task<int> AddItem(int goodId, int quantity);
        Task<int> RemoveItem(int goodId);
        Task<ShoppingCart> GetUserCart();
        Task<int> GetCartItemCount(string userId = "");
        Task<ShoppingCart> GetCart(string userId);
        Task<bool> DoCheckout(CheckoutModel model);
    }
}
