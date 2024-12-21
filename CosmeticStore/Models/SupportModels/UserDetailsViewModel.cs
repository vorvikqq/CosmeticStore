using Microsoft.AspNetCore.Identity;

namespace CosmeticStore.Models.SupportModels
{
    public class UserDetailsViewModel
    {
        public IdentityUser User { get; set; }
        public bool IsBlocked { get; set; }
    }
}
