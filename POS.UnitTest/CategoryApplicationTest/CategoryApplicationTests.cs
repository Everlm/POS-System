using AutoMapper;
using Moq;
using POS.Application.Dtos.Category.Request;
using POS.Application.Mappers;
using POS.Application.Services;
using POS.Application.Validators.Category;
using POS.Domain.Entities;
using POS.Infrastructure.Commons.Bases.Request;
using POS.Infrastructure.Commons.Bases.Response;
using POS.Infrastructure.Persistences.Interfaces;
using POS.UnitTest.MockData;
using Xunit;

namespace PPOS.UnitTest.CategoryApplicationTest
{
    public class CategoryApplicationTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IMapper _mapper;
        private readonly CategoryValidator _validateRules;
        private readonly CategoryApplication _categoryApplication;

        public CategoryApplicationTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<CategoryMappingsProfile>());
            _mapper = config.CreateMapper();

            _validateRules = new CategoryValidator();

            _categoryApplication = new CategoryApplication(
                _unitOfWorkMock.Object,
                _mapper,
                _validateRules
            );
        }

        [Fact]
        public async Task ListCategories_ShouldReturnListOfCategories()
        {
            // Arrange
            var filters = new BaseFiltersRequest();
            var categories = CategoryMockData. GetSampleCategories();
            var baseEntityResponse = new BaseEntityResponse<Category>
            {
                Items = categories,
                TotalRecords = categories.Count
            };

            _unitOfWorkMock.Setup(u => u.Category.ListCategories(filters))
                .ReturnsAsync(baseEntityResponse);

            // Act
            var result = await _categoryApplication.ListCategories(filters);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(categories.Count, result.Data?.Items?.Count);
        }

        [Fact]
        public async Task GetCategoryById_ShouldReturnCategory()
        {
            // Arrange
            var categoryId = 1;
            var category = CategoryMockData.GetSampleCategory(categoryId);

            _unitOfWorkMock.Setup(u => u.Category.GetByIdAsync(categoryId))
                .ReturnsAsync(category);

            // Act
            var result = await _categoryApplication.GetCategoryById(categoryId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(categoryId, result.Data?.CategoryId);
            _unitOfWorkMock.Verify(u => u.Category.GetByIdAsync(categoryId), Times.Once);
        }

        [Fact]
        public async Task RegisterCategory_ShouldReturnTrue()
        {
            // Arrange
            var requestDto = CategoryMockData.GetSampleCategoryRequestDto();

            _unitOfWorkMock.Setup(u => u.Category.RegisterAsync(It.IsAny<Category>()))
                .ReturnsAsync(true);

            // Act
            var result = await _categoryApplication.RegisterCategory(requestDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task EditCategory_ShouldReturnTrue()
        {
            // Arrange
            var categoryId = 1;
            var requestDto = CategoryMockData.GetSampleCategoryRequestDto();
            var existingCategory = CategoryMockData.GetSampleCategory(categoryId);

            _unitOfWorkMock.Setup(u => u.Category.GetByIdAsync(categoryId))
                .ReturnsAsync(existingCategory);

            _unitOfWorkMock.Setup(u => u.Category.EditAsync(It.IsAny<Category>()))
                .ReturnsAsync(true);

            // Act
            var result = await _categoryApplication.EditCategory(requestDto, categoryId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

        [Fact]
        public async Task DeleteCategory_ShouldReturnTrue()
        {
            // Arrange
            var categoryId = 1;

            _unitOfWorkMock.Setup(u => u.Category.DeleteAsync(categoryId))
                .ReturnsAsync(true);

            // Act
            var result = await _categoryApplication.DeleteCategory(categoryId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data);
        }

    }
}
