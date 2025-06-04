using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using POS.Application.Commons.Bases.Response;
using POS.Application.Dtos.Auth;
using POS.Application.Dtos.Auth.Request;
using POS.Application.Dtos.Auth.Response;
using POS.Application.Dtos.User.Request;
using POS.Application.Interfaces;
using POS.Domain.Entities;
using POS.Infrastructure.Persistences.Interfaces;
using POS.Utilities.AppSettings;
using POS.Utilities.Static;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WatchDog;
using BC = BCrypt.Net.BCrypt;

namespace POS.Application.Services
{
    public class AuthApplication : IAuthApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly AppSettings _appSettings;

        public AuthApplication(IUnitOfWork unitOfWork, IConfiguration config, IOptions<AppSettings> appSettings)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _appSettings = appSettings.Value;
        }

        public async Task<BaseResponse<LoginResponseDto>> Login(LoginRequestDto requestDto, string authType)
        {
            var response = new BaseResponse<LoginResponseDto>();

            var user = await _unitOfWork.User.UserByEmail(requestDto.Email!);

            if (user is null)
            {
                response.IsSuccess = false;
                response.Message += ReplyMessage.MESSAGE_TOKEN_ERROR;
                return response;
            }

            if (user.AuthType != authType)
            {
                response.IsSuccess = false;
                response.Message += ReplyMessage.MESSAGE_AUTHTYPE_ERROR;
                return response;
            }

            if (!BC.Verify(requestDto.Password, user.Password))
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_LOGIN_FAILED;
                return response;
            }

            var refreshToken = GenerateRefreshToken();
            var token = GenerateToken(user);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _unitOfWork.User.EditAsync(user);

            response.IsSuccess = true;
            response.Data = new LoginResponseDto
            {
                Token = token,
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = user.RefreshTokenExpiryTime
            };
            response.Message = ReplyMessage.MESSAGE_TOKEN;
            return response;
        }

        public async Task<BaseResponse<LoginResponseDto>> RefreshToken(TokenRequestDto requestDto)
        {
            var response = new BaseResponse<LoginResponseDto>();

            var principalClaimsFromToken = GetPrincipalFromExpiredToken(requestDto.Token);

            if (principalClaimsFromToken == null)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_TOKEN_ERROR;
                return response;
            }

            var userEmail = principalClaimsFromToken.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userEmail))
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_TOKEN_ERROR;
                return response;
            }

            var user = await _unitOfWork.User.UserByEmail(userEmail);

            if (user is null)
            {
                response.IsSuccess = false;
                response.Message += ReplyMessage.MESSAGE_TOKEN_ERROR;
                return response;
            }

            if (user.RefreshToken != requestDto.RefreshToken)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_INVALID_REFRESH_TOKEN;
                return response;
            }

            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_REFRESH_TOKEN_EXPIRED;
                return response;
            }

            var newToken = GenerateToken(user);
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _unitOfWork.User.EditAsync(user);

            response.IsSuccess = true;
            response.Data = new LoginResponseDto
            {
                Token = newToken,
                RefreshToken = newRefreshToken,
                RefreshTokenExpiryTime = user.RefreshTokenExpiryTime
            };

            response.Message = ReplyMessage.MESSAGE_TOKEN;
            return response;

        }

        public async Task<BaseResponse<string>> LoginWithGoogle(string credentials, string authType)
        {
            var response = new BaseResponse<string>();

            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>
                    {
                        _appSettings.ClientId!
                    }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(credentials, settings);
                var user = await _unitOfWork.User.UserByEmail(payload.Email);

                if (user is null)
                {
                    response.IsSuccess = false;
                    response.Message += ReplyMessage.MESSAGE_AUTHGOOGLE_ERROR;
                    return response;
                }

                if (user.AuthType != authType)
                {
                    response.IsSuccess = false;
                    response.Message += ReplyMessage.MESSAGE_AUTHTYPE_ERROR;
                    return response;
                }

                response.IsSuccess = true;
                response.Data = GenerateToken(user);
                response.Message = ReplyMessage.MESSAGE_TOKEN;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchLogger.Log(ex.Message);
            }

            return response;
        }

        public async Task<BaseResponse<bool>> Logout(LogoutRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();

            var principalClaimsFromToken = GetPrincipalFromExpiredToken(requestDto.Token);

            if (principalClaimsFromToken == null)
            {
                response.IsSuccess = false;
                response.Data = false;
                response.Message = ReplyMessage.MESSAGE_TOKEN_ERROR;
                return response;
            }

            var userEmail = principalClaimsFromToken.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userEmail))
            {
                response.IsSuccess = false;
                response.Data = false;
                response.Message = ReplyMessage.MESSAGE_TOKEN_ERROR;
                return response;
            }

            var user = await _unitOfWork.User.UserByEmail(userEmail);

            if (user is null)
            {
                response.IsSuccess = false;
                response.Data = false;
                response.Message += ReplyMessage.MESSAGE_TOKEN_ERROR;
                return response;
            }

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;
            response.IsSuccess = true;
            response.Data = await _unitOfWork.User.EditAsync(user);
            response.Message = ReplyMessage.MESSAGE_QUERY;
            return response;

        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principalToken = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return principalToken;
        }

        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]));

            var Credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Email!),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.UserName!),
                new Claim(JwtRegisteredClaimNames.GivenName, user.Email!),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,
                    new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),

            };

            var token = new JwtSecurityToken(

                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(int.Parse(_config["Jwt:Expiret"])),
                notBefore: DateTime.UtcNow,
                signingCredentials: Credentials

                );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

    }
}
