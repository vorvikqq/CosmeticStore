using CosmeticStore.Data;
using CosmeticStore.Models;
using CosmeticStore.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;

namespace CosmeticStore.Repository
{
    public class SupportRepository : ISupportRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public SupportRepository( UserManager<IdentityUser> userManager, IHttpContextAccessor contextAccessor, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _contextAccessor = contextAccessor;
            _context = dbContext;
        }

        public string GetUserId()
        {
            var user = _contextAccessor.HttpContext.User;

            var userId = _userManager.GetUserId(user);

            return userId;
        }

        public async Task<IdentityUser> GetUserByIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                Console.WriteLine($"User with ID {userId} not found.");
            }

            return user;
        }

        public async Task<IList<string>> GetUserRolesAsync(IdentityUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }
        public bool IsUserBlocked(string userId)
        {
            return _context.BlockedUsers.Any(b => b.UserId == userId);
        }
        public void BlockUser(string userId)
        {
            if (!_context.BlockedUsers.Any(b => b.UserId == userId))
            {
                _context.BlockedUsers.Add(new BlockedUser
                {
                    UserId = userId,
                    BlockedAt = DateTime.UtcNow
                });
                _context.SaveChanges();
            }
        }

        public void UnblockUser(string userId)
        {
            var blockedUser = _context.BlockedUsers.FirstOrDefault(b => b.UserId == userId);
            if (blockedUser != null)
            {
                _context.BlockedUsers.Remove(blockedUser);
                _context.SaveChanges();
            }
        }


    }
}
