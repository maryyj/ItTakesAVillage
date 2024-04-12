using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItTakesAVillage.Tests.IntegrationTests
{
    public class EndPointTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;

        public EndPointTests(WebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateDefaultClient();
        }

        [Theory]
        [InlineData("/Identity/Account/Register")]
        [InlineData("/DinnerInvitation")]
        [InlineData("/Group")]
        [InlineData("/PlayDate")]
        [InlineData("/Notification")]
        [InlineData("/ToolPool")]
        public async Task EndPoints_Existing_ShouldReturnSuccessAndContentType(string url)
        {
            // Arrange

            // Act
            var response = await _httpClient.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(response.Content);
            Assert.NotNull(response.Content.Headers);
            Assert.NotNull(response.Content.Headers.ContentType);
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }
    }
}
