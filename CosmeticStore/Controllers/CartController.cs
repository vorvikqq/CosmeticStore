using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CosmeticStore.Repository.Interfaces;
using System.Net;
using CosmeticStore.Models;
using CosmeticStore.Models.OrderModels;

namespace CosmeticStore.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;
        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async  Task<IActionResult> AddItem(int goodId, int quantity = 1, int redirect = 0)
        {
            var cartCount = await _cartRepository.AddItem(goodId, quantity);

            if (redirect == 0)
                return Ok(cartCount);
            return RedirectToAction("GetUserCart");
        }

        public async Task<IActionResult> RemoveItem(int goodId)
        {
            var cartCount = await _cartRepository.RemoveItem(goodId);
            return RedirectToAction("GetUserCart");
        }

        public async Task<IActionResult> GetUserCart()
        {
            var cart = await _cartRepository.GetUserCart();
            return View(cart);
        }

        public async Task<IActionResult> GetTotalItemInCart()
        {
            var cartItem = await _cartRepository.GetCartItemCount();
            return Ok(cartItem);
        }

        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            bool isCheckedOut = await _cartRepository.DoCheckout(model);
            if (!isCheckedOut)
                return RedirectToAction(nameof(OrderFailure));
            return RedirectToAction(nameof(OrderSuccess));
        }

        public IActionResult OrderSuccess()
        {
            return View();
        }

        public IActionResult OrderFailure()
        {
            return View();
        }
    }
}
