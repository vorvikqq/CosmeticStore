using CosmeticStore.Constants;
using CosmeticStore.Data;
using CosmeticStore.Models;
using CosmeticStore.Models.OrderModels;
using CosmeticStore.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CosmeticStore.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly ISupportRepository _supportRepository;
        
        private readonly ApplicationDbContext _context;

        public CartRepository(ISupportRepository supportRepository, ApplicationDbContext context)
        {
            _supportRepository = supportRepository;
            _context = context;
        }

        public async Task<int> AddItem(int goodId, int quantity)
        {
            string userId = _supportRepository.GetUserId();
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                if (userId.IsNullOrEmpty())
                    throw new UnauthorizedAccessException("user is not logged-in");

                var cart = await GetCart(userId);

                if (cart is null)
                {
                    cart = new ShoppingCart
                    {
                        UserId = userId,
                    }; 
                    
                    _context.ShoppingCarts.Add(cart);
                }

                _context.SaveChanges();

                var cartItem = _context.CartDetails.FirstOrDefault(x => x.ShoppingCartId == cart.Id && x.GoodId == goodId);

                if (cartItem is not null) 
                {
                    cartItem.Quantity += quantity;
                }
                else
                {
                    cartItem = new CartDetail
                    {
                        GoodId = goodId,
                        ShoppingCartId = cart.Id,
                        Quantity = quantity
                    };

                    _context.CartDetails.Add(cartItem);
                }

                _context.SaveChanges();
                
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }

            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }

        public async Task<int> RemoveItem(int goodId)
        {
            using var transaction = _context.Database.BeginTransaction();
            string userId = _supportRepository.GetUserId();
            try
            {

                if (userId.IsNullOrEmpty())
                    throw new UnauthorizedAccessException("user is not logged-in");

                var cart = await GetCart(userId);

                if (cart is null)
                    throw new InvalidOperationException("Invalid cart");
                

                var cartItem = _context.CartDetails.FirstOrDefault(x => x.ShoppingCartId == cart.Id && x.GoodId == goodId);

                if (cartItem is null)
                    throw new InvalidOperationException("Not items in cart");
                else if(cartItem.Quantity == 1)
                    _context.CartDetails.Remove(cartItem);
                else
                    cartItem.Quantity -= 1;
                
                _context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }

            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }


        public async Task<ShoppingCart> GetUserCart()
        {
            var userId = _supportRepository.GetUserId();
            if (userId == null)
                throw new InvalidOperationException("Invalid userid");

            var shoppingCart = await _context.ShoppingCarts
                                  .Include(a => a.CartDetail)!
                                  .ThenInclude(a => a.Good)
                                  .ThenInclude(a => a.Category)
                                  .Where(a => a.UserId == userId).FirstOrDefaultAsync();

            return shoppingCart!;

        }
        public async Task<ShoppingCart> GetCart(string userId)
        {
            var cart = await _context.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == userId);

            return cart!;
        }

        public async Task<bool> DoCheckout(CheckoutModel model)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var userId = _supportRepository.GetUserId();
                var user = await _supportRepository.GetUserByIdAsync(userId);
                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException("User is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new InvalidOperationException("Invalid cart");
                var cartDetail = _context.CartDetails
                                    .Include(a => a.Good)
                                    .Where(a => a.ShoppingCartId == cart.Id).ToList();
                if (cartDetail.Count == 0)
                    throw new InvalidOperationException("Cart is empty");

               

                var order = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.Now,
                    Name = model.Name,
                    MobileNumber = user.PhoneNumber,
                    PaymentMethod = model.PaymentMethod,
                    ShippingAddress = model.Address,
                    IsPaid = false,
                    OrderStatusId = (int)OrderStatusNames.Pending,
                };

                _context.Orders.Add(order);
                _context.SaveChanges();

                foreach(var item in cartDetail)
                {
                    var orderDetail = new OrderDetail
                    {
                        GoodId = item.GoodId,
                        OrderId = order.Id,
                        Quantity = item.Quantity,   
                        UnitPrice = item.Quantity * item.Good.Price,
                    };

                    var soldItem = new SoldItem
                    {
                        GoodId = item.Good.Id,
                        SoldDate = DateTime.Now,
                        Price = item.Good.Price,
                        Quantity = item.Quantity
                    };
                    _context.OrderDetails.Add(orderDetail);
                    _context.SoldItems.Add(soldItem);

                }
                _context.SaveChanges();

                //removing 

                _context.CartDetails.RemoveRange(cartDetail);
                _context.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
            }
        }

        public async Task<int> GetCartItemCount(string userId = "")
        {
            if (string.IsNullOrEmpty(userId)) 
            {
                userId = _supportRepository.GetUserId();
            }
            var data = await (from cart in _context.ShoppingCarts
                              join cartDetail in _context.CartDetails
                              on cart.Id equals cartDetail.ShoppingCartId
                              where cart.UserId == userId 
                              select new { cartDetail.Quantity }
                       ).ToListAsync();
            return data.Sum(x => x.Quantity);
        }
    }
}
