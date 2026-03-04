using Application.DTOs;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly ApplicationDbContext _context;

        public AuthController(JwtService jwtService, ApplicationDbContext context)
        {
            _jwtService = jwtService;
            _context = context;
        }

      

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto loginModel)
        {
            if (loginModel == null || string.IsNullOrEmpty(loginModel.Username) || string.IsNullOrEmpty(loginModel.PasswordHash))
            {
                return BadRequest("Invalid login data.");
            }

            var user = _context.Users.FirstOrDefault(u => u.Username == loginModel.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginModel.PasswordHash, user.PasswordHash))
            {
                return Unauthorized("Invalid credentials");
            }

            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();
            _jwtService.SaveRefreshToken(user, refreshToken);

            return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });
        }   
        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] RefreshTokenModel refreshTokenModel)
        {
            var newAccessToken = _jwtService.RefreshAccessToken(refreshTokenModel.RefreshToken);
            if (newAccessToken == null)
            {
                return Unauthorized("Invalid refresh token");
            }

            return Ok(new { AccessToken = newAccessToken });
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == model.Username);

            if (existingUser != null)
                return BadRequest("User already exists");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

            var user = new User
            {
                Username = model.Username,
                PasswordHash = hashedPassword,
                Role = "User"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully");
        }


        public class RefreshTokenModel
        {
            public string RefreshToken { get; set; }
        }
    }
}