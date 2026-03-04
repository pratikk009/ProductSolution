using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Xunit;

namespace API.Tests.Controllers;

public class ProductsControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ProductsControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_Products_Should_Return_200()
    {
        // Get JWT token for admin
        var token = await GetJwtTokenAsync();

        // Attach token to request
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        // Call the endpoint
        var response = await _client.GetAsync("/api/v1/products");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Post_Product_Should_Return_401_Without_Token()
    {
        var product = new
        {
            productName = "Test Product",
            stock = 5,
            price = 100
        };

        var content = new StringContent(
            JsonSerializer.Serialize(product),
            Encoding.UTF8,
            "application/json");

        // Call the POST /products endpoint without JWT
        var response = await _client.PostAsync("/api/v1/products", content);

        // Check that it returns 401 Unauthorized
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    // -------------------------
    // Helper method for login
    // -------------------------
    private async Task<string> GetJwtTokenAsync()
    {
        var loginRequest = new
        {
            username = "admin",
            passwordHash = "Admin@123" // Send plain text; the controller now verifies it
        };

        // Note the route: No "/v1/" here to match AuthController
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        if (doc.RootElement.TryGetProperty("accessToken", out var tokenElement))
        {
            return tokenElement.GetString();
        }
        return doc.RootElement.GetProperty("AccessToken").GetString();
    }
}