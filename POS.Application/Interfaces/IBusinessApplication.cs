using POS.Application.Commons.Base;
using POS.Application.Dtos.Business.Request;
using POS.Application.Dtos.Business.Response;
using POS.Infrastructure.Commons.Bases.Request;
using POS.Infrastructure.Commons.Bases.Response;

namespace POS.Application.Interfaces
{
    public interface IBusinessApplication
    {
        Task<BaseResponse<BaseEntityResponse<BusinessResponseDto>>> ListBusiness(BaseFiltersRequest filters);
        Task<BaseResponse<BusinessResponseDto>> GetBusinessById(int businessId);
        Task<BaseResponse<bool>> RegisterBusiness(BusinessRequestDto requestDto);
        Task<BaseResponse<bool>> EditBusiness(BusinessRequestDto requestDto, int businessId);
        Task<BaseResponse<bool>> DeleteBusiness(int businessId);
    }
}
