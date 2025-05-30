
namespace POS.Application.Dtos.Auth.Request
{
    public class TokenRequestDto
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
    }
}