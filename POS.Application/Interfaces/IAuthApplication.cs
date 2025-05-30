using POS.Application.Commons.Bases.Response;
using POS.Application.Dtos.Auth.Request;
using POS.Application.Dtos.Auth.Response;
using POS.Application.Dtos.User.Request;

namespace POS.Application.Interfaces
{
    public interface IAuthApplication
    {
        Task<BaseResponse<LoginResponseDto>> Login(LoginRequestDto requestDto, string authType);
        Task<BaseResponse<LoginResponseDto>> RefreshToken(TokenRequestDto requestDto);
        Task<BaseResponse<string>> LoginWithGoogle(string credentials, string authType);
    }
}
