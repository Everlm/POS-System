using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using POS.Application.Commons.Bases.Response;
using POS.Application.Dtos.User.Request;
using POS.Application.Interfaces;
using POS.Domain.Entities;
using POS.Infrastructure.Persistences.Interfaces;
using POS.Utilities.AppSettings;
using POS.Utilities.Static;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

        public async Task<BaseResponse<string>> Login(TokenRequestDto requestDto, string authType)
        {
            var response = new BaseResponse<string>();

            try
            {
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

                if (user is not null)
                {
                    if (BC.Verify(requestDto.Password, user.Password))
                    {
                        response.IsSuccess = true;
                        response.Data = GenerateToken(user);
                        response.Message = ReplyMessage.MESSAGE_TOKEN;
                        return response;
                    }
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchLogger.Log(ex.Message);
            }

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
                new Claim(JwtRegisteredClaimNames.Iat, Guid.NewGuid().ToString(), ClaimValueTypes.Integer64)
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
    }
}
