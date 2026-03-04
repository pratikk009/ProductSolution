using Infrastructure.Data;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Linq;

namespace Infrastructure.Tests
{
    public class UserRepositoryTests
    {
        private readonly ApplicationDbContext _context;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDb") // ✅ Requires Microsoft.EntityFrameworkCore.InMemory
                .Options;

            _context = new ApplicationDbContext(options);

            if (!_context.Users.Any())
            {
                _context.Users.Add(new User
                {
                    Username = "testuser",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password@123"), // ✅ Requires BCrypt.Net-Next
                    Role = "User",
                    RefreshTokens = new System.Collections.Generic.List<RefreshToken>()
                });
                _context.SaveChanges();
            }
        }

        [Fact]
        public void Can_Get_User_By_Username()
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == "testuser");
            Assert.NotNull(user);
        }
    }
}