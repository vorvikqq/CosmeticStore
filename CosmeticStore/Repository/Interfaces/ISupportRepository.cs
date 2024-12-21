using Microsoft.AspNetCore.Identity;

namespace CosmeticStore.Repository.Interfaces
{
    public interface ISupportRepository
    {
        string GetUserId();
        Task<IdentityUser> GetUserByIdAsync(string userId);
        Task<IList<string>> GetUserRolesAsync(IdentityUser user);
        bool IsUserBlocked(string userId);
        void BlockUser(string userId);
        void UnblockUser(string userId);
    }
}
