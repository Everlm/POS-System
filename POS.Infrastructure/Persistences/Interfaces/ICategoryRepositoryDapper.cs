using POS.Domain.Entities;

namespace POS.Infrastructure.Persistences.Interfaces;

public interface ICategoryRepositoryDapper
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task<Category?> GetCategoryByIdAsync(int categoryId);    
    Task<bool> CreateCategoryAsync(Category category);      
    Task<bool> UpdateCategoryAsync(Category category);           
    Task<bool> SoftDeleteCategoryAsync(int auditDeleteUser, int categoryId);
    Task<bool> HardDeleteCategoryAsync(int categoryId);
}
