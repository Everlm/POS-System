using POS.Domain.Entities;

namespace POS.Infrastructure.Persistences.Interfaces;

public interface ICategoryRepositoryDapper
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task<Category?> GetCategoryByIdAsync(int categoryId);    
    Task<int> RegisterCategoryAsync(Category category);      
    Task<int> EditCategoryAsync(Category category);           
    Task<int> DeleteCategoryAsync(int categoryId);

}
