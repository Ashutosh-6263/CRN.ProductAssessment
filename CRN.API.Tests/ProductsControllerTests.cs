using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace CRN.API.Tests
{
    public class ProductsControllerTests
        : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ProductsControllerTests(
            CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetProducts_WithoutAuthentication_ReturnsUnauthorized()
        {
            var response = await _client.GetAsync(
                "/api/v1.0/products?pageNumber=1&pageSize=10");

            Assert.Equal(
                HttpStatusCode.Unauthorized,
                response.StatusCode);
        }

        [Fact]
        public async Task GetProducts_AsUser_ReturnsOk()
        {
            _client.DefaultRequestHeaders.Add(
                "X-Test-Role",
                "User");

            var response = await _client.GetAsync(
                "/api/v1.0/products?pageNumber=1&pageSize=10");

            Assert.Equal(
                HttpStatusCode.OK,
                response.StatusCode);
        }

        [Fact]
        public async Task CreateProduct_AsUser_ReturnsForbidden()
        {
            _client.DefaultRequestHeaders.Add(
                "X-Test-Role",
                "User");

            var content = new StringContent(
                """
                {
                    "productName": "Test Product"
                }
                """,
                System.Text.Encoding.UTF8,
                "application/json");

            var response = await _client.PostAsync(
                "/api/v1.0/products",
                content);

            Assert.Equal(
                HttpStatusCode.Forbidden,
                response.StatusCode);
        }
    }
}