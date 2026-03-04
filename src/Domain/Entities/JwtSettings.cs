

namespace Domain.Entities
{
    public class JwtSettings
    {
        public string Secret { get; set; } = null!;
        public int AccessTokenExpirationMinutes { get; set; }
        public int RefreshTokenExpirationDays { get; set; }
    }
}
