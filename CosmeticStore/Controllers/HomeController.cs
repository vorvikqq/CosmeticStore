using CosmeticStore.Data;
using CosmeticStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using CosmeticStore.Repository.Interfaces;
using CosmeticStore.Repository;

namespace CosmeticStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository;


        public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository)
        {
            _logger = logger;
            _homeRepository = homeRepository;
        }

        public async Task<IActionResult> Index(string categoryName  = "", decimal? maxPrice = null)
        {
            var categories = await _homeRepository.GetCategoryList();
            var goods = await _homeRepository.DisplayGoods(categoryName, maxPrice);
            int totalGoods = goods.Count;

            ViewBag.TotalGoods = totalGoods;

            var model = new CategoryGoodViewModel
            {
                Categories = new SelectList(categories),
                Goods = goods,
            };



            ViewData["ActivePage"] = "Home";
            return View(model);
        }

        public async Task<IActionResult> SortBy(string sortBy)
        {
            var goods = await _homeRepository.DisplaySortedGoods(sortBy);
            var categories = await _homeRepository.GetCategoryList();


            var model = new CategoryGoodViewModel
            {
                Categories = new SelectList(categories),
                Goods = goods,
            };

            int totalGoods = goods.Count;

            ViewBag.TotalGoods = totalGoods;


            ViewData["ActivePage"] = "Home";
            return View("Index", model);
        }
        public IActionResult Privacy()
        {
            ViewData["ActivePage"] = "Privacy";
            return View();
        }

        public IActionResult AdminPanel()
        {
            ViewData["ActivePage"] = "AdminPanel";
            return View();
        }
        public IActionResult SellerPanel()
        {
            ViewData["ActivePage"] = "SellerPanel";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
