using POS.Domain.Entities;
using POS.Infrastructure.Persistences.Interfaces;
using POS.Infrastructure.Persistences.StoredProcedures;
using POS.Utilities.Static;

namespace POS.Infrastructure.Persistences.Repositories;

public class CategoryRepositoryDapper : ICategoryRepositoryDapper
{
    private readonly IStoredProcedureService _storedProcedureService;
    public CategoryRepositoryDapper(IStoredProcedureService storedProcedureService)
    {
        _storedProcedureService = storedProcedureService;
    }
    public Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return _storedProcedureService.ExecuteQueryAsync<Category>(StoredProcedureNames.GetAllCategories);
    }

    public async Task<Category?> GetCategoryByIdAsync(int categoryId)
    {
        var result = await _storedProcedureService.ExecuteQueryAsync<Category>(
            StoredProcedureNames.GetCategoryById,
            new { CategoryId = categoryId }
        );

        return result.FirstOrDefault();
    }

    public async Task<bool> CreateCategoryAsync(Category category)
    {
        var affectedRows = await _storedProcedureService.ExecuteNonQueryAsync(
            StoredProcedureNames.CreateCategory,
            new { category.Name, category.Description, category.State, category.AuditCreateUser }
        );

        return affectedRows > 0;
    }

    public async Task<bool> UpdateCategoryAsync(Category category)
    {
        var affectedRows = await _storedProcedureService.ExecuteNonQueryAsync(
            StoredProcedureNames.UpdateCategory,
            new { category.Id, category.Name, category.Description, category.State, category.AuditUpdateUser }
        );

        return affectedRows > 0;
    }

    public async Task<bool> SoftDeleteCategoryAsync(int auditDeleteUser, int categoryId)
    {
        var affectedRows = await _storedProcedureService.ExecuteNonQueryAsync(
            StoredProcedureNames.DeleteCategory,
            new { CategoryId = categoryId, AuditDeleteUser = auditDeleteUser }
        );

        return affectedRows > 0;
    }

    public async Task<bool> HardDeleteCategoryAsync(int categoryId)
    {
        var affectedRows = await _storedProcedureService.ExecuteNonQueryAsync(
            StoredProcedureNames.HardDeleteCategory,
            new { CategoryId = categoryId }
        );

        return affectedRows > 0;
    }

}
