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

    public async Task<int> RegisterCategoryAsync(Category category)
    {
        return await _storedProcedureService.ExecuteNonQueryAsync(
            StoredProcedureNames.RegisterCategory,
            new { category.Name, category.Description, category.State}
        );
    }

    public async Task<int> EditCategoryAsync(Category category)
    {
        return await _storedProcedureService.ExecuteNonQueryAsync(
            StoredProcedureNames.EditCategory,
            new { category.Id, category.Name, category.Description }
        );
    }

    public async Task<int> DeleteCategoryAsync(int categoryId)
    {
        return await _storedProcedureService.ExecuteNonQueryAsync(
            StoredProcedureNames.DeleteCategory,
            new { CategoryId = categoryId }
        );
    }
}
