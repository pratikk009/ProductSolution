namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }  // Primary Key

        public string Role { get; set; } = "User";
        public required string  Username { get; set; }
        public required string PasswordHash { get; set; }  // Store hashed password
        public ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}