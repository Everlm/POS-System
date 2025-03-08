using POS.Application.Dtos.Category.Request;
using POS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.UnitTest.MockData
{
    public class CategoryMockData
    {
        public static List<Category> GetSampleCategories() =>
           new()
           {
                new Category { Id = 1, Name = "Category 1" },
                new Category { Id = 2, Name = "Category 2" }
           };

        public static Category GetSampleCategory(int id) =>
            new() { Id = id, Name = $"Category {id}" };

        public static CategoryRequestDto GetSampleCategoryRequestDto() =>
            new() { Name = "Sample Category", Description = "Sample Description", State = 1 };
    }
}
