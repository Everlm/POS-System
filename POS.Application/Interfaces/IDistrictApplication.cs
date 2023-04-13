using POS.Application.Commons.Base;
using POS.Application.Dtos.District.Request;
using POS.Application.Dtos.District.Response;
using POS.Infrastructure.Commons.Bases.Request;
using POS.Infrastructure.Commons.Bases.Response;

namespace POS.Application.Interfaces
{
    public interface IDistrictApplication
    {
        Task<BaseResponse<BaseEntityResponse<DistrictResponseDto>>> ListDistricts(BaseFiltersRequest filters);
        Task<BaseResponse<DistrictResponseDto>> GetDistrictById(int districtId);
        Task<BaseResponse<bool>> RegisterDistrict(DistrictRequestDto requestDto);
        Task<BaseResponse<bool>> EditDistrict(DistrictRequestDto requestDto, int districtId);
        Task<BaseResponse<bool>> DeleteDistrict(int districtId);
    }
}
