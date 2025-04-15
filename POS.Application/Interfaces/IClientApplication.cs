using POS.Application.Commons.Bases.Request;
using POS.Application.Commons.Bases.Response;
using POS.Application.Commons.Select.Response;
using POS.Application.Dtos.Client.Request;
using POS.Application.Dtos.Client.Response;

namespace POS.Application.Interfaces
{
    public interface IClientApplication
    {
        Task<BaseResponse<IEnumerable<ClientResponseDto>>> ListClient(BaseFiltersRequest filters);
        Task<BaseResponse<IEnumerable<SelectResponse>>> ListSelectClient();
        Task<BaseResponse<ClientResponseDto>> GetClientById(int clientId);
        Task<BaseResponse<bool>> CreateClient(ClientRequestDto requestDto);
        Task<BaseResponse<bool>> UpdateClient(ClientRequestDto requestDto, int clientId);
        Task<BaseResponse<bool>> DeleteClient(int clientId);
    }
}
