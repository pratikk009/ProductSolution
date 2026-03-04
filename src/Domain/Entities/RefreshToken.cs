namespace Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = null!;  // Required
        public DateTime Expires { get; set; }
        public bool IsRevoked { get; set; }
        public int UserId { get; set; }             // FK to Users
        public DateTime CreatedOn { get; set; }
        public DateTime ExpiresOn { get; set; }

        // Navigation property to User entity
        public User User { get; set; } = null!;
    }
}