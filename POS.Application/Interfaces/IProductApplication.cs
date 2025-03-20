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
        //Task<BaseResponse<bool>> EditWarehouse(WarehouseRequestDto requestDto, int warehouseId);
        //Task<BaseResponse<bool>> RemoveWarehouse(int warehouseId);
    }
}
