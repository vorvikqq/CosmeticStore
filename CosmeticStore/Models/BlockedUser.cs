namespace CosmeticStore.Models
{
    public class BlockedUser
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public DateTime BlockedAt { get; set; }
    }
}
