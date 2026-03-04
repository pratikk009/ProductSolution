using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Domain.Entities;
using System.Text;

namespace API.Tests
{
    public class CustomWebApplicationFactory
        : WebApplicationFactory<Program>  // Must match the Program class in your API project
    {
        protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove real DB context
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (descriptor != null)
                    services.Remove(descriptor);

                // Add InMemory database
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                // Build service provider to seed data
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Seed admin user if not exists
                if (!context.Users.Any(u => u.Username == "admin"))
                {
                    var password = "Admin@123";
                    var Password = BCrypt.Net.BCrypt.HashPassword(password);

                    context.Users.Add(new User
                    {
                        Id = 1,
                        Username = "admin",
                        PasswordHash = Password,  // <-- now compatible with BCrypt.Verify
                        Role = "Admin",
                        RefreshTokens = new List<RefreshToken>()
                    });

                    context.SaveChanges();
                }

            }); 
        }
            } 
    }