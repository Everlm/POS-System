using POS.Application.Commons.Base;
using POS.Application.Dtos.Product.Request;
using POS.Application.Dtos.Sale.Response;
using POS.Infrastructure.Commons.Bases.Request;
using POS.Infrastructure.Commons.Bases.Response;

namespace POS.Application.Interfaces
{
    public interface ISaleApplication
    {
        Task<BaseResponse<BaseEntityResponse<SaleResponseDto>>> ListSales(BaseFiltersRequest filters);
        Task<BaseResponse<SaleResponseDto>> GetSaleById(int SaleId);
        Task<BaseResponse<bool>> RegisterSale(SaleRequestDto requestDto);
    }
}
