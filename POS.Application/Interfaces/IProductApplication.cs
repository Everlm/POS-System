using POS.Application.Commons.Bases.Request;
using POS.Application.Commons.Bases.Response;
using POS.Application.Dtos.Product.Request;
using POS.Application.Dtos.Product.Response;

namespace POS.Application.Interfaces
{
    public interface IProductApplication
    {
        Task<BaseResponse<IEnumerable<ProductResponseDto>>> ListProducts(BaseFiltersRequest filters);
        Task<BaseResponse<ProductByIdResponseDto>> GetProductById(int productId);
        Task<BaseResponse<bool>> CreateProduct(ProductRequestDto requestDto);
        Task<BaseResponse<bool>> UpdateProduct(ProductRequestDto requestDto, int productId);
        Task<BaseResponse<bool>> DeleteProduct(int productId);
    }
}
