using Microsoft.AspNetCore.Identity;

namespace CosmeticStore.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsBlocked { get; set; } = false; 
    }
}
