using Microsoft.Extensions.DependencyInjection;
using POS.Application.Interfaces;
using POS.IntegrationTests.MockData;
using POS.Utilities.Static;
using Xunit;

namespace POS.IntegrationTests.CategoryApplicationTest
{
    public class CategoryApplicationTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public CategoryApplicationTest(CustomWebApplicationFactory factory)
        {
            _scopeFactory = factory.Services.GetRequiredService<IServiceScopeFactory>();
        }

        [Fact]
        public async Task RegisterCategory_WhenSendingNullValuesOrEmpty_ValidationError()
        {
            // Arrange
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ICategoryApplication>();
            var categoryRequest = CategoryMockData.GetCategoryEmptyRequest();
            var expected = ReplyMessage.MESSAGE_VALIDATE;

            // Act
            var result = await context.RegisterCategory(categoryRequest);

            // Assert
            Assert.Equal(expected, result.Message);
        }

        [Fact(Skip = "Revisar configuracion")]
        public async Task RegisterCategory_WhenSendingCorrectValues_RegisteredSuccessfully()
        {
            // Arrange
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ICategoryApplication>();
            var categoryRequest = CategoryMockData.GetCategoryRequest();
            var expected = ReplyMessage.MESSAGE_SAVE;

            // Act
            var result = await context.RegisterCategory(categoryRequest);

            // Assert
            Assert.Equal(expected, result.Message);
        }
    }
}
