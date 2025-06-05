namespace POS.Application.Dtos.Auth.Response
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public string[] Roles { get; set; } = Array.Empty<string>();
    }
}