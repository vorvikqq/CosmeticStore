using CosmeticStore.Models.OrderModels;
using CosmeticStore.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticStore.Controllers
{
    [Authorize]
    public class UserOrderController : Controller
    {
        private readonly IUserOrderRepository _userOrderRepository;
        public UserOrderController(IUserOrderRepository userOrderRepository)
        {
            _userOrderRepository = userOrderRepository;
        }

        public async Task<IActionResult> UserOrder()
        {
            var orders = await _userOrderRepository.UserOrders();

            return View(orders);
        }

        public async Task<IActionResult> CancelORder(int orderId)
        {
            await _userOrderRepository.CancelOrder(orderId);
            return RedirectToAction("UserOrder");
        }

        public async Task<IActionResult> ApproveOrder(int orderId)
        {
            await _userOrderRepository.ApproveOrder(orderId);
            return RedirectToAction("UserOrder");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int orderId)
        {
            var order = await _userOrderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }

            // Перетворюємо Order в OrderEditModel
            var model = new OrderEditModel
            {
                Id = order.Id,
                Name = order.Name,
                ShippingAddress = order.ShippingAddress,
                PaymentMethod = order.PaymentMethod
            };

            return View(model);
        }

        // POST: /Order/Edit/{orderId}
        [HttpPost]
        public async Task<IActionResult> Edit(OrderEditModel model)
        {
            if (ModelState.IsValid)
            {
                var order = await _userOrderRepository.GetOrderByIdAsync(model.Id);
                if (order == null)
                {
                    return NotFound();
                }

                // Оновлюємо дані замовлення
                order.Name = model.Name;
                order.ShippingAddress = model.ShippingAddress;
                order.PaymentMethod = model.PaymentMethod;

                await _userOrderRepository.UpdateOrderAsync(order);
                return RedirectToAction("UserOrder");
            }

            return View(model);
        }
    }
}
