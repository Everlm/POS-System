using POS.Application.Commons.Base;
using POS.Application.Dtos.Province.Request;
using POS.Application.Dtos.Province.Response;
using POS.Infrastructure.Commons.Bases.Request;
using POS.Infrastructure.Commons.Bases.Response;

namespace POS.Application.Interfaces
{
    public interface IProvinceApplication
    {
        Task<BaseResponse<BaseEntityResponse<ProvinceResponseDto>>> ListProvinces(BaseFiltersRequest filters);
        Task<BaseResponse<ProvinceResponseDto>> GetProvinceById(int provinceId);
        Task<BaseResponse<bool>> Registerprovince(ProvinceRequestDto requestDto);
        Task<BaseResponse<bool>> EditProvince(ProvinceRequestDto requestDto, int provinceId);
        Task<BaseResponse<bool>> DeleteProvince(int provinceId);
    }
}
