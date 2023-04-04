using POS.Application.Commons.Base;
using POS.Application.Dtos.Purchase.Request;
using POS.Application.Dtos.Purchase.Response;
using POS.Infrastructure.Commons.Bases.Request;
using POS.Infrastructure.Commons.Bases.Response;

namespace POS.Application.Interfaces
{
    public interface IPurchaseApplication
    {
        Task<BaseResponse<BaseEntityResponse<PurchaseResponseDto>>> ListPurchase(BaseFiltersRequest filters);
        Task<BaseResponse<PurchaseResponseDto>> GetPurchaseById(int PurchaseId);
        Task<BaseResponse<bool>> RegisterPurchase(PurchaseRequestDto requestDto);
    }
}
