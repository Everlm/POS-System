using POS.Application.Commons.Base;
using POS.Application.Dtos.Product.Request;
using POS.Application.Dtos.Product.Response;
using POS.Infrastructure.Commons.Bases.Request;
using POS.Infrastructure.Commons.Bases.Response;

namespace POS.Application.Interfaces
{
    public interface IProductApplication
    {
        Task<BaseResponse<BaseEntityResponse<ProductResponseDto>>> ListProducts(BaseFiltersRequest filters);
        Task<BaseResponse<ProductResponseDto>> GetProductById(int productId);
        Task<BaseResponse<bool>> RegisterProduct(ProductRequestDto requestDto);
        Task<BaseResponse<bool>> EditProduct(ProductRequestDto requestDto, int productId);
        Task<BaseResponse<bool>> DeleteProduct(int productId); 

    }
}
