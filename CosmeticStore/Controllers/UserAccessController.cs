using CosmeticStore.Models.SupportModels;
using CosmeticStore.Models;
using CosmeticStore.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CosmeticStore.Repository.Interfaces;

namespace CosmeticStore.Controllers
{
    public class UserAccessController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ISupportRepository _supportRepository;

        // Ініціалізація через конструктор
        public UserAccessController(UserManager<IdentityUser> userManager, ISupportRepository supportRepository)
        {
            _userManager = userManager;
            _supportRepository = supportRepository;
        }

        // Виводить список всіх користувачів
        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            ViewBag.SupportRepository = _supportRepository;

            return View(users);
        }

        // Створення або видалення користувача зі списку заблокованих
        public async Task<IActionResult> ToggleBlock(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Перевіряємо, чи є користувач у списку заблокованих
            if (_supportRepository.IsUserBlocked(userId))
            {
                // Якщо користувач заблокований, розблокуємо його
                _supportRepository.UnblockUser(userId);
            }
            else
            {
                // Якщо користувач не заблокований, блокуємо його
                _supportRepository.BlockUser(userId);
            }

            return RedirectToAction(nameof(Index));
        }

        // Діємо на сторінку конкретного користувача
        public async Task<IActionResult> Details(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var isBlocked = _supportRepository.IsUserBlocked(userId);
            var model = new UserDetailsViewModel
            {
                User = user,
                IsBlocked = isBlocked
            };
            return View(model);
        }
    }
}
