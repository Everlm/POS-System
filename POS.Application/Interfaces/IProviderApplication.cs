using POS.Application.Commons.Bases.Request;
using POS.Application.Commons.Bases.Response;
using POS.Application.Commons.Select.Response;
using POS.Application.Dtos.Provider.Request;
using POS.Application.Dtos.Provider.Response;

namespace POS.Application.Interfaces
{
    public interface IProviderApplication
    {
        Task<BaseResponse<IEnumerable<ProviderResponseDto>>> ListProviders(BaseFiltersRequest filters);
        Task<BaseResponse<IEnumerable<SelectResponse>>> GetAllProviders();
        Task<BaseResponse<ProviderByIdResponseDto>> GetProviderById(int providerId);
        Task<BaseResponse<bool>> RegisterProvider(ProviderRequestDto requestDto);
        Task<BaseResponse<bool>> EditProvider(ProviderRequestDto requestDto, int providerId);
        Task<BaseResponse<bool>> DeleteProvider(int providerId);
    }
}
