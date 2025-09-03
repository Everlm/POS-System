using AutoMapper;
using Moq;
using POS.Application.Commons.Bases.Request;
using POS.Application.Commons.Ordering;
using POS.Application.Documents;
using POS.Application.Mappers;
using POS.Application.Services;
using POS.Application.Validators.Category;
using POS.Domain.Entities;
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
        private readonly Mock<IOrderingQuery> _orderingQueryMock;
        private readonly Mock<IDocumentGenerator> _documentGeneratorMock;
        private readonly Mock<IDocumentFactory> _documentFactoryMock;
        private readonly Mock<ICategoryRepositoryDapper> _categoryRepositoryDapperMock;

        public CategoryApplicationTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<CategoryMappingsProfile>());
            _mapper = config.CreateMapper();

            _validateRules = new CategoryValidator();

            _orderingQueryMock = new Mock<IOrderingQuery>();
            _documentGeneratorMock = new Mock<IDocumentGenerator>();
            _documentFactoryMock = new Mock<IDocumentFactory>();
            _categoryRepositoryDapperMock = new Mock<ICategoryRepositoryDapper>();

            _categoryApplication = new CategoryApplication(
                _unitOfWorkMock.Object,
                _mapper,
                _validateRules,
                _orderingQueryMock.Object,
                _documentGeneratorMock.Object,
                _documentFactoryMock.Object,
                _categoryRepositoryDapperMock.Object
            );
        }

        [Fact(Skip = "Error en el ordering")]
        public async Task ListCategories_ShouldReturnListOfCategories()
        {
            // Arrange
            var filters = new BaseFiltersRequest();
            var categoriesList = CategoryMockData.GetSampleCategories();

            _unitOfWorkMock.Setup(u => u.Category.GetAllQueryable())
                .Returns(categoriesList.AsQueryable());

            _orderingQueryMock.Setup(o => o.Ordering(It.IsAny<BaseFiltersRequest>(), It.IsAny<IQueryable<Category>>(), It.IsAny<bool>()))
                .Returns((BaseFiltersRequest _, IQueryable<Category> q, bool _) => q);

            // Act
            var result = await _categoryApplication.ListCategories(filters);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(categoriesList.Count, result.Data?.Count());
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

            var existingCategory = CategoryMockData.GetSampleCategory(categoryId);

            _unitOfWorkMock.Setup(u => u.Category.GetByIdAsync(categoryId))
                .ReturnsAsync(existingCategory);

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
