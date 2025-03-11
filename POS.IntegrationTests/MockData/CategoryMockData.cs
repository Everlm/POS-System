using POS.Application.Dtos.Category.Request;
using POS.Domain.Entities;


namespace POS.IntegrationTests.MockData
{
    public class CategoryMockData
    {
        public static CategoryRequestDto GetCategoryEmptyRequest()
        {
            return new CategoryRequestDto
            {
                Name = "",
                Description = "",
                State = 1
            };
        }

        public static CategoryRequestDto GetCategoryRequest()
        {
            return new CategoryRequestDto
            {
                Name = "New category",
                Description = "Description",
                State = 1
            };
        }

    }
}
