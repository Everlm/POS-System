using POS.Application.Commons.Base;
using POS.Application.Dtos.Client.Request;
using POS.Application.Dtos.Client.Response;
using POS.Infrastructure.Commons.Bases.Request;
using POS.Infrastructure.Commons.Bases.Response;

namespace POS.Application.Interfaces
{
    public interface IClientApplication
    {
        Task<BaseResponse<BaseEntityResponse<ClientResponseDto>>> ListClients(BaseFiltersRequest filters);
        Task<BaseResponse<ClientResponseDto>> GetClientById(int clientId);
        Task<BaseResponse<bool>> Registerclient(ClientRequestDto requestDto);
        Task<BaseResponse<bool>> EditClient(ClientRequestDto requestDto, int clientId);
        Task<BaseResponse<bool>> DeleteClient(int clientId);
    }
}
