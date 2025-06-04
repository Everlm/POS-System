using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS.Application.Dtos.Auth.Request;
using POS.Application.Dtos.User.Request;
using POS.Application.Interfaces;

namespace POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthApplication _authApplication;

        public AuthController(IAuthApplication authApplication)
        {
            _authApplication = authApplication;
        }


        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto requestDto, [FromQuery] string authType)
        {
            var response = await _authApplication.Login(requestDto, authType);
            return Ok(response);
        }


        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequestDto requestDto)
        {
            var response = await _authApplication.RefreshToken(requestDto);
            return Ok(response);
        }


        [HttpPost("LoginWithGoogle")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWithGoogle([FromBody] string credencials, [FromQuery] string authType)
        {
            var response = await _authApplication.LoginWithGoogle(credencials, authType);
            return Ok(response);
        }

        [HttpPut("Logout")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout([FromBody] LogoutRequestDto requestDto)
        {
            var response = await _authApplication.Logout(requestDto);
            return Ok(response);
        }
    }
}
