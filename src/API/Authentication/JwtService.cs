using Domain.Entities;
using Infrastructure.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtService
{
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _context;

    public JwtService(IConfiguration configuration, ApplicationDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    public string GenerateAccessToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[] {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role) 

        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(15),  
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

   
    public string GenerateRefreshToken()
    {
        return Guid.NewGuid().ToString();
    }

    public void SaveRefreshToken(User user, string refreshToken)
    {
        var existingToken = _context.RefreshTokens.SingleOrDefault(rt => rt.UserId == user.Id);
        if (existingToken != null)
        {
            existingToken.Token = refreshToken;
            existingToken.ExpiresOn = DateTime.Now.AddDays(7); // 7 days 
        }
        else
        {
            var newToken = new RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                CreatedOn = DateTime.Now,
                ExpiresOn = DateTime.Now.AddDays(7),
            };
            _context.RefreshTokens.Add(newToken);
        }

        _context.SaveChanges();
    }

    // Validate refresh token
    public bool ValidateRefreshToken(string refreshToken)
    {
        var token = _context.RefreshTokens
            .SingleOrDefault(rt => rt.Token == refreshToken && !rt.IsRevoked);

        if (token == null)
        {
            return false;
        }

        return token.ExpiresOn > DateTime.UtcNow;
    }

    // Get user by refresh token
    public User GetUserByRefreshToken(string refreshToken)
    {
        var token = _context.RefreshTokens.SingleOrDefault(rt => rt.Token == refreshToken);
        return token?.User;
    }

    // Refresh the access token using a valid refresh token
    public string RefreshAccessToken(string refreshToken)
    {
        if (!ValidateRefreshToken(refreshToken))
            return null;

        var user = GetUserByRefreshToken(refreshToken);
        if (user == null) return null;

        var newAccessToken = GenerateAccessToken(user);
        var newRefreshToken = GenerateRefreshToken();

        SaveRefreshToken(user, newRefreshToken);

        return newAccessToken;
    }
}