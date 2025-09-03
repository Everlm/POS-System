using POS.Application.Commons.Bases.Request;
using POS.Application.Commons.Bases.Response;
using POS.Application.Commons.Select.Response;
using POS.Application.Dtos.Category.Request;
using POS.Application.Dtos.Category.Response;

namespace POS.Application.Interfaces
{
    public interface ICategoryApplication
    {  
        Task<BaseResponse<byte[]>> GenerateCategoriesPdfDocument();
        Task<BaseResponse<IEnumerable<CategoryResponseDto>>> ListCategories(BaseFiltersRequest filters);
        Task<BaseResponse<IEnumerable<SelectResponse>>> ListSelectCategories();
        Task<BaseResponse<IEnumerable<SelectResponse>>> SPListSelectCategories();
        Task<BaseResponse<CategoryResponseDto>> GetCategoryById(int categoryId);
        Task<BaseResponse<CategoryResponseDto>> SPGetCategoryById(int categoryId);
        Task<BaseResponse<bool>> RegisterCategory(CategoryRequestDto requestDto);
        Task<BaseResponse<bool>> SPCreateCategory(CategoryRequestDto requestDto);
        Task<BaseResponse<bool>> EditCategory(CategoryRequestDto requestDto, int categoryId);
        Task<BaseResponse<bool>> SPUpdateCategory(CategoryRequestDto requestDto, int categoryId);
        Task<BaseResponse<bool>> DeleteCategory(int categoryId);
        Task<BaseResponse<bool>> SPDeleteCategory(int categoryId);
        Task<BaseResponse<bool>> SPHardDeleteCategory(int categoryId);
    }
}
